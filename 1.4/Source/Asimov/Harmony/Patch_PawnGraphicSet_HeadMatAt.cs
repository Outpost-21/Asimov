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
    [HarmonyPatch(typeof(PawnGraphicSet), "HeadMatAt")]
    public static class Patch_PawnGraphicSet_HeadMatAt
    {
        [HarmonyPrefix]
        public static bool Prefix(PawnGraphicSet __instance, ref Material __result, Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false, bool portrait = false, bool allowOverride = true)
        {
            if(__instance.pawn != null && stump && __instance.pawn.IsHumanlikeAutomaton())
            {
                return false;
            }
            return true;
        }
    }
}
