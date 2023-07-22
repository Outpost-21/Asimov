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
    [HarmonyPatch(typeof(Need), "IsFrozen", MethodType.Getter)]
    public static class Patch_Need_IsFrozen
	{
        [HarmonyPostfix]
        public static void Postfix(Need __instance, ref bool __result)
		{
            if (!__result)
			{
				if(__instance.pawn.TryGetComp<Comp_Hibernation>() != null)
                {
                    if(__instance.pawn.CurJob.def == AsimovDefOf.Asimov_Hibernate && !__instance.pawn.pather.Moving)
                    {
                        __result = true;
                    }
                }
			}
		}
	}
}
