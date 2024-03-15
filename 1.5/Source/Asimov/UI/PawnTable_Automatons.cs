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
	public class PawnTable_Automatons : PawnTable
	{
		public override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return input.OrderBy((Pawn p) => p.KindLabel).ThenBy((Pawn p) => p.Label);
		}

		public PawnTable_Automatons(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
			: base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
