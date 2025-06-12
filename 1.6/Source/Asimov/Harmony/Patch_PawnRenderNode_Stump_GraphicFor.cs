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
    [HarmonyPatch(typeof(PawnRenderNode_Stump), "GraphicFor")]
    public static class Patch_PawnRenderNode_Stump_GraphicFor
    {
        [HarmonyPrefix]
        public static bool Prefix(PawnRenderNode_Head __instance, ref Graphic __result, Pawn pawn)
        {
            if(pawn != null && !pawn.health.hediffSet.HasHead && pawn.IsHumanlikeAutomaton())
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
