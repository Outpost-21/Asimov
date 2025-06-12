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
    [HarmonyPatch(typeof(ThinkNode_ConditionalShouldFollowMaster), "ShouldFollowMaster")]
    public static class Patch_ThinkNode_ConditionalShouldFollowMaster_ShouldFollowMaster
	{
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, ref bool __result)
		{
			if (pawn.IsAutomaton() && (pawn.Faction?.IsPlayer ?? false))
			{
				__result = false;
			}
		}
	}
}
