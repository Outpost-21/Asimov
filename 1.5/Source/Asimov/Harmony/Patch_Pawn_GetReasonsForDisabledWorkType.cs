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
    [HarmonyPatch(typeof(Pawn), "GetReasonsForDisabledWorkType")]
    public static class Patch_Pawn_GetReasonsForDisabledWorkType
    {
        [HarmonyPostfix]
        public static void Postfix(WorkTypeDef workType, Pawn __instance, ref List<string> __result)
		{
			Comp_Automaton comp = __instance.TryGetComp<Comp_Automaton>();
			if (comp != null && !comp.DisabledWorkTypes.NullOrEmpty())
			{
				foreach (WorkTypeDef workTypeDef in comp.DisabledWorkTypes)
				{
					if (workTypeDef == workType)
					{
						__result.Add("Asimov.WorkDisabledByProgramming".Translate(__instance.def.LabelCap, comp.Props.workDisableTerm));
					}
				}
			}
		}
	}
}
