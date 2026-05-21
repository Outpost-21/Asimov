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
    public class JobDriver_RestorePower : JobDriver
	{
		public Thing MaterialUsed => job.targetB.Thing;

		public Corpse CorpseThing => job.targetA.Thing as Corpse;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (CorpseThing == null || !pawn.Reserve(CorpseThing, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			return true;
		}

		public override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate
			{
				if (MaterialUsed != null && pawn.Faction == Faction.OfPlayer && CorpseThing.InnerPawn.playerSettings != null)
				{
					return true;
				}
				return false;
			});
			AddEndCondition(delegate
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					return JobCondition.Ongoing;
				}
				return ((job.playerForced || pawn.Faction != Faction.OfPlayer) && CorpseThing.InnerPawn.DiedFromPowerLoss()) ? JobCondition.Ongoing : JobCondition.Succeeded;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			PathEndMode interactionCell = PathEndMode.ClosestTouch;
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
			yield return gotoToil;
			int ticks = (int)(1f / pawn.GetStatValue(StatDefOf.WorkSpeedGlobal) * 100f);
			yield return Toils_Repair.FinalizeRestorePower(CorpseThing);
			yield return Toils_Jump.Jump(gotoToil);
		}
	}
}
