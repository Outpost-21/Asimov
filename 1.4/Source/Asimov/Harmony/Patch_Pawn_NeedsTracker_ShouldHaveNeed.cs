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
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Patch_Pawn_NeedsTracker_ShouldHaveNeed
	{
		[HarmonyPrefix]
		public static bool Prefix(NeedDef nd, Pawn_NeedsTracker __instance, bool __result)
		{
			Comp_Automaton comp = __instance.pawn.TryGetComp<Comp_Automaton>();
			if(comp != null)
            {
                if (comp.Props.disabledNeeds.Contains(nd))
                {
					__result = false;
					return false;
                }
            }
			return true;
		}
	}
}
