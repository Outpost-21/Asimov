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
	public class PawnColumnWorker_Rename : PawnColumnWorker
	{
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			DrawRenameButton(rect, pawn);
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), GetMinWidth(table));
		}
		public void DrawRenameButton(Rect rect, Pawn pawn)
		{
			TooltipHandler.TipRegionByKey(rect, "Asimov.RenameAutomaton");
			if (Widgets.ButtonImage(rect, TexButton.Rename))
			{
				Find.WindowStack.Add(pawn.NamePawnDialog());
			}
		}
	}
}
