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

namespace Asimov
{
    [HarmonyPatch(typeof(Pawn_IdeoTracker), "CertaintyChangeFactor", MethodType.Getter)]
    public static class Patch_Pawn_IdeoTracker_CertaintyChangeFactor
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn_IdeoTracker __instance, ref float __result)
		{
			if (!ModsConfig.BiotechActive)
			{
				return true;
			}
			if(__instance.pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeBaby && __instance.pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeChild && __instance.pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeAdult)
            {
				__result = 1f;
				return false;
            }
			return true;
		}
    }
}
