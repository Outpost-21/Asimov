using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class Comp_AutoCrafter : ThingComp, IThingHolder
    {
        public CompProperties_AutoCrafter Props => (CompProperties_AutoCrafter)props;

        public ThingOwner<Thing> ingredients = new ThingOwner<Thing>();

        public ThingOrderProcessor orderProcessor;

        private CompPowerTrader powerComp;

        public int workTick = -1;
        public int workTickMax = 0;

        public ProducerStatus currentStatus = ProducerStatus.idle;

        public AutomatonRecipeDef curRecipe = null;

        public RepeatMode repeatMode = RepeatMode.none;
        public int repeatCount = 0;
        public int repeatTarget = 0;
        public bool suspended = false;

        public bool hasOrder = false;

        public float WorkProgress => (1f - (float)((float)workTick / (float)workTickMax));

        public void GetChildHolders(List<IThingHolder> outChildren) { }

        public ThingOwner GetDirectlyHeldThings() { return ingredients; }

        public string CurrentStatusLabel()
        {
            string result = " ";
            if (suspended)
            {
                result = "Suspended: Request Met";
            }
            else if (currentStatus == ProducerStatus.idle)
            {
                result = "Idle";
            }
            else if (currentStatus == ProducerStatus.awaitingResources && orderProcessor.PendingRequests() != null)
            {
                result = "Awaiting Resources: ";
                foreach (ThingOrderRequest thing in orderProcessor.PendingRequests())
                {
                    result = result + "\n" + thing.amount.ToString() + " " + thing.thingDef.LabelCap;
                }
            }
            else if (currentStatus == ProducerStatus.working)
            {
                result = "Working: " + WorkProgress.ToStringPercent();
            }
            else if (currentStatus == ProducerStatus.producing)
            {
                result = "Producing";
            }
            return result;
        }

        public string RepeatString()
        {
            if (repeatMode == RepeatMode.none)
            {
                return "Repeat Never";
            }
            else if (repeatMode == RepeatMode.until)
            {
                return "Repeat Until X";
            }
            else if (repeatMode == RepeatMode.times)
            {
                return "Repeat X Times";
            }
            else
            {
                return "Repeat Forever";
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            powerComp = parent.TryGetComp<CompPowerTrader>();

            if (workTick <= 0)
            {
                ResetWorkTick();
            }

            if (!respawningAfterLoad)
            {
                orderProcessor = new ThingOrderProcessor(ingredients);
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            this.GetProducerStatus();

            this.ShouldBeSuspended();
            if (this.HasRecipe() && !this.hasOrder)
            {
                //Update costs.
                if (!curRecipe.costList.NullOrEmpty())
                {
                    this.orderProcessor.requestedItems.Clear();

                    foreach (ThingDefCountClass cost in curRecipe.costList)
                    {
                        ThingOrderRequest costCopy = new ThingOrderRequest();
                        costCopy.thingDef = cost.thingDef;
                        costCopy.amount = cost.count;

                        this.orderProcessor.requestedItems.Add(costCopy);
                    }

                    this.hasOrder = true;
                }
            }

            if (this.curRecipe != null && this.IsWorking() && this.workTick <= 0)
            {
                this.ProduceFromRecipe();
            }

            if (IsWorking() && !suspended)
            {

                this.workTick--;
            }
        }

        public void ProduceFromRecipe()
        {
            if (curRecipe != null && IsWorking())
            {
                if (parent.InteractionCell == null)
                {
                    LogUtil.LogError("Interaction Cell not defined on " + this.parent.def.defName + ", output requires an interaction cell.");
                }
                SpawnPawn(parent.InteractionCell, parent.Map);
            }
            ingredients.ClearAndDestroyContents();
            RepeatRecipe();
            ResetWorkTick();
        }
        public void SpawnPawn(IntVec3 loc, Map map)
        {
            PawnKindDef pawnKind = curRecipe.pawnKind;

            PawnGenerationRequest request = new PawnGenerationRequest(kind: pawnKind, faction: Faction.OfPlayer, forceGenerateNewPawn: true, forceNoIdeo:true);
            Pawn newThing = PawnGenerator.GeneratePawn(request);
            newThing.ageTracker.DebugSetAge(0);
            GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
        }

        public void RepeatRecipe()
        {
            if (curRecipe != null)
            {
                if (repeatMode == RepeatMode.none)
                {
                    curRecipe = null;
                }
                else if (repeatMode == RepeatMode.times)
                {
                    repeatCount--;
                }
            }
        }

        public void ShouldBeSuspended()
        {
            if ((repeatMode == RepeatMode.until && CheckRepeatCountProducts(curRecipe) >= repeatTarget) || (repeatMode == RepeatMode.times && repeatCount <= 0))
            {
                suspended = true;
            }
            else
            {
                suspended = false;
            }
        }

        public int CheckRepeatCountProducts(AutomatonRecipeDef recipe)
        {
            return parent.Map.mapPawns.AllPawns.Count((Pawn x) => (x.Faction == Faction.OfPlayer && x.def.defName == recipe.pawnKind.race.defName));
        }

        public void ResetWorkTick()
        {
            if (orderProcessor != null)
            {
                orderProcessor.requestedItems.Clear();
                hasOrder = false;
            }
            int result = -1;
            if (curRecipe != null)
            {
                result = (int)(curRecipe.workAmount * Props.craftingTimeMulti);
            }
            workTick = result;
            workTickMax = result;
        }

        public void CancelRecipe()
        {
            ingredients.TryDropAll(this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near);
            this.curRecipe = null;
            repeatMode = RepeatMode.none;
            repeatTarget = 0;
        }

        public bool ShouldProduceThisTick()
        {
            if (powerComp != null)
            {
                if (powerComp.PowerOn && Current.Game.tickManager.TicksGame >= workTick)
                {
                    return true;
                }
            }
            else if (suspended)
            {
                return false;
            }
            else if (Current.Game.tickManager.TicksGame >= workTick)
            {
                return true;
            }
            return false;
        }

        public void GetProducerStatus()
        {
            if (!IsPowered() || !HasRecipe())
            {
                currentStatus = ProducerStatus.idle;
            }
            else if ((HasCosts() || AwaitingCosts()) && !HasIngredients())
            {
                currentStatus = ProducerStatus.awaitingResources;
            }
            else if (IsWorking())
            {
                currentStatus = ProducerStatus.working;
            }
            else
            {
                currentStatus = ProducerStatus.producing;
            }
        }

        public bool IsPowered()
        {
            if (powerComp != null && !powerComp.PowerOn)
            {
                return false;
            }
            return true;
        }

        public bool HasRecipe()
        {
            if (curRecipe != null)
            {
                return true;
            }
            return false;
        }

        public bool HasCosts()
        {
            if (HasRecipe() && curRecipe.costList != null)
            {
                return true;
            }
            return false;
        }

        public bool AwaitingCosts()
        {
            if (HasCosts() && orderProcessor.PendingRequests().Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool HasIngredients()
        {
            if (!HasCosts() || (orderProcessor.PendingRequests() == null || (orderProcessor.PendingRequests() != null && orderProcessor.PendingRequests().Count() <= 0)))
            {
                return true;
            }
            return false;
        }

        public bool IsWorking()
        {
            if (HasIngredients() && IsPowered() && workTick > -2)
            {
                return true;
            }
            return false;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref workTick, "workTick");
            Scribe_Values.Look(ref workTickMax, "workTickMax");
            Scribe_Values.Look(ref repeatMode, "repeatMode", RepeatMode.none);
            Scribe_Values.Look(ref hasOrder, "hasOrder");
            Scribe_Values.Look(ref currentStatus, "currentStatus");
            Scribe_Values.Look(ref repeatCount, "repeatCount");
            Scribe_Values.Look(ref repeatTarget, "repeatTarget");
            Scribe_Values.Look(ref suspended, "suspended");

            Scribe_Defs.Look(ref curRecipe, "currentRecipe");

            Scribe_Deep.Look(ref ingredients, "ingredients");
            Scribe_Deep.Look(ref orderProcessor, "orderProcessor", ingredients);
        }
    }
}
