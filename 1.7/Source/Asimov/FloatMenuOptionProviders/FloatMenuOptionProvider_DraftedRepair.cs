
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Asimov
{
    public class FloatMenuOptionProvider_DraftedRepair : FloatMenuOptionProvider
    {
        public override bool Drafted => true;
        public override bool Undrafted => false;
        public override bool Multiselect => false;
        public override bool MechanoidCanDo => true;
        public override bool RequiresManipulation => true;
        public override bool CanSelfTarget => false;

        public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
        {
            Log.Message("A1");
            if (!IsValidRepairTarget(context.FirstSelectedPawn, clickedPawn))
            {
                yield break;
            }
            Log.Message("A2");
            if (!clickedPawn.health.HasHediffsNeedingTend())
            {
                if (context.FirstSelectedPawn != clickedPawn)
                {
                    yield return new FloatMenuOption("Asimov.CannotRepair".Translate(clickedPawn) + ": " + "Asimov.RepairNotRequired".Translate(clickedPawn), null);
                }
                yield break;
            }
            Log.Message("A3");
            if (context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
            {
                yield return new FloatMenuOption("Asimov.CannotRepair".Translate(clickedPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null);
                yield break;
            }
            if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly))
            {
                yield return new FloatMenuOption("Asimov.CannotRepair".Translate(clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
                yield break;
            }
            if (clickedPawn == context.FirstSelectedPawn && context.FirstSelectedPawn.playerSettings != null && !context.FirstSelectedPawn.playerSettings.selfTend)
            {
                yield return new FloatMenuOption("Asimov.CannotRepair".Translate(clickedPawn) + ": " + "SelfTendDisabled".Translate().CapitalizeFirst(), null);
                yield break;
            }
            if (clickedPawn.InAggroMentalState && !clickedPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria))
            {
                yield return new FloatMenuOption("Asimov.CannotRepair".Translate(clickedPawn) + ": " + "PawnIsInAggroMentalState".Translate(clickedPawn).CapitalizeFirst(), null);
                yield break;
            }
            yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Asimov.Repair".Translate(clickedPawn), Action), context.FirstSelectedPawn, clickedPawn);
            void Action()
            {
                Job job = JobMaker.MakeJob(AsimovDefOf.Asimov_RepairAutomaton, clickedPawn);
                context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            }
        }

        public bool IsValidRepairTarget(Pawn doctor, Pawn patient)
        {
            if (patient == doctor)
            {
                return false;
            }
            if (!patient.IsAutomaton() && !patient.IsHumanlikeAutomaton())
            {
                return false;
            }
            Comp_Automaton automatonComp = patient.GetAutomatonComp();
            Log.Message(automatonComp?.Props?.canBeRepaired.ToString() ?? "Null!");
            if (!automatonComp?.Props?.canBeRepaired ?? true)
            {
                return false;
            }
            if (patient.HostileTo(doctor.Faction))
            {
                return false;
            }
            if (patient.Downed || patient.IsColonist || patient.IsQuestLodger() || patient.IsPrisonerOfColony || patient.IsSlaveOfColony || patient.Faction == Faction.OfPlayer)
            {
                return true;
            }
            return false;
        }
    }
}
