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
    public class JobDriver_InsertChargepacks : JobDriver
	{
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.targetA, job) && pawn.Reserve(job.targetB, job);
		}

		public override void Notify_PatherFailed()
		{
			Building_ChargepackCharger building = (Building_ChargepackCharger)job.GetTarget(TargetIndex.A).Thing;
			EndJobWith(JobCondition.ErroredPather);
		}

		public override IEnumerable<Toil> MakeNewToils()
		{
			Building_ChargepackCharger building = (Building_ChargepackCharger)job.GetTarget(TargetIndex.A).Thing;
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.DoAtomic(delegate
			{
				job.count = Building_ChargepackCharger.processCount - building.GetDirectlyHeldThings().TotalStackCount;
			});
			Toil reserveIngredient = Toils_Reserve.Reserve(TargetIndex.B);
			yield return reserveIngredient;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveIngredient, TargetIndex.B, TargetIndex.None, takeFromValidStorage: true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A)
				.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
				.WithProgressBarToilDelay(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate
				{
					if (building.processState == ProcessState.AwaitingInput)
					{
						building.TryAcceptThing(job.targetB.Thing);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
