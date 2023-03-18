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
        // Genes
        // Genes are disabled on Asimov races.

        // General Stuff
        public bool immuneToAge = false;
        public bool useHumanRecipes = false;

        // Body
        public List<HeadTypeDef> headTypeWhitelist = new List<HeadTypeDef>();
        public List<BodyTypeDef> bodyTypeWhitelist = new List<BodyTypeDef>();
        public List<BodyTypeDef> bodyTypeWhitelistFemale = new List<BodyTypeDef>();

        // Styles
        public bool allowHair = true;
        public List<string> hairTagWhitelist = new List<string>();
        public bool allowBeards = true;
        public List<string> beardTagWhitelist = new List<string>();
        public bool allowTattoos = true;
        public List<string> tattooTagWhitelist = new List<string>();

        // Thoughts
        public bool restrictThoughtsToRace = false;
        public bool restrictRaceThoughts = false;
        public List<ThoughtDef> thoughtBlacklist = new List<ThoughtDef>();
        public List<ThoughtDef> thoughtWhitelist = new List<ThoughtDef>();

        // Traits
        public bool restrictTraitsToRace = false;
        public bool restrictRaceTraits = false;
        public List<TraitDef> traitBlacklist = new List<TraitDef>();
        public List<TraitDef> traitWhitelist = new List<TraitDef>();
        public IntRange traitCount = new IntRange(2, 3);

        // Weapons
        public bool restrictWeaponsToRace = false;
        public bool restrictRaceWeapons = false;
        public List<ThingDef> weaponBlacklist = new List<ThingDef>();
        public List<ThingDef> weaponWhitelist = new List<ThingDef>();

        // Apparel
        public bool restrictApparelToRace = false;
        public bool restrictRaceApparel = false;
        public List<ThingDef> apparelBlacklist = new List<ThingDef>();
        public List<ThingDef> apparelWhitelist = new List<ThingDef>();

        // Food
        public bool restrictFoodToRace = false;
        public bool restrictRaceFood = false;
        public List<ThingDef> foodBlacklist = new List<ThingDef>();
        public List<ThingDef> foodWhiteList = new List<ThingDef>();

        // Buildings
        public bool restrictBuildingsToRace = false;
        public bool restrictRaceBuildings = false;
        public List<ThingDef> buildingBlacklist = new List<ThingDef>();
        public List<ThingDef> buildingWhitelist = new List<ThingDef>();

        // Backstories
        public bool restrictBackstoriesToRace = false;
        public bool restrictRaceToBackstories = false;
        public List<string> backstoryBlacklist = new List<string>();
        public List<string> backstoryWhitelist = new List<string>();
    }
}
