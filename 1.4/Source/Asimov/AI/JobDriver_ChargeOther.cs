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
    public class JobDriver_ChargeOther : JobDriver
	{
		public Thing Chargepack => job.targetA.Thing;

		public Pawn Deliveree => (Pawn)job.targetB.Thing;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			return true;
		}

		public override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			if (pawn.inventory != null && pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(pawn, TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				Toil toil = new Toil();
				toil.initAction = delegate
				{
					Pawn actor = toil.actor;
					Job curJob = actor.jobs.curJob;
					Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
					if (curJob.count <= 0)
					{
						Log.Error("Tried to do PickupChargepack toil with job.count = " + curJob.count);
						actor.jobs.EndCurrentJob(JobCondition.Errored);
					}
					else
					{
						int count = Mathf.Min(thing.stackCount, curJob.count);
						actor.carryTracker.TryStartCarry(thing, count);
						if (thing != actor.carryTracker.CarriedThing && actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
						{
							actor.Map.reservationManager.Release(thing, actor, curJob);
						}
						actor.jobs.curJob.targetA = actor.carryTracker.CarriedThing;
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				yield return toil;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_General.Wait(100).WithProgressBarToilDelay(TargetIndex.A, false);

			Toil rechargeToil = new Toil();
			rechargeToil.AddFinishAction(delegate ()
			{
				Thing carriedThing = pawn.carryTracker.CarriedThing;
				if (carriedThing != null)
				{
					Comp_EnergySource energyComp = carriedThing.TryGetComp<Comp_EnergySource>();
					if (energyComp != null)
					{
						energyComp.RechargeEnergyNeed(pawn);
					}

					pawn.carryTracker.DestroyCarriedThing();
				}
			});

			yield return rechargeToil;
		}
	}
}
