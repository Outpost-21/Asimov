using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
	public class Recipe_RepairKit : RecipeWorker
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield return null;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			List<Hediff> list = new List<Hediff>();
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff is Hediff_MissingPart || hediff is Hediff_Injury)
				{
					list.Add(hediff);
				}
			}
			foreach (Hediff item in list)
			{
				pawn.health.RemoveHediff(item);
			}
		}
	}
}
