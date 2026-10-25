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
    [HarmonyPatch(typeof(HistoryAutoRecorderWorker_ColonistMood), "PullRecord")]
    public static class Patch_HistoryAutoRecorderWorker_ColonistMood_PullRecord
    {
        [HarmonyPrefix]
        public static bool Prefix(HistoryAutoRecorderWorker_ColonistMood __instance, ref float __result)
        {
            if(!PawnsFinder.AllMaps_FreeColonists.Any(x => x.needs.mood != null))
            {
                __result = 0f;
                return false;
            }
            return true;
        }
	}
}
