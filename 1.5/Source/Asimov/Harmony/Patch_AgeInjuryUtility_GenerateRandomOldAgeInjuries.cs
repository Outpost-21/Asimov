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
    [HarmonyPatch(typeof(AgeInjuryUtility), "GenerateRandomOldAgeInjuries")]
    public static class Patch_AgeInjuryUtility_GenerateRandomOldAgeInjuries
	{
		[HarmonyPrefix]
		public static bool Prefix(Pawn pawn)
		{
			PawnDef def = pawn.def as PawnDef;
			if(def != null && def.pawnSettings.immuneToAge)
            {
				return false;
            }
			Comp_Automaton comp = pawn.GetComp<Comp_Automaton>();
			if(comp != null && comp.Props.immuneToDisease)
            {
				return false;
            }
			return true;
		}
	}
}
