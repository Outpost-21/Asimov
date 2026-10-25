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
    public class WorkGiver_InsertChargepacks : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode => PathEndMode.Touch;

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return EnergyUtil.GetChargepackChargersOnMap(pawn.Map);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_ChargepackCharger building = t as Building_ChargepackCharger;
			if (building == null || building.processState != ProcessState.AwaitingInput || building.IsFull)
			{
				return false;
			}
			if (!t.IsForbidden(pawn))
			{
				LocalTargetInfo target = t;
				if (pawn.CanReserve(target, 1, 1, null, forced))
				{
					if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
					{
						return false;
					}
					if (FindEmptyChargepacks(pawn) == null)
					{
						JobFailReason.Is("Asimov.NoEmptyChargepacksFound");
						return false;
					}
					return !t.IsBurning();
				}
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_ChargepackCharger building = (Building_ChargepackCharger)t;
			Thing thing = FindEmptyChargepacks(pawn);
			return new Job(AsimovDefOf.Asimov_InsertChargepacks, t, thing);
		}

		private Thing FindEmptyChargepacks(Pawn pawn)
		{
			Predicate<Thing> predicate2 = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1);
			IntVec3 position2 = pawn.Position;
			Map map2 = pawn.Map;
			ThingRequest thingReq = ThingRequest.ForDef(AsimovDefOf.Asimov_Chargepack_Empty);
			PathEndMode peMode2 = PathEndMode.ClosestTouch;
			TraverseParms traverseParams2 = TraverseParms.For(pawn);
			Predicate<Thing> validator2 = predicate2;
			return GenClosest.ClosestThingReachable(position2, map2, thingReq, peMode2, traverseParams2, 9999f, validator2);
		}
	}
}
