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
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    public static class Patch_FloatMenuMakerMap_AddDraftedOrders
    {
        [HarmonyPrefix]
        public static bool Prefix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			if (pawn.IsAutomaton())
			{
				if (pawn.Downed)
				{
					return false;
				}
			}
			return true;
		}
	}
}
