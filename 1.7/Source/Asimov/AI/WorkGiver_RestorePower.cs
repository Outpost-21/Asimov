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
    public class WorkGiver_RestorePower : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode => PathEndMode.InteractionCell;

		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Corpse);

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Thing> unburiedAutomatonCorpsesResult = new List<Thing>();
			List<Thing> list = pawn.Map.listerThings.ThingsMatching(PotentialWorkThingRequest);
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if (corpse.InnerPawn.DiedFromPowerLoss())
                {
					Log.Message(corpse.InnerPawn.Name);

                    unburiedAutomatonCorpsesResult.Add(corpse);
				}
			}
			return unburiedAutomatonCorpsesResult;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse corpse) || pawn.WorkTypeIsDisabled(WorkTypeDefOf.Crafting) || (!corpse.InnerPawn.IsAutomaton() && !corpse.InnerPawn.IsHumanlikeAutomaton()) || corpse.IsForbidden(pawn) || !pawn.CanReserve(corpse, 1, -1, null, forced) || corpse.InnerPawn.InAggroMentalState)
			{
				return false;
			}
			return true;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = AutomatonUtil.GetBestChargepack(pawn, pawn2);
			if (thing != null)
            {
                //return JobMaker.MakeJob(AsimovDefOf.Asimov_RestoreAutomatonPower, pawn2, thing);
            }
			return null;
		}
	}
}
