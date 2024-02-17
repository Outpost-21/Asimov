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

namespace Asimov
{
    [HarmonyPatch(typeof(ResearchProjectDef), "UnlockedDefs", MethodType.Getter)]
    public static class Patch_ResearchProjectDef_UnlockedDefs
    {
        public static bool nullCache = false;

        [HarmonyPrefix]
        public static void Prefix(ResearchProjectDef __instance)
        {
            if(__instance.cachedUnlockedDefs == null)
            {
                nullCache = true;
                LogUtil.LogMessage("Cache Null");
            }
        }

        [HarmonyPostfix]
        public static void Postfix(ResearchProjectDef __instance, ref List<Def> __result)
        {
            if (nullCache)
            {
                __instance.cachedUnlockedDefs.Concat(from x in DefDatabase<AutomatonRecipeDef>.AllDefs.Where((AutomatonRecipeDef x) => x.researchPrerequisite == __instance || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(__instance))).Select((AutomatonRecipeDef x) => x.pawnKind.race) orderby x.label select x).Distinct().ToList();
                LogUtil.LogMessage("Additions Made");
            }
            __result = __instance.cachedUnlockedDefs;
            LogUtil.LogMessage("Cache Restore");
            nullCache = false;
        }
    }
}
