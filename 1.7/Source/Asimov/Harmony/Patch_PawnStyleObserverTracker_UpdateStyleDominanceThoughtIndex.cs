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
    [HarmonyPatch(typeof(Pawn_StyleObserverTracker), "UpdateStyleDominanceThoughtIndex")]
    public static class Patch_PawnStyleObserverTracker_UpdateStyleDominanceThoughtIndex
	{
        [HarmonyPrefix]
        public static bool Prefix(Pawn_StyleObserverTracker __instance, float styleDominance, float pointsThreshold, int lastIndex)
		{
			if (__instance.pawn.needs.mood == null)
			{
				return false;
			}
			return true;
		}
	}
}
