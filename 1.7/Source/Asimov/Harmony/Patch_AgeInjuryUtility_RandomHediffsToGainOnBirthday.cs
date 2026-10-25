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
    [HarmonyPatch(typeof(AgeInjuryUtility), "RandomHediffsToGainOnBirthday", new Type[] {typeof(ThingDef), typeof(float), typeof(float)})]
    public static class Patch_AgeInjuryUtility_RandomHediffsToGainOnBirthday
	{
		[HarmonyPostfix]
		public static void Postfix(ref IEnumerable<HediffGiver_Birthday> __result, ThingDef raceDef)
		{
			PawnDef def = raceDef as PawnDef;
			if(def != null && def.pawnSettings.immuneToAge)
            {
				__result = new List<HediffGiver_Birthday>();
			}
			CompProperties_Automaton comp = raceDef.GetCompProperties<CompProperties_Automaton>();
			if (comp != null && comp.immuneToDisease)
			{
				__result = new List<HediffGiver_Birthday>();
			}
		}
	}
}
