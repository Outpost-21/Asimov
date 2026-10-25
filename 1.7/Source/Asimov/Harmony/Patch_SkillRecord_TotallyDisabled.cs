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
    [HarmonyPatch(typeof(SkillRecord), "TotallyDisabled", MethodType.Getter)]
    public class Patch_SkillRecord_TotallyDisabled
    {
        [HarmonyPrefix]
        public static bool Prefix(SkillRecord __instance, bool __result)
        {
            if (__instance.cachedTotallyDisabled == BoolUnknown.Unknown)
            {
                Comp_Automaton comp = __instance.Pawn.TryGetComp<Comp_Automaton>();
                if (comp != null)
                {
                    if (__instance.def == SkillDefOf.Shooting)
                    {
                        if (comp.Props.canUseRanged)
                        {
                            __instance.cachedTotallyDisabled = BoolUnknown.False;
                        }
                        else
                        {
                            __instance.cachedTotallyDisabled = BoolUnknown.True;
                        }
                    }
                    if (__instance.def == SkillDefOf.Melee)
                    {
                        if (comp.Props.canUseMelee)
                        {
                            __instance.cachedTotallyDisabled = BoolUnknown.False;
                        }
                        else
                        {
                            __instance.cachedTotallyDisabled = BoolUnknown.True;
                        }
                    }
                }
            }
            return true;
        }
    }
}
