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
    [HarmonyPatch(typeof(PawnComponentsUtility), "AddAndRemoveDynamicComponents")]
    public static class Patch_PawnComponentsUtility_AddAndRemoveDynamicComponents
    {
        [HarmonyPostfix]
        public static void Prefix(Pawn pawn)
		{
			if (pawn.IsAutomaton() && pawn.Faction != null && (pawn.Faction?.IsPlayer ?? false))
			{
				if (pawn.drafter == null)
				{
					pawn.drafter = new Pawn_DraftController(pawn);
				}
				if (pawn.equipment == null)
				{
					pawn.equipment = new Pawn_EquipmentTracker(pawn);
				}
			}
		}
    }
}
