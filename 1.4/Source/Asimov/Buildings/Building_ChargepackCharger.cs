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
    public class Building_ChargepackCharger : Building, IThingHolder
    {
        public CompPowerTrader compPowerTrader;

        public CompRefuelable compFuelable;

        public DefModExt_ChargerGraphics modExt;

        public bool IsPowered => (compPowerTrader == null || compPowerTrader.PowerOn) && (compFuelable == null || compFuelable.HasFuel);

        public ThingOwner innerContainer = null;

        public const int processCount = 10;
        public const int processTickCost = 10000;

        public int processTick = -1;
        public bool processManualStart = false;

        public ProcessState processState;

        public Graphic graphicWorkingInt;
        public Graphic graphicFinishedInt;

        public override Graphic Graphic
        {
            get
            {
                if(modExt != null)
                {
                    switch (processState)
                    {
                        case ProcessState.Working:
                            return GraphicWorking;
                        case ProcessState.Finished:
                            return GraphicFinished;
                        default:
                            return base.Graphic;
                    }
                }
                return base.Graphic;
            }
        }

        public Graphic GraphicWorking
        {
            get
            {
                if(graphicWorkingInt == null)
                {
                    if (def.graphicData == null)
                    {
                        return BaseContent.BadGraphic;
                    }
                    graphicWorkingInt = modExt.workingGraphicData.GraphicColoredFor(this);
                }
                return graphicWorkingInt;
            }
        }

        public Graphic GraphicFinished
        {
            get
            {
                if (graphicFinishedInt == null)
                {
                    if (def.graphicData == null)
                    {
                        return BaseContent.BadGraphic;
                    }
                    graphicFinishedInt = modExt.finishedGraphicData.GraphicColoredFor(this);
                }
                return graphicFinishedInt;
            }
        }

        public bool IsFull => GetDirectlyHeldThings().TotalStackCount >= processCount;

        public bool IsEmpty => GetDirectlyHeldThings().TotalStackCount == 0;

        public float CurrentPercentageFloat => (float)processTick / (float)processTickCost;

        public string CurrentPercentage => CurrentPercentageFloat.ToStringPercent();

        public Building_ChargepackCharger()
        {
             innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
        }

        public override string GetInspectString()
        {
            string inspectString = base.GetInspectString();
            string text = "\n";
            text += "Asimov.CurrentState".Translate() + " " + GetStateString();
            text += "\n" + (processState == ProcessState.Finished ? AsimovDefOf.Asimov_Chargepack : AsimovDefOf.Asimov_Chargepack_Empty).label + $": ( {GetDirectlyHeldThings().TotalStackCount} / {processCount} )";
            return inspectString + text;

        }

        public string GetStateString()
        {
            switch (processState)
            {
                case ProcessState.Inactive: return "Asimov.State_Inactive".Translate();
                case ProcessState.AwaitingInput: return "Asimov.State_AwaitingInput".Translate();
                case ProcessState.Working: return "Asimov.State_Working".Translate(CurrentPercentage);
                case ProcessState.Finished: return "Asimov.State_Finished".Translate();
                case ProcessState.ProductRemoved: return "Asimov.State_ProductRemoved".Translate();
                default: return "Asimov.State_Invalid".Translate();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            Scribe_Values.Look(ref processState, "processState");
            Scribe_Values.Look(ref processTick, "processTick");
            Scribe_Values.Look(ref processManualStart, "processManualStart");
        }

        public override void Tick()
        {
            base.Tick();
            if(Find.TickManager.TicksGame % 60 == 0)
            {
                WorkTick(60);
            }
        }

        public override void TickRare()
        {
            base.TickRare();
            WorkTick(250);
        }

        public void WorkTick(int tickCount)
        {
            switch (processState)
            {
                case ProcessState.Inactive:
                    State_Inactive();
                    break;
                case ProcessState.AwaitingInput:
                    State_AwaitingInput();
                    break;
                case ProcessState.Working:
                    State_Working(tickCount);
                    break;
                case ProcessState.Finished:
                    State_Finished();
                    break;
                default:
                    processState = ProcessState.Inactive;
                    break;
            }
        }

        public void State_Inactive()
        {
            processTick = 0;
            processManualStart = false;
            processState = ProcessState.AwaitingInput;
            DirtyMesh();
        }

        public void State_AwaitingInput()
        {
            if (IsFull || processManualStart)
            {
                processState = ProcessState.Working;
                DirtyMesh();
            }
        }

        public void State_Working(int tickCount)
        {
            if (IsPowered)
            {
                processTick += tickCount;
                if(processTick >= processTickCost)
                {
                    processState = ProcessState.Finished;
                    DirtyMesh();
                }
            }
        }

        public void State_Finished()
        {
            if (IsEmpty)
            {
                processState = ProcessState.Inactive;
                DirtyMesh();
            }
        }

        public void ResetWork()
        {
            processTick = -1;
            processState = ProcessState.Inactive;
            DirtyMesh();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach(Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if(!IsFull && !IsEmpty)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Asimov.BeginProcessingEarlyLabel".Translate(),
                    defaultDesc = "Asimov.BeginProcessingEarlyDesc".Translate(),
                    //icon = MaterialPool.MatFrom("Asimov/UI/EarlyProcessing").mainTexture,
                    action = delegate { processManualStart = true; }
                };
            }
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public bool TryAcceptThing(Thing thing, int Count = 0)
        {
            bool flag;
            if (thing.holdingOwner != null)
            {
                if (Count == 0)
                {
                    thing.holdingOwner.TryTransferToContainer(thing, innerContainer, thing.stackCount);
                }
                else
                {
                    Thing item = thing.SplitOff(Count);
                    innerContainer.TryAdd(item);
                }
                DirtyMesh();
                flag = true;
            }
            else
            {
                if (Count == 0)
                {
                    flag = innerContainer.TryAdd(thing);
                }
                else
                {
                    Thing item2 = thing.SplitOff(Count);
                    flag = innerContainer.TryAdd(item2);
                }
                DirtyMesh();
            }
            bool result;
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    DirtyMesh();
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void DirtyMesh()
        {
            Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
        }

        public void DestroyContents()
        {
            innerContainer.ClearAndDestroyContents();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            EnergyUtil.GetEnergyNeedWorldComp.AddChargepackCharger(this, map);
            compPowerTrader = GetComp<CompPowerTrader>();
            compFuelable = GetComp<CompRefuelable>();
            modExt = def.GetModExtension<DefModExt_ChargerGraphics>();
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            EnergyUtil.GetEnergyNeedWorldComp.RemoveChargepackCharger(this, Map);
            base.DeSpawn(mode);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            EnergyUtil.GetEnergyNeedWorldComp.RemoveChargepackCharger(this, Map);
            base.Destroy(mode);
        }
    }
}
