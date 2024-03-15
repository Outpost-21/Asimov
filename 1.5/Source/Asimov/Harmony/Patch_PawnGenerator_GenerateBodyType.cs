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
    [HarmonyPatch(typeof(PawnGenerator), "GenerateBodyType")]
    public static class Patch_PawnGenerator_GenerateBodyType
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn)
        {
            pawn.story.bodyType = GenUtil.GetBodyType(pawn);
        }
    }
}
