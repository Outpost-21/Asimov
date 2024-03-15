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
    [HarmonyPatch(typeof(Precept_RoleMulti), "Unassign")]
    public static class Patch_Precept_RoleMulti_Unassign
	{
		[HarmonyPrefix]
		public static bool Prefix(Precept_RoleMulti __instance, Pawn p, bool generateThoughts)
		{
			// TODO: Replace with Transpiler, overriding isn't great.
			if(p?.needs?.mood == null)
			{
				if (__instance.IsAssigned(p))
				{
					IdeoRoleInstance ideoRoleInstance = __instance.chosenPawns.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
					if (ideoRoleInstance != null)
					{
						__instance.chosenPawns.Remove(ideoRoleInstance);
						__instance.chosenPawnsCache.Add(ideoRoleInstance);
						__instance.Notify_PawnUnassigned(p);
					}
				}
				return false;
			}
			return true;
		}
	}
}
