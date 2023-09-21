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
    [HarmonyPatch(typeof(GatheringsUtility), "ShouldGuestKeepAttendingGathering")]
    public static class Patch_GatheringsUtility_ShouldGuestKeepAttendingGathering
	{
        [HarmonyPrefix]
        public static bool Prefix(Pawn p, ref bool __result)
		{
			if (p.IsAutomaton() || p.def is PawnDef)
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
}
