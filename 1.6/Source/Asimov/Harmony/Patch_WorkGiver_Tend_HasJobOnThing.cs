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
	[HarmonyPatch(typeof(WorkGiver_Tend), "HasJobOnThing")]
	public class Patch_WorkGiver_Tend_HasJobOnThing
	{
		[HarmonyPrefix]
		public static bool Prefix(Pawn pawn, Thing t, bool forced, ref bool __result)
		{
			Pawn p = t as Pawn;
			if(p != null)
            {
				if(p.IsAutomaton() || p.IsHumanlikeAutomaton())
                {
					Comp_Automaton automaton = p.TryGetComp<Comp_Automaton>();
					if(automaton != null && !automaton.Props.canBeTended)
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
