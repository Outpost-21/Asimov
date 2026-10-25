using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Asimov
{
    public class WorkGiver_FillAutoCrafter : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(WorkGiverProperties.defToScan);

        public override PathEndMode PathEndMode => PathEndMode.Touch;

        private WorkGiverProperties_FillAutoCrafter intWorkGiverProperties = null;

        public WorkGiverProperties_FillAutoCrafter WorkGiverProperties
        {
            get
            {
                if (intWorkGiverProperties == null)
                {
                    intWorkGiverProperties = def.GetModExtension<WorkGiverProperties_FillAutoCrafter>();
                }

                return intWorkGiverProperties;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_AutoCrafter autoProducer = t as Building_AutoCrafter;

            if (autoProducer == null || autoProducer.TryGetComp<Comp_AutoCrafter>().currentStatus != ProducerStatus.awaitingResources)
                return false;

            if (t.IsForbidden(pawn) || !pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced))
            {
                return false;
            }

            if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
            {
                return false;
            }

            IEnumerable<ThingOrderRequest> potentionalRequests = autoProducer.TryGetComp<Comp_AutoCrafter>().orderProcessor.PendingRequests();
            bool validRequest = false;
            if (potentionalRequests != null)
            {
                foreach (ThingOrderRequest request in potentionalRequests)
                {
                    Thing ingredientThing = FindIngredient(pawn, autoProducer, request);
                    if (ingredientThing != null)
                    {
                        validRequest = true;
                        break;
                    }
                }
            }

            return validRequest;
        }

        public override Job JobOnThing(Pawn pawn, Thing crafterThing, bool forced = false)
        {
            Building_AutoCrafter automatedProducer = crafterThing as Building_AutoCrafter;

            IEnumerable<ThingOrderRequest> potentionalRequests = automatedProducer.TryGetComp<Comp_AutoCrafter>().orderProcessor.PendingRequests();

            if (potentionalRequests != null)
            {
                foreach (ThingOrderRequest request in potentionalRequests)
                {
                    Thing ingredientThing = FindIngredient(pawn, automatedProducer, request);

                    if (ingredientThing != null)
                    {
                        return new Job(WorkGiverProperties.fillJob, ingredientThing, crafterThing)
                        {
                            count = (int)request.amount
                        };
                    }
                }
            }

            return null;
        }

        private Thing FindIngredient(Pawn pawn, Building_AutoCrafter automatedProducer, ThingOrderRequest request)
        {
            if (request != null)
            {
                Predicate<Thing> extraPredicate = request.ExtraPredicate();
                Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false) && extraPredicate(x);
                Predicate<Thing> validator = predicate;

                return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, request.Request(), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
            }

            return null;
        }
    }
}
