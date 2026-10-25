using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace Asimov
{
    [HarmonyPatch(typeof(PawnUIOverlay), "DrawPawnGUIOverlay")]
    public static class Patch_PawnUIOverlay_DrawPawnGUIOverlay
    {
        [HarmonyPostfix]
        public static void Postfix(PawnUIOverlay __instance)
        {
            Pawn pawn = __instance.pawn;
            if(!pawn.Spawned || pawn.Map.fogGrid.IsFogged(pawn.Position) || WorldComponent_GravshipController.CutsceneInProgress)
            {
                return;
            }
            if (pawn.IsHumanlikeAutomaton())
            {
                return;
            }
            if (!pawn.IsPlayerAutomaton())
            {
                return;
            }
            Vector2 pos = GenMapUI.LabelDrawPosFor(pawn, -0.6f);
            GenMapUI.DrawPawnLabel(pawn, pos);
        }
    }
}
