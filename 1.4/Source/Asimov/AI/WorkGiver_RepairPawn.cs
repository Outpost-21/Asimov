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
    public class WorkGiver_RepairPawn : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode => PathEndMode.InteractionCell;

		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Pawn tPawn) || pawn.WorkTypeIsDisabled(WorkTypeDefOf.Crafting) || (def.tendToHumanlikesOnly && !tPawn.RaceProps.Humanlike) || (def.tendToAnimalsOnly && !tPawn.RaceProps.Animal) || !IsReadyForTending(tPawn, pawn) || !HealthAIUtility.ShouldBeTendedNowByPlayer(tPawn) || tPawn.IsForbidden(pawn) || !pawn.CanReserve(tPawn, 1, -1, null, forced) || (tPawn.InAggroMentalState && !tPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria)))
			{
				return false;
			}
			return true;
		}

		public static bool IsReadyForTending(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return true;
			}
			if (!patient.Downed)
			{
				return patient.CurJobDef == AsimovDefOf.Asimov_Hibernate || patient.CurJobDef == AsimovDefOf.Asimov_HibernateTillRepaired;
			}
			return patient.GetPosture() != PawnPosture.Standing;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2);
			if (thing != null)
			{
				return JobMaker.MakeJob(AsimovDefOf.Asimov_RepairAutomaton, pawn2, thing);
			}
			return JobMaker.MakeJob(AsimovDefOf.Asimov_RepairAutomaton, pawn2);
		}
	}
}
