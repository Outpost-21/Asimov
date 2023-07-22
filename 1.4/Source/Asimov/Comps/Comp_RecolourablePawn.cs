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
    public class Comp_RecolourablePawn : ThingComp
    {
        public CompProperties_RecolourablePawn Props => (CompProperties_RecolourablePawn)props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
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
        }

        public Color GetSkinColor(bool second)
        {
            Color result = Color.white;
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                Comp_PawnData comp = pawn.TryGetComp<Comp_PawnData>();
                if (comp != null)
                {
                    result = second ? (comp.skinSecond ?? Color.white) : (comp.skinFirst ?? Color.white);
                }
            }
            return result;
        }

        public void SetSkinColor(Color color, bool second)
        {
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                Comp_PawnData comp = pawn.TryGetComp<Comp_PawnData>();
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
