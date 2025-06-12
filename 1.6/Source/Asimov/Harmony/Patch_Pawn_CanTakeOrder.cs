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
    [HarmonyPatch(typeof(Pawn), "CanTakeOrder", MethodType.Getter)]
    public static class Patch_Pawn_CanTakeOrder
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref bool __result)
		{
			if (__instance.IsPlayerAutomaton())
			{
				__result = true;
			}
		}
	}
}
