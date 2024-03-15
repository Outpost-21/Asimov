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
    [HarmonyPatch(typeof(ITab_Genes), "Hidden", MethodType.Getter)]
    public static class Patch_ITab_Genes_Hidden
	{
        [HarmonyPostfix]
        public static void Postfix(ITab_Genes __instance, ref bool __result)
		{
            if (!__result)
			{
				if (__instance.SelPawn != null && __instance.SelPawn.def is PawnDef def)
				{
					__result = true;
				}
			}
		}
	}
}
