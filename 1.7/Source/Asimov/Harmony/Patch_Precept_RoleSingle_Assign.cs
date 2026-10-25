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
    [HarmonyPatch(typeof(Precept_RoleSingle), "Assign")]
    public static class Patch_Precept_RoleSingle_Assign
	{
		[HarmonyPrefix]
		public static void Prefix(Precept_RoleSingle __instance, Pawn p, ref bool addThoughts)
		{
			if(__instance.ChosenPawnValue?.needs?.mood == null)
            {
				addThoughts = false;
            }
		}
	}
}
