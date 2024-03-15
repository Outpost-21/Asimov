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

namespace Asimov
{
    [HarmonyPatch(typeof(Pawn_StoryTracker), "TryGetRandomHeadFromSet")]
    public static class Patch_Pawn_StoryTracker_TryGetRandomHeadFromSet
    {
        [HarmonyPostfix]
        public static void Postfix(IEnumerable<HeadTypeDef> options, Pawn_StoryTracker __instance, ref bool __result)
		{
			Pawn pawn = __instance.pawn;
			if(pawn.def is PawnDef pawnDef)
			{
				if(pawnDef.pawnSettings == null || pawnDef.pawnSettings.headTypeWhitelist.NullOrEmpty())
                {
					return;
				}
				options = pawnDef.pawnSettings.headTypeWhitelist.AsEnumerable();
				Rand.PushState(__instance.pawn.thingIDNumber);
				bool result = options.Where((HeadTypeDef h) => CanUseHeadType(pawn, h)).TryRandomElementByWeight((HeadTypeDef x) => x.selectionWeight, out __instance.headType);
				Rand.PopState();
				__result = result;
				return;
			}
		}

		public static bool CanUseHeadType(Pawn pawn, HeadTypeDef head)
		{
			if (ModsConfig.BiotechActive && !head.requiredGenes.NullOrEmpty())
			{
				if (pawn.genes == null)
				{
					return false;
				}
				foreach (GeneDef requiredGene in head.requiredGenes)
				{
					if (!pawn.genes.HasGene(requiredGene))
					{
						return false;
					}
				}
			}
			if (head.gender != 0)
			{
				return head.gender == pawn.gender;
			}
			return true;
		}
	}
}
