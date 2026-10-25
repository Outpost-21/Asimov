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
    [HarmonyPatch(typeof(ITab_Pawn_Gear), "DrawThingRow")]
    public static class Patch_ITab_Pawn_Gear_DrawThingRow
	{
		public static bool drawingThingRow;

		[HarmonyPrefix]
		public static void Prefix()
		{
			drawingThingRow = true;
		}

		[HarmonyPostfix]
		public static void Postfix()
		{
			drawingThingRow = false;
		}
	}
}
