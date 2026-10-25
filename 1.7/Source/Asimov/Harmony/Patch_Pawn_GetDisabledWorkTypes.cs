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
    [HarmonyPatch(typeof(Pawn), "GetDisabledWorkTypes")]
    public static class Patch_Pawn_GetDisabledWorkTypes
    {
        [HarmonyPostfix]
        public static void Postfix(bool permanentOnly, Pawn __instance, ref List<WorkTypeDef> __result)
        {
            Comp_Automaton comp = __instance.TryGetComp<Comp_Automaton>();
            if (comp != null && !comp.DisabledWorkTypes.NullOrEmpty())
            {
                if (__instance.cachedDisabledWorkTypes.NullOrEmpty())
                {
                    __instance.cachedDisabledWorkTypes = new List<WorkTypeDef>();
                }
                foreach(WorkTypeDef workTypeDef in comp.DisabledWorkTypes)
                {
                    if (!__instance.cachedDisabledWorkTypes.Contains(workTypeDef))
                    {
                        __instance.cachedDisabledWorkTypes.Add(workTypeDef);
                    }
                }
            }
        }
	}
}
