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

    [HarmonyPatch(typeof(StunHandler), "AffectedByEMP", MethodType.Getter)]
    public class Patch_StunHandler_AffectedByEMP
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, ref StunHandler __instance, Thing ___parent)
        {
            if (___parent is Pawn)
            {
                Comp_Automaton comp = ___parent.TryGetComp<Comp_Automaton>();
                if (comp != null && !comp.Props.affectedByEMP)
                {
                    __result = true;
                    return;
                }
            }
        }
    }
}
