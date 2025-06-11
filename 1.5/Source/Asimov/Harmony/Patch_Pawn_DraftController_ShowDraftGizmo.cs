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
    [HarmonyPatch(typeof(Pawn_DraftController), "ShowDraftGizmo", MethodType.Getter)]
    public static class Patch_Pawn_DraftController_ShowDraftGizmo
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn ___pawn, ref bool __result)
        {
            if (___pawn.IsAutomaton() && ___pawn.Faction != null && (___pawn.Faction?.IsPlayer ?? false) && ___pawn.drafter != null)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
