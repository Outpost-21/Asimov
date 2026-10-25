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
    [HarmonyPatch(typeof(ITab_Genes), "CanShowGenesTab")]
    public static class Patch_ITab_Genes_CanShowGenesTab
	{
        [HarmonyPostfix]
        public static void Postfix(ITab_Genes __instance, ref bool __result)
		{
            if (__result)
			{
				Pawn p = Find.Selector.SingleSelectedThing as Pawn;
				if(p != null && p.genes != null)
				{
					ThingDef pawn = ITab_Genes.PawnForGenes(p).def;
					if (pawn is PawnDef def)
					{
						__result = false;
					}
				}
			}
		}
	}
}
