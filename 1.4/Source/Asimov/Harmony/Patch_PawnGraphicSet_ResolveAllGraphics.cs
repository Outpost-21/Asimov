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

namespace Asimov
{
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    public static class Patch_PawnGraphicSet_ResolveAllGraphics
    {
        [HarmonyPrefix]
        public static bool Prefix(PawnGraphicSet __instance)
        {
            Pawn pawn = __instance.pawn;
            if(pawn.def is PawnDef pawnDef)
            {
                //LogUtil.LogError("Shouldn't Be Here");
                Comp_PawnData comp = pawn.TryGetComp<Comp_PawnData>();

                if(pawnDef != null && comp != null && pawnDef.customGraphics != null)
                {
                    if (!pawnDef.customGraphics.skinColorPairs.NullOrEmpty())
                    {
                        ColorPair pair = pawnDef.customGraphics.GetSkinColor;
                        if(comp.skinFirst == null)
                        {
                            comp.skinFirst = pair.colorOne;
                        }
                        if(comp.skinSecond == null)
                        {
                            comp.skinSecond = pair.colorTwo;
                        }
                    }

                    Color skinFirst = comp.skinFirst ?? pawn.story.SkinColor;
                    Color skinSecond = comp.skinSecond ?? pawn.story.SkinColor;

                    string headPath = PawnGraphicUtil.GetHeadPath(pawn);
                    if (headPath != null)
                    {
                        __instance.headGraphic = PawnGraphicUtil.GetInner<Graphic_Multi>(new GraphicRequest(typeof(Graphic_Multi), headPath, (ContentFinder<Texture2D>.Get(headPath + "_northm", false) == null) ? pawnDef.customGraphics.Shader : ShaderDatabase.CutoutComplex, pawnDef.customGraphics.bodyScale, skinFirst, skinSecond, null, 0, null, headPath));
                    }

                    string bodyPath = PawnGraphicUtil.GetBodyPath(pawn, pawnDef.customGraphics.bodyPath);
                    if (bodyPath != null)
                    {
                        __instance.nakedGraphic = PawnGraphicUtil.GetInner<Graphic_Multi>(new GraphicRequest(typeof(Graphic_Multi), bodyPath, (ContentFinder<Texture2D>.Get(bodyPath + "_northm", false) == null) ? pawnDef.customGraphics.Shader : ShaderDatabase.CutoutComplex, pawnDef.customGraphics.bodyScale, skinFirst, skinSecond, null, 0, null, bodyPath));
                    }

                    if(pawnDef.pawnSettings != null)
                    {
                        if (!pawnDef.pawnSettings.allowHair)
                        {
                            pawn.story.hairDef = HairDefOf.Bald;
                        }
                        if (!pawnDef.pawnSettings.allowBeards)
                        {
                            pawn.style.beardDef = BeardDefOf.NoBeard;
                        }
                        if (ModLister.IdeologyInstalled)
                        {
                            if (!pawnDef.pawnSettings.allowTattoos)
                            {
                                pawn.style.FaceTattoo = TattooDefOf.NoTattoo_Face;
                                pawn.style.BodyTattoo = TattooDefOf.NoTattoo_Body;
                            }
                        }
                    }
                    __instance.ResolveApparelGraphics();
                    __instance.ResolveGeneGraphics();
                    PortraitsCache.SetDirty(pawn);
                    GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
                    return false;
                }
            }
            return true;
        }
    }
}
