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
                Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();

                if(pawnDef != null && comp != null && pawnDef.customGraphics != null)
                {
                    if (!pawnDef.customGraphics.skinColorPairs.NullOrEmpty() && !comp.resolved)
                    {
                        ColorPair pair = pawnDef.customGraphics.GetSkinColor;

                        DefModExt_AutomatonColours modExt = pawn?.kindDef?.GetModExtension<DefModExt_AutomatonColours>() ?? null;
                        if(modExt != null)
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
                        // LogUtil.LogMessage($"HeadShader = {shader}");
                        __instance.headGraphic = GraphicDatabase.Get<Graphic_Multi>(headPath, shader, pawnDef.customGraphics.bodyScale, skinFirst, skinSecond);
                    }

                    string bodyPath = PawnGraphicUtil.GetBodyPath(pawn, pawnDef.customGraphics.bodyPath);
                    if (bodyPath != null)
                    {
                        Shader shader = (ContentFinder<Texture2D>.Get(bodyPath + "_northm", false) == null) ? pawnDef.customGraphics.Shader : ShaderDatabase.CutoutComplex;
                        // LogUtil.LogMessage($"BodyShader = {shader}");
                        __instance.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(bodyPath, shader, pawnDef.customGraphics.bodyScale, skinFirst, skinSecond);
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
            else if (pawn.IsAutomaton())
            {
                Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();

                if(comp != null)
                {
                    Comp_RecolourablePawn compRecol = pawn.TryGetComp<Comp_RecolourablePawn>();
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

                    Color skinFirst = comp.skinFirst ?? pawn.story.SkinColor;
                    Color skinSecond = comp.skinSecond ?? pawn.story.SkinColor;

                    PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
                    string bodyPath = curKindLifeStage.bodyGraphicData.Graphic.path;
                    if (bodyPath != null)
                    {
                        Shader shader = (ContentFinder<Texture2D>.Get(bodyPath + "_northm", false) == null) ? ShaderDatabase.Cutout : ShaderDatabase.CutoutComplex;
                        // LogUtil.LogMessage($"BodyShader = {shader}");
                        __instance.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(bodyPath, shader, curKindLifeStage.bodyGraphicData.drawSize, skinFirst, skinSecond);
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
