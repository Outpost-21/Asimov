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
    [HarmonyPatch(typeof(Pawn_GeneTracker), "AddGene", new Type[2] {typeof(Gene), typeof(bool)})]
    public static class Patch_Pawn_GeneTracker_AddGene
    {
        [HarmonyPrefix]
        public static bool Prefix(Gene gene, Pawn ___pawn, ref Gene __result)
        {
            if (!RestrictionUtil.CanHaveGene(___pawn.def, gene.def))
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
