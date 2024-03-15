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
    [HarmonyPatch(typeof(Recipe_Surgery), "AvailableOnNow")]
    public static class Patch_Recipe_Surgery_AvailableOnNow
	{
		[HarmonyPrefix]
		public static bool Prefix(Recipe_Surgery __instance, ref bool __result, Thing thing, BodyPartRecord part = null)
		{
			PawnDef pawnDef = thing.def as PawnDef;
			if (__instance.recipe.defName.Contains("Administer_") && pawnDef != null)
            {
				Comp_Automaton comp = thing.TryGetComp<Comp_Automaton>();
				if(comp != null)
                {
					if(!comp.Props.disabledNeeds.NullOrEmpty() && comp.Props.disabledNeeds.Contains(AsimovDefOf.DrugDesire))
                    {
						__result = false;
						return false;
                    }
                }
            }
			return true;
		}
	}
}
