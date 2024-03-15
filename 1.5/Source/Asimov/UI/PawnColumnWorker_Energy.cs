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
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Energy : PawnColumnWorker
	{
		public static readonly Texture2D EnergyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(252, byte.MaxValue, byte.MaxValue, 65));

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Need_Energy need = pawn.needs.TryGetNeed<Need_Energy>();
			if (need != null)
			{
				Widgets.FillableBar(rect.ContractedBy(4f), need.CurLevelPercentage, EnergyBarTex, BaseContent.ClearTex, doBorder: false);
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, need.CurLevelPercentage.ToStringPercent());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 120);
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), GetMinWidth(table));
		}
	}
}
