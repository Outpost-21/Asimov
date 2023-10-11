using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
	public class PawnColumnWorker_SkillCheckbox : PawnColumnWorker_Checkbox
	{
		public override bool HasCheckbox(Pawn pawn)
		{
			Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
			if (comp != null && !pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer && pawn.workSettings != null)
			{
				return comp.EnabledWorkTypes.Contains(def.workType);
			}
			return false;
		}

		public override bool GetValue(Pawn pawn)
		{
			return pawn.workSettings.GetPriority(def.workType) > 0;
		}

		public override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.workSettings.SetPriority(def.workType, GetValue(pawn) ? 0 : 1);
		}
	}
}
