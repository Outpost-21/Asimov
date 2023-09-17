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
    [HarmonyPatch(typeof(PawnGenerator), "FinalLevelOfSkill")]
    public static class Patch_PawnGenerator_FinalLevelOfSkill
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, Pawn pawn, SkillDef sk)
        {
            Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
            if (comp != null)
            {
                if (!comp.Props.skillSettings.NullOrEmpty())
                {
                    if (comp.Props.skillSettings.Any(sr => sr.skill == sk))
                    {
                        __result = comp.Props.skillSettings.Find(sr => sr.skill == sk).level;
                    }
                    else if (comp.Props.flattenSkills)
                    {
                        __result = comp.Props.defaultSkillLevel;
                    }
                }
            }
        }
    }
}
