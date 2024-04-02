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
    public class PawnRenderNode_AutomatonAnimalBody : PawnRenderNode
	{
		public PawnRenderNode_AutomatonAnimalBody(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
			: base(pawn, props, tree)
		{
		}

		public override Graphic GraphicFor(Pawn pawn)
		{
			Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();

			if (comp != null)
			{
				Comp_RecolourablePawn compRecol = pawn.TryGetComp<Comp_RecolourablePawn>();
				if (compRecol != null)
				{
					if (!compRecol?.Props?.skinColorPairs.NullOrEmpty() ?? false && !comp.resolved)
					{
						ColorPair pair = compRecol.Props.GetSkinColor;
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
				}

				Color skinFirst = comp.skinFirst ?? Color.white;
				Color skinSecond = comp.skinSecond ?? Color.white;

				PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
				string bodyPath = curKindLifeStage.bodyGraphicData.Graphic.path;
				if (bodyPath != null)
				{
					Shader shader = (ContentFinder<Texture2D>.Get(bodyPath + "_northm", false) == null) ? ShaderDatabase.Cutout : ShaderDatabase.CutoutComplex;
					return GraphicDatabase.Get<Graphic_Multi>(bodyPath, shader, curKindLifeStage.bodyGraphicData.drawSize, skinFirst, skinSecond);
				}
			}
			return null;
		}
	}
}
