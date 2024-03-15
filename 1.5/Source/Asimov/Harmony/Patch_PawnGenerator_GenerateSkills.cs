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
    [HarmonyPatch(typeof(PawnGenerator), "GenerateSkills")]
    public static class Patch_PawnGenerator_GenerateSkills
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn)
        {
            Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
            if (comp != null)
            {
                if (comp.Props.clearPassions)
                {
                    for (int i = 0; i < pawn.skills.skills.Count(); i++)
                    {
                        pawn.skills.skills[i].passion = Passion.None;
                    }
                }
            }
        }
    }
}
