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
    public class Comp_AutoCrafterOverlay : ThingComp
	{
		public CompProperties_AutoCrafterOverlay Props => (CompProperties_AutoCrafterOverlay)props;

		public Comp_AutoCrafter prodComp;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);

			prodComp = parent.TryGetComp<Comp_AutoCrafter>();
		}

		public override void PostDraw()
		{
			base.PostDraw();

			if (prodComp != null)
			{
				if (prodComp.curRecipe != null)
				{
					string recipeTexPath = GetRecipeTexture(prodComp.curRecipe);
					if (!recipeTexPath.NullOrEmpty())
					{
						Rot4 rotation = parent.Rotation;
						if (parent.def.graphicData.graphicClass == typeof(Graphic_Multi))
						{
							rotation = Rot4.North;
						}
						Vector3 s = new Vector3(parent.def.graphicData.drawSize.x, 1f, parent.def.graphicData.drawSize.y);
						Vector3 drawPos = this.parent.DrawPos;
						drawPos.y += 0.085f;
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(drawPos, rotation.AsQuat, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, MaterialPool.MatFrom(recipeTexPath, parent.def.graphicData.shaderType.Shader), 0);
					}
				}
			}
		}

		public string GetRecipeTexture(AutomatonRecipeDef recipe)
		{
			string result = "";
			RecipeState state = Props.recipeStates.Find(rs => rs.recipeDef == recipe.defName);
			if (state != null && !state.states.NullOrEmpty())
			{
				result = GetCurrentState(state.states).texPath;

				if (parent.def.graphicData.graphicClass == typeof(Graphic_Multi))
				{
					result += ("_" + parent.Rotation.ToStringWord().ToLower());
				}
			}
			return result;
		}

		public ProgressState GetCurrentState(List<ProgressState> progStates)
		{
			float progress = prodComp.WorkProgress;
			for (int i = progStates.Count - 1; i >= 0; i--)
			{
				if (progress >= progStates[i].progress)
				{
					return progStates[i];
				}
			}
			return progStates.FirstOrDefault();
		}
	}
}
