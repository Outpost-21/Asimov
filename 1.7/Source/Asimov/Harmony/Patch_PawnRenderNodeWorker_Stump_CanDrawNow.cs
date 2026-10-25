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
    [HarmonyPatch(typeof(PawnRenderNodeWorker_Stump), "CanDrawNow")]
    public static class Patch_PawnRenderNodeWorker_Stump_CanDrawNow
    {
        [HarmonyPrefix]
        public static bool Prefix(bool __result, PawnRenderNode node, PawnDrawParms parms)
        {
            if(parms.pawn != null && !parms.pawn.health.hediffSet.HasHead && parms.pawn.IsHumanlikeAutomaton())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
