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
    [HarmonyPatch(typeof(Pawn_AgeTracker), "TrySimulateGrowthPoints")]
    public static class Patch_Pawn_AgeTracker_TrySimulateGrowthPoints
	{
        [HarmonyPrefix]
        public static bool Prefix(Pawn_AgeTracker __instance)
		{
            if (__instance.pawn.IsHumanlikeAutomaton())
            {
                PawnDef def = __instance.pawn.def as PawnDef;
                if (!def.pawnSettings.hasGrowthMoments)
                {
                    return false;
                }
            }
            return true;
		}
	}
}
