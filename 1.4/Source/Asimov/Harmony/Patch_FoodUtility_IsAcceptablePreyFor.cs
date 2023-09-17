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
    [HarmonyPatch(typeof(FoodUtility), "IsAcceptablePreyFor")]
    public static class Patch_FoodUtility_IsAcceptablePreyFor
	{
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result, Pawn predator, Pawn prey)
        {
            Comp_Automaton comp = prey.TryGetComp<Comp_Automaton>();
            if (comp != null && comp.Props.immuneToDisease)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
