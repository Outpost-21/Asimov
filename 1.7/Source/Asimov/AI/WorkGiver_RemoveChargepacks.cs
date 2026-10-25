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
    public class WorkGiver_RemoveChargepacks : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode => PathEndMode.Touch;

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return EnergyUtil.GetChargepackChargersOnMap(pawn.Map);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_ChargepackCharger building = t as Building_ChargepackCharger;
			if (building == null || building.processState != ProcessState.Finished || building.IsEmpty)
			{
				return false;
			}
			if (!t.IsForbidden(pawn))
			{
				LocalTargetInfo target = t;
				if (pawn.CanReserve(target, 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(AsimovDefOf.Asimov_RemoveChargepacks, t);
		}
	}
}
