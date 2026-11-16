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
    [HarmonyPatch(typeof(CompAssignableToPawn_Bed), "AssigningCandidates", MethodType.Getter)]
    public static class Patch_CompAssignableToPawn_Bed_AssigningCandidates
    {
        [HarmonyPostfix]
        public static void Postfix(CompAssignableToPawn_Bed __instance, ref IEnumerable<Pawn> __result)
		{
            if (!__instance.parent.def.building.bed_humanlike)
            {
                IEnumerable<Pawn> additions = from p in __instance.parent.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
                                              where p.IsAutomaton() && p.RaceProps.needsRest
                                              select p;
                __result = __result.Concat(additions);
            }
		}
	}
}
