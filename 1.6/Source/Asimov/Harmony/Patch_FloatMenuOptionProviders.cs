using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld.Planet;
using Verse.AI;

namespace Asimov
{
    [HarmonyPatch(typeof(FloatMenuOptionProvider_DraftedTend), "AppliesInt")]
    public static class Patch_FloatMenuOptionProvider_DraftedTend_AppliesInt
    {
        [HarmonyPrefix]
        public static bool Prefix(FloatMenuContext context)
		{
			if (context.FirstSelectedPawn.IsAutomaton())
			{
				if (context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Caring))
				{
					return false;
				}
			}
			return true;
		}

		//[HarmonyPostfix]
		//public static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		//{
		//  if (pawn.IsAutomaton())
		//  {
		//		try
		//		{
		//			if (pawn.Faction == Faction.OfPlayer)
		//			{
		//				IntVec3 c = IntVec3.FromVector3(clickPos);
		//				c.GetThingList(pawn.Map);
		//				Apparel ap = pawn.Map.thingGrid.ThingAt<Apparel>(c);
		//				if (ap != null)
		//				{
		//					if (pawn.CanWear(ap))
		//					{
		//						string cantReason;
		//						FloatMenuOption op = EquipmentUtility.CanEquip(ap, pawn, out cantReason) ? FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ForceWear".Translate(ap.LabelShort, ap), delegate
		//						{
		//							ap.SetForbidden(value: false);
		//							Job job = JobMaker.MakeJob(JobDefOf.Wear, ap);
		//							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						}, MenuOptionPriority.High), pawn, ap) : new FloatMenuOption("CannotWear".Translate(ap.Label, ap) + ": " + cantReason, null);
		//						opts.Add(op);
		//					}
		//				}
		//			}
		//		}
		//		catch (Exception e)
		//		{
		//			LogUtil.LogError("Error while checking for wearable apparel for selected pawn: " + e);
		//		}
		//	}
		//}
	}
}
