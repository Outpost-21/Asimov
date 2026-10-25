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
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] {typeof(PawnGenerationRequest)})]
    public static class Patch_PawnGenerator_GeneratePawn
    {
        [HarmonyPrefix]
        public static void Prefix(PawnGenerationRequest request)
        {
            if(request.KindDef.race is PawnDef)
            {
                request.ForcedXenotype = XenotypeDefOf.Baseliner;
            }
        }
    }
}
