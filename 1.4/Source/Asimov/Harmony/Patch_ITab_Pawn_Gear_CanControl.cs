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
    [HarmonyPatch(typeof(ITab_Pawn_Gear), "CanControl", MethodType.Getter)]
    public static class Patch_ITab_Pawn_Gear_CanControl
	{
        [HarmonyPostfix]
        public static void Postfix(ITab_Pawn_Gear __instance, ref bool __result)
		{
			if (Patch_ITab_Pawn_Gear_DrawThingRow.drawingThingRow)
			{
				Pawn pawn = pawnToShowInfoAbout(__instance);
				Comp_Automaton comp = pawn.GetComp<Comp_Automaton>();
				if (comp != null && !comp.Props.canUseRanged)
				{
					__result = false;
				}
			}
		}

		public delegate Pawn PawnToShowInfoAbout(ITab_Pawn_Gear __instance);

		public static readonly PawnToShowInfoAbout pawnToShowInfoAbout = AccessTools.MethodDelegate<PawnToShowInfoAbout>(AccessTools.Method(typeof(ITab_Pawn_Gear), "get_SelPawnForGear"));
	}
}
