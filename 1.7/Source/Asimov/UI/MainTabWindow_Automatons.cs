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
	public class MainTabWindow_Automatons : MainTabWindow_PawnTable
	{
		public override PawnTableDef PawnTableDef => AsimovDefOf.Asimov_Automatons;

		public override IEnumerable<Pawn> Pawns => from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
												   where p.def.HasComp(typeof(Comp_Automaton)) /*&& p.RaceProps.intelligence != Intelligence.Humanlike*/
												   select p;

		public override void PostOpen()
		{
			base.PostOpen();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
            if (Event.current.type != EventType.Layout)
            {
                DoManualPrioritiesCheckbox();
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                Text.Anchor = TextAnchor.UpperCenter;
                Text.Font = GameFont.Tiny;
                Widgets.Label(new Rect(370f, rect.y + 5f, 160f, 30f), "<= " + "HigherPriority".Translate());
                Widgets.Label(new Rect(630f, rect.y + 5f, 160f, 30f), "LowerPriority".Translate() + " =>");
                GUI.color = Color.white;
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

        public void DoManualPrioritiesCheckbox()
        {
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect = new Rect(5f, 5f, 140f, 30f);
            bool useWorkPriorities = AsimovMod.settings.doManualPriorities;
            Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref AsimovMod.settings.doManualPriorities);
            if (useWorkPriorities != AsimovMod.settings.doManualPriorities)
            {
                foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_Alive)
                {
                    if (item.Faction == Faction.OfPlayer && item.workSettings != null)
                    {
                        item.workSettings.Notify_UseWorkPrioritiesChanged();
                    }
                }
            }
            if (!AsimovMod.settings.doManualPriorities)
            {
                UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
            }
        }
    }
}
