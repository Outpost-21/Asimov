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
    public class JobDriver_HibernateTillRepaired : JobDriver
    {
        public Thing Target => TargetA.Thing;

        public CompPowerTrader powerTrader;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.CanReserveAndReach(Target, PathEndMode.OnCell, Danger.Deadly))
            {
                pawn.Reserve(Target, job, errorOnFailed: errorOnFailed);
                return true;
            }

            return false;
        }

        public override RandomSocialMode DesiredSocialMode()
        {
            return RandomSocialMode.Off;
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            powerTrader = Target.TryGetComp<CompPowerTrader>();

            this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);

            Toil hibernateToil = new Toil();
            hibernateToil.handlingFacing = true;
            hibernateToil.initAction = delegate ()
            {
                pawn.Rotation = Rot4.South;
                pawn.pather.StopDead();
            };
            hibernateToil.tickAction = delegate ()
            {
                // Interrupt once fully repaired
                if (!pawn.health.HasHediffsNeedingTend())
                {
                    EndJobWith(JobCondition.Succeeded);
                }
                // Interrupt if pawn takes damage.
                if (pawn.mindState.lastHarmTick - Find.TickManager.TicksGame >= -20)
                {
                    EndJobWith(JobCondition.InterruptOptional);
                }
                // Interrupt if a fire is found nearby.
                if (Find.TickManager.TicksGame % 200 == 0)
                {
                    foreach (IntVec3 vec in pawn.CellsAdjacent8WayAndInside())
                    {
                        if (vec.InBounds(pawn.Map) && vec.GetFirstThing(pawn.Map, RimWorld.ThingDefOf.Fire) is Thing fire)
                        {
                            EndJobWith(JobCondition.InterruptOptional);
                            break;
                        }
                    }
                }
                // Interrupt if spot loses power (if powered).
                if (powerTrader != null && !powerTrader.PowerOn)
                {
                    EndJobWith(JobCondition.InterruptOptional);
                }
            };
            hibernateToil.defaultCompleteMode = ToilCompleteMode.Never;
            yield return hibernateToil;
        }
    }
}
