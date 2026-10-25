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
	[HarmonyPatch(typeof(DebugToolsSpawning), "GetCategoryForPawnKind")]
	public class Patch_DebugToolsSpawning_GetCategoryForPawnKind
	{
		[HarmonyPostfix]
		public static void Postfix(ref string __result, PawnKindDef kindDef)
		{
			if(kindDef.race.HasComp(typeof(Comp_Automaton)))
            {
				__result = "Automaton";
            }
		}
	}
}
