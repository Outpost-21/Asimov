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
    [HarmonyPatch(typeof(PawnRelationWorker), "BaseGenerationChanceFactor")]
    public static class Patch_PawnRelationWorker_BaseGenerationChanceFactor
	{
		[HarmonyPrefix]
		public static bool Prefix(Pawn generated, Pawn other, PawnGenerationRequest request, float __result)
		{
			PawnDef def = generated.def as PawnDef;
			if(def != null && !def.pawnSettings.generateRelations)
            {
				__result = 0f;
				return false;
            }
			return true;
		}
	}
}
