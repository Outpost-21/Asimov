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
using System.Reflection;
using System.Reflection.Emit;

namespace Asimov
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateTraitsFor", null)]
    public static class Patch_PawnGenerator_GenerateTraitsFor
    {
        [HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> list = instructions.ToList();
			MethodInfo allDefsMethodInfo = AccessTools.Property(typeof(DefDatabase<TraitDef>), "AllDefsListForReading").GetGetMethod();
			foreach (CodeInstruction instruction in list)
			{
				yield return instruction;
				if (instruction.opcode == OpCodes.Call && instruction.OperandIs(allDefsMethodInfo))
				{
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return CodeInstruction.Call(typeof(Patch_PawnGenerator_GenerateTraitsFor), "GenerateTraitsForCI");
				}
			}
		}

		public static IEnumerable<TraitDef> GenerateTraitsForCI(List<TraitDef> traits, Pawn p)
        {
			return traits.Where(td => td.CanHaveTrait(p.def));
        }
	}
}
