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
    public class PawnRenderNode_AutomatonHead : PawnRenderNode
	{
		public PawnRenderNode_AutomatonHead(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
			: base(pawn, props, tree)
		{
		}

		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (props.overrideMeshSize.HasValue)
			{
				return MeshPool.GetMeshSetForSize(props.overrideMeshSize.Value.x, props.overrideMeshSize.Value.y);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn);
		}

		public override Graphic GraphicFor(Pawn pawn)
		{
			PawnDef pawnDef = pawn.def as PawnDef;
			Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();

			if (!pawnDef.customGraphics.skinColorPairs.NullOrEmpty() && !comp.resolved)
			{
				ColorPair pair = pawnDef.customGraphics.GetSkinColor;

				DefModExt_AutomatonColours modExt = pawn?.kindDef?.GetModExtension<DefModExt_AutomatonColours>() ?? null;
				if (modExt != null)
				{
					pair = modExt.GetSkinColor;
				}

				if (comp.skinFirst == null)
				{
					comp.skinFirst = pair.colorOne;
				}
				if (comp.skinSecond == null)
				{
					comp.skinSecond = pair.colorTwo;
				}
				comp.resolved = true;
			}

			Color skinFirst = comp.skinFirst ?? pawn.story.SkinColor;
			Color skinSecond = comp.skinSecond ?? pawn.story.SkinColor;

			string headPath = PawnGraphicUtil.GetHeadPath(pawn);
			if (headPath != null)
			{
				Shader shader = (ContentFinder<Texture2D>.Get(headPath + "_northm", false) == null) ? pawnDef.customGraphics.Shader : ShaderDatabase.CutoutComplex;
				return GraphicDatabase.Get<Graphic_Multi>(headPath, shader, pawnDef.customGraphics.headScale, skinFirst, skinSecond);
			}
			return null;
		}
	}
}
