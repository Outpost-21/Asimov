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
    [HarmonyPatch(typeof(PawnGenerator), "GenerateTraits", null)]
    public static class Patch_PawnGenerator_GenerateTraits
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn pawn, PawnGenerationRequest request)
        {
            PawnDef def = pawn.def as PawnDef;
            if (def != null && !def.pawnSettings.hasTraits)
            {
                return false;
            }
            return true;
        }
    }
}
