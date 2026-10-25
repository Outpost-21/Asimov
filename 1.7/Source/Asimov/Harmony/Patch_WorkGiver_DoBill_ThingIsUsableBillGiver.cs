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
	[HarmonyPatch(typeof(WorkGiver_DoBill), "ThingIsUsableBillGiver")]
	public class Patch_WorkGiver_DoBill_ThingIsUsableBillGiver
	{
		[HarmonyPostfix]
		public static void Postfix(WorkGiver_DoBill __instance, ref bool __result, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			WorkGiverDef def = __instance.def;
			if(pawn != null && (!def.billGiversAllHumanlikes && !def.billGiversAllMechanoids && !def.billGiversAllAnimals) && pawn.IsAutomaton())
            {
				__result = true;
            }
		}
	}
}
