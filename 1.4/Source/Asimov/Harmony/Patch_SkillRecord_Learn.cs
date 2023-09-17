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
    [HarmonyPatch(typeof(SkillRecord), "Learn")]
    public class Patch_SkillRecord_Learn
    {
        [HarmonyPrefix]
        public static bool Prefix(SkillRecord __instance, float xp, bool direct = false)
        {
            Comp_Automaton comp = __instance.Pawn.TryGetComp<Comp_Automaton>();
            if(comp != null)
            {
                if (xp >= 0)
                {
                    xp *= comp.Props.skillGainMulti;
                }
                else
                {
                    xp *= comp.Props.skillLossMulti;
                }
            }
            return true;
        }
    }
}
