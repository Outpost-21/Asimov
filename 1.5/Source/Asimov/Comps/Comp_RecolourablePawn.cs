﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class Comp_RecolourablePawn : ThingComp
    {
        Pawn pawn => parent as Pawn;

        public CompProperties_RecolourablePawn Props => (CompProperties_RecolourablePawn)props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            if (!pawn.Faction?.IsPlayer ?? false)
            {
                yield break;
            }
            if (Props.channelOne)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Asimov.FirstColorLabel".Translate(),
                    defaultDesc = "Asimov.FirstColorDescription".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Asimov/UI/ColorWheel"),
                    action = () => Find.WindowStack.Add(new Popup_ColourPicker(this, false))
                };
            }
            if (Props.channelTwo)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Asimov.SecondColorLabel".Translate(),
                    defaultDesc = "Asimov.SecondColorDescription".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Asimov/UI/ColorWheel"),
                    action = () => Find.WindowStack.Add(new Popup_ColourPicker(this, true))
                };
            }
            yield break;
        }

        public Color GetSkinColor(bool second)
        {
            Color result = Color.white;
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
                if (comp != null)
                {
                    result = second ? (comp.skinSecond ?? Color.white) : (comp.skinFirst ?? Color.white);
                }
            }
            return result;
        }

        public virtual void SetSkinColor(Color color, bool second)
        {
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
                if (comp != null)
                {
                    if (!second)
                    {
                        comp.skinFirst = color;
                    }
                    else
                    {
                        comp.skinSecond = color;
                    }
                    comp.ResolveGraphics();
                }
            }
        }
    }
}
