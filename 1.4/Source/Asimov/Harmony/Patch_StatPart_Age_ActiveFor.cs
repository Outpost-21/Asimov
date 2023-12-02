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
    [HarmonyPatch(typeof(StatPart_Age), "ActiveFor")]
    public static class Patch_StatPart_Age_ActiveFor
	{
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, ref bool __result)
		{
            if (__result)
			{
				if(pawn.def is PawnDef pawnDef)
                {
                    __result = !pawnDef.pawnSettings.immuneToAge;
                }
			}
		}
	}
}
