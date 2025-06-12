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
	public class MainButtonWorker_ToggleAutomatonTab : MainButtonWorker_ToggleTab
	{
		public override bool Disabled
		{
			get
			{
				if (base.Disabled)
				{
					return true;
				}
				Map currentMap = Find.CurrentMap;
				if (currentMap != null)
				{
					List<Pawn> list = currentMap.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].TryGetComp<Comp_Automaton>() != null/* && list[i].RaceProps.intelligence != Intelligence.Humanlike*/)
						{
							return false;
						}
					}
					List<Pawn> list2 = currentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].TryGetComp<Comp_Automaton>() != null/* && list2[j].RaceProps.intelligence != Intelligence.Humanlike*/)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public override bool Visible => !Disabled;
	}
}
