using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class StockGenerator_Automatons : StockGenerator
    {
        public bool respectPopulationIntent;

        public JoinStatus joinStatus = JoinStatus.JoinAsColonist;

        public List<PawnKindDef> pawnKinds;

        public List<HediffDef> hediffs;

        public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			if (respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
			{
				yield break;
			}
            int count = countRange.RandomInRange;
            for (int i = 0; i < count; i++)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKinds.RandomElement(), null, PawnGenerationContext.NonPlayer, forTile, forceGenerateNewPawn: true));
                if(pawn?.guest?.joinStatus != null)
                {
                    pawn.guest.joinStatus = joinStatus;
                }
                if (!hediffs.NullOrEmpty())
                {
                    for (int j = 0; j < hediffs.Count; j++)
                    {
                        pawn.health.AddHediff(hediffs[j]);
                    }
                }
                yield return pawn;
            }
		}

        public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
        {
            foreach(string item in base.ConfigErrors(parentDef))
            {
                yield return item;
            }
            if (pawnKinds.NullOrEmpty())
            {
                yield return "StockGenerator_Automatons added to trader without any pawnKinds, at least one is required.";
            }
        }

        public override bool HandlesThingDef(ThingDef thingDef)
        {
            if (thingDef.category == ThingCategory.Pawn)
            {
                return thingDef.tradeability != Tradeability.None;
            }
            return false;
        }
    }
}
