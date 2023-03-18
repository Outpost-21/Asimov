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
												   where p.def.HasComp(typeof(Comp_Automaton)) && p.RaceProps.intelligence != Intelligence.Humanlike
												   select p;

		public override void PostOpen()
		{
			base.PostOpen();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
		}
	}
}
