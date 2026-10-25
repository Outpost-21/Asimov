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
    public class WorkGiver_ChargeOther : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode => PathEndMode.InteractionCell;

		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Pawn tPawn) || tPawn == pawn)
			{
				return false;
			}
			if (!EnergyUtil.NeedsChargepack(tPawn))
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(tPawn))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (!TryFindBestChargepackFor(pawn, tPawn, out var _))
			{
				JobFailReason.Is("Asimov.NoChargepacks".Translate());
				return false;
			}
			return true;
		}

		public bool TryFindBestChargepackFor(Pawn pawn, Pawn patient, out Thing chargepack)
        {
			chargepack = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn), 9999f, thing => thing.TryGetComp<Comp_EnergySource>() != null && !thing.IsForbidden(pawn) && pawn.CanReserve(new LocalTargetInfo(thing)) && thing.Position.InAllowedArea(pawn) && pawn.CanReach(new LocalTargetInfo(thing), PathEndMode.OnCell, Danger.Deadly));
			return chargepack != null;
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
				return JobMaker.MakeJob(AsimovDefOf.Asimov_ChargeOther, pawn2, thing);
			}
			return null;
		}
	}
}
