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
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike")]
    public static class Patch_PawnDiedOrDownedThoughtsUtility_AppendThoughts_ForHumanlike
	{
        [HarmonyPrefix]
        public static bool Prefix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			bool flag = dinfo.HasValue && dinfo.Value.Def.execution;
			if (victim?.Faction?.IsPlayer ?? false && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !flag)
			{
				PawnDef pawnDef = victim.def as PawnDef;
				if (pawnDef != null && pawnDef.pawnSettings != null && !pawnDef.pawnSettings.colonyCaresIfDead)
                {
					return false;
                }
			}
			return true;
		}
	}
}
