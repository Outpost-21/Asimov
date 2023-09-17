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
    [HarmonyPatch(typeof(ImmunityHandler), "DiseaseContractChanceFactor", new Type[] { typeof(HediffDef), typeof(BodyPartRecord) })]
    public class Patch_ImmunityHandler_DiseaseContractChanceFactor
    {
        [HarmonyPostfix]
        public static void Postfix(ImmunityHandler __instance, float __result)
        {
            if (__result > 0f)
            {
                Comp_Automaton comp = __instance.pawn.TryGetComp<Comp_Automaton>();
                if (comp != null && comp.Props.immuneToDisease)
                {
                    __result = 0f;
                }
            }
        }
    }
}
