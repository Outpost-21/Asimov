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
    [HarmonyPatch(typeof(ThingDef), "IsIngestible", MethodType.Getter)]
    public class Patch_ThingDef_IsIngestible
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result, ref ThingDef __instance)
        {
            if (__instance.IsCorpse)
            {
                if (__instance.HasModExtension<DefModExt_NonIngestible>())
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
