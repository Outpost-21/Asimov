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
    [HarmonyPatch(typeof(FloatMenuOptionProvider_DraftedTend), "AppliesInt")]
    public static class Patch_FloatMenuOptionProvider_DraftedTend_AppliesInt
    {
        [HarmonyPrefix]
        public static bool Prefix(FloatMenuContext context)
		{
			if (context.FirstSelectedPawn.IsAutomaton())
			{
				if (context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Caring))
				{
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
