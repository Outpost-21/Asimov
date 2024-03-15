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
    public static class AutomatonUtil
    {
        public static bool IsAutomaton(this Pawn pawn)
        {
            if(pawn as Automaton != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsHumanlikeAutomaton(this Pawn pawn)
        {
            if(pawn.def as PawnDef != null)
            {
                return true;
            }
            return false;
        }

        public static AcceptanceReport CanDraftAutomaton(Pawn pawn)
        {
            if (pawn.Faction != null && pawn.Faction.IsPlayer && pawn.drafter != null)
            {
                if (pawn.Downed)
                {
                    return "Asimov.CannotDraftDownedAutomaton".Translate(pawn.Named("PAWN"));
                }
                return true;
            }
            return false;
        }

        public static Thing GetBestRepairThing(Pawn tender, Pawn target)
        {
            Comp_Automaton comp = target.TryGetComp<Comp_Automaton>();
            if(comp == null)
            {
                LogUtil.LogError("Tried to get repair item for automaton but target is not an automaton.");
                return null;
            }
            if (comp.Props.repairThings.NullOrEmpty())
            {
                return null;
            }
            Predicate<Thing> validator = (Thing m) => (!m.IsForbidden(tender) && (target.playerSettings == null || target.playerSettings.medCare.AllowsMedicine(m.def)) && tender.CanReserve(m, 10, 1)) ? true : false;
            Thing bestRepairThing = GenClosest.ClosestThing_Global_Reachable(target.Position, target.Map, GetAllRepairThings(comp.Props.repairThings, target.Map), PathEndMode.ClosestTouch, TraverseParms.For(tender), 9999f, validator, null);
            return bestRepairThing;
        }

        public static IEnumerable<Thing> GetAllRepairThings(List<ThingDef> acceptableDefs, Map map)
        {
            foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver))
            {
                if (acceptableDefs.Contains(thing.def))
                {
                    yield return thing;
                }
            }
            yield break;
        }

        public static int GetNeededMaterialCount(Pawn pawn)
        {
            int total = 0;
            foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
            {
                if(hediff is Hediff_Injury)
                {
                    total++;
                }
            }
            return total;
        }

        public static void DoRepair(Pawn doctor, Pawn patient, Thing medicine)
        {
	        if (!patient.health.HasHediffsNeedingTend())
	        {
		        return;
	        }
	        if (medicine != null && medicine.Destroyed)
	        {
		        Log.Warning("Tried to use destroyed medicine.");
		        medicine = null;
	        }
            RemoveWorstDamage(patient);
            if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
            {
                patient.mindState.timesGuestTendedToByPlayer++;
            }
            if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && patient.RaceProps.playerCanChangeMaster && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
            {
                InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, useAudiovisualEffects: false);
            }
            patient.records.Increment(RecordDefOf.TimesTendedTo);
            doctor?.records.Increment(RecordDefOf.TimesTendedOther);
            if (doctor == patient && !doctor.Dead)
            {
                doctor.mindState.Notify_SelfTended();
            }
            if (medicine != null)
            {
                if (medicine.stackCount > 1)
                {
                    medicine.stackCount--;
                }
                else if (!medicine.Destroyed)
                {
                    medicine.Destroy();
                }
            }
            if (ModsConfig.IdeologyActive && doctor != null && doctor.Ideo != null)
            {
                Precept_Role role = doctor.Ideo.GetRole(doctor);
                if (role != null && role.def.roleEffects != null)
                {
                    foreach (RoleEffect roleEffect in role.def.roleEffects)
                    {
                        roleEffect.Notify_Tended(doctor, patient);
                    }
                }
            }
            if (doctor != null && doctor.Faction == Faction.OfPlayer && doctor != patient)
            {
                QuestUtility.SendQuestTargetSignals(patient.questTags, "PlayerTended", patient.Named("SUBJECT"));
            }
        }

        public static void RemoveWorstDamage(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.GetHediffsTendable().ToList();
            TendUtility.SortByTendPriority(hediffs);
            for (int i = 0; i < Mathf.Min(3, hediffs.Count); i++)
            {
                hediffs[i].Heal(2f);
            }
        }

        public static bool CanWear(this Pawn pawn, Apparel ap)
        {
            DefModExt_AutomatonApparel modExt = pawn.def.GetModExtension<DefModExt_AutomatonApparel>();
            if (modExt != null && !modExt.apparelWhitelist.NullOrEmpty())
            {
                return modExt.apparelWhitelist.Contains(ap.def);
            }

            return false;
        }
    }
}
