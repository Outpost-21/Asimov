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
    public class WorkGiver_RepairSelf : WorkGiver_RepairPawn
	{
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Undefined);

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			int num;
			if (pawn == t && pawn.playerSettings != null)
			{
				num = (base.HasJobOnThing(pawn, t, forced) ? 1 : 0);
				if (num != 0 && !pawn.playerSettings.selfTend)
				{
					JobFailReason.Is("SelfTendDisabled".Translate());
				}
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				return pawn.playerSettings.selfTend;
			}
			return false;
		}
	}
}
