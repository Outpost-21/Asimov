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
    public class PawnSettings
    {
        // General Stuff
        public bool immuneToAge = false;
        public bool generateRelations = true;
        public bool colonyCaresIfDead = false;
        public bool hasGrowthMoments = true;

        // Body
        public List<HeadTypeDef> headTypeWhitelist = new List<HeadTypeDef>();
        public List<BodyTypeDef> bodyTypeWhitelist = new List<BodyTypeDef>();

        // Styles
        public bool allowHair = true;
        public List<string> hairTagWhitelist = new List<string>(); //TODO
        public bool allowBeards = true;
        public List<string> beardTagWhitelist = new List<string>(); //TODO
        public bool allowTattoos = true;
        public List<string> tattooTagWhitelist = new List<string>(); //TODO

        // Thoughts
        public bool onlyRestrictedThoughts = false;
        public List<ThoughtDef> thoughts = new List<ThoughtDef>();

        // Traits
        public bool onlyRestrictedTraits = false;
        public List<TraitDef> traits = new List<TraitDef>();
        public bool hasTraits = true;

        // Backstories
        public bool onlyRestrictedBackstories = false;
        public List<string> backstories = new List<string>();

        // Apparel
        public bool onlyRestrictedApparel = false;
        public List<string> apparel = new List<string>();
    }
}
