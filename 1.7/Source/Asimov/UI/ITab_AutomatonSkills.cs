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
    public class ITab_AutomatonSkills : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(200f, 400f);

        public ITab_AutomatonSkills()
        {
            this.size = WinSize;
            this.labelKey = "Asimov.TabSkills";
        }

        public override void FillTab()
        {
            Rect rect = new Rect(0f, 0f, size.x, size.y);
            Rect inRect = rect.ContractedBy(18f);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            Pawn pawn = SelPawn;
            Comp_Automaton comp = pawn.TryGetComp<Comp_Automaton>();
            if(pawn != null && comp != null)
            {
                foreach (SkillRecord skillRecord in pawn.skills.skills)
                {
                    if (!skillRecord.TotallyDisabled)
                    {
                        listing.LabelDouble(skillRecord.def.LabelCap, skillRecord.Level.ToString());
                    }
                }
            }
            listing.End();
        }

        public override bool IsVisible => base.IsVisible && SelPawn.IsAutomaton();
    }
}
