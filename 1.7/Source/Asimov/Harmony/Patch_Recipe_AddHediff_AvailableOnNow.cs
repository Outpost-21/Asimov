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
    [HarmonyPatch(typeof(Recipe_AddHediff), "AvailableOnNow")]
    public static class Patch_Recipe_AddHediff_AvailableOnNow
	{
		[HarmonyPrefix]
		public static bool Prefix(Recipe_Surgery __instance, ref bool __result, Thing thing, BodyPartRecord part = null)
		{
			if (__instance.recipe.defName.Contains("Sterilize"))
			{
				Pawn pawn = thing as Pawn;
				if (pawn.Sterile())
				{
					__result = false;
					return false;
				}
			}
			return true;
		}
	}
}
