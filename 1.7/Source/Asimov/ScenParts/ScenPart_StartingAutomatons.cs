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
    public class ScenPart_StartingAutomatons : ScenPart
    {
        public PawnKindDef automatonKind;

        public int count = 1;
        public string countBuf;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref automatonKind, "automatonKind");
            Scribe_Values.Look(ref count, "count");
        }

        public override void DoEditInterface(Listing_ScenEdit listing)
        {
            Rect scenPartRect = listing.GetScenPartRect(this, RowHeight * 2f);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(scenPartRect.TopHalf());
            ls.ColumnWidth = scenPartRect.width;
            ls.TextFieldNumeric(ref count, ref countBuf, 1f);
            ls.End();
            if(!Widgets.ButtonText(scenPartRect.BottomHalf(), CurrentAutomatonLabel().CapitalizeFirst()))
            {
                return;
            }
            List<FloatMenuOption> ops = new List<FloatMenuOption>();
            ops.Add(new FloatMenuOption("Asimov.RandomAutomaton".Translate().CapitalizeFirst(), delegate { automatonKind = null; }));
            foreach(PawnKindDef pkd in PossibleAutomatons())
            {
                ops.Add(new FloatMenuOption(pkd.LabelCap, delegate { automatonKind = pkd; }));
            }
            Find.WindowStack.Add(new FloatMenu(ops));
        }

        public IEnumerable<PawnKindDef> PossibleAutomatons()
        {
            return DefDatabase<PawnKindDef>.AllDefs.Where(pkd => pkd.race is PawnDef || pkd.race.HasComp(typeof(Comp_Automaton)));
        }

        public string CurrentAutomatonLabel()
        {
            if(automatonKind == null)
            {
                return "Asimov.RandomAutomaton".TranslateSimple();
            }
            return automatonKind.label;
        }

        public override string Summary(Scenario scen)
        {
            return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
        }

        public override IEnumerable<string> GetSummaryListEntries(string tag)
        {
            if(tag == "PlayerStartsWith")
            {
                yield return CurrentAutomatonLabel().CapitalizeFirst() + " x" + count;
            }
        }

        public override IEnumerable<Thing> PlayerStartingThings()
        {
            for (int i = 0; i < count; i++)
            {
                PawnKindDef kindDef;
                if(automatonKind == null)
                {
                    PossibleAutomatons().TryRandomElement(out kindDef);
                }
                else
                {
                    kindDef = automatonKind;
                }
                Pawn automaton = PawnGenerator.GeneratePawn(kindDef, Faction.OfPlayer);
                if (automaton.Name == null || automaton.Name.Numerical)
                {
                    automaton.Name = PawnBioAndNameGenerator.GeneratePawnName(automaton);
                }
                yield return automaton;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ((automatonKind != null) ? automatonKind.GetHashCode() : 0) ^ count;
        }
    }
}
