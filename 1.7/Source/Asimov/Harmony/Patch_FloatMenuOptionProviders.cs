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
    [HarmonyPatch(typeof(FloatMenuOptionProvider_DraftedTend), "IsValidTendTarget")]
    public static class Patch_FloatMenuOptionProvider_DraftedTend_IsValidTendTarget
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn doctor, Pawn patient, ref bool __result)
		{
			if (patient.IsAutomaton() || patient.IsHumanlikeAutomaton())
            {
                if (!patient.GetAutomatonComp()?.Props?.canBeTended ?? true)
                {
                    __result = false;
					return false;
				}
			}
			return true;
		}
    }

    [HarmonyPatch(typeof(FloatMenuOptionProvider_Equip), "GetSingleOptionFor")]
    public static class FloatMenuOptionProvider_Equip_GetSingleOptionFor
    {
        [HarmonyPrefix]
        public static bool Prefix(Thing clickedThing, FloatMenuContext context, FloatMenuOption __result)
        {
            Pawn pawn = context.FirstSelectedPawn;
            if (pawn.IsAutomaton() && clickedThing.HasComp<CompEquippable>())
            {
                Comp_Automaton comp = pawn.GetComp<Comp_Automaton>();
                if (comp != null && !comp.Props.canUseRanged)
                {
                    __result = null;
                    return false;
                }
            }
            return true;
        }
    }
}
