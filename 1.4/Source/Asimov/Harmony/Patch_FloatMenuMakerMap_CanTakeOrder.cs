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
    [HarmonyPatch(typeof(FloatMenuMakerMap), "CanTakeOrder")]
    public static class Patch_FloatMenuMakerMap_CanTakeOrder
	{
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, ref bool __result)
		{
			if (pawn.Faction != null && (pawn.Faction?.IsPlayer ?? false) && pawn.IsAutomaton())
			{
				__result = true;
			}
		}
	}
}
