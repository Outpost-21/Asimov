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
    [HarmonyPatch(typeof(MainTabWindow_Research), "VisibleResearchProjects", MethodType.Getter)]
    public static class Patch_MainTabWindow_Research_VisibleResearchProjects
    {
        public static bool resolved;

        [HarmonyPrefix]
        public static void Prefix(MainTabWindow_Research __instance)
        {
            if (__instance.cachedVisibleResearchProjects == null)
            {
                resolved = false;
            }
        }

        [HarmonyPostfix]
        public static void Postfix(MainTabWindow_Research __instance, ref List<ResearchProjectDef> __result)
        {
            if (!resolved)
            {
                if (AsimovStartup.researchHideFlag_wirelessCharging)
                {
                    if (__instance.cachedVisibleResearchProjects.Contains(AsimovDefOf.Asimov_WirelessCharging))
                    {
                        __instance.cachedVisibleResearchProjects.Remove(AsimovDefOf.Asimov_WirelessCharging);
                    }
                }
                resolved = true;
            }
        }
    }
}
