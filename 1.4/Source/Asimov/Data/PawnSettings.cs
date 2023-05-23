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
        public bool blockGeneMechanics = false;

        public bool restrictGenesToRace = false; //TODO
        public bool restrictRaceGenes = false; //TODO
        public List<GeneDef> geneWhitelist = new List<GeneDef>(); //TODO
        public List<GeneDef> geneBlacklist = new List<GeneDef>(); //TODO

        public bool restrictXenotypesToRace = false; //TODO
        public bool restrictRaceXenotypes = false; //TODO
        public List<XenotypeDef> xenotypeWhitelist = new List<XenotypeDef>(); //TODO
        public List<XenotypeDef> xenotypeBlacklist = new List<XenotypeDef>(); //TODO

        // General Stuff
        public bool immuneToAge = false; //TODO
        public bool useHumanRecipes = false; //TODO

        // Body
        public List<HeadTypeDef> headTypeWhitelist = new List<HeadTypeDef>(); //TODO
        public List<BodyTypeDef> bodyTypeWhitelist = new List<BodyTypeDef>(); //TODO
        public List<BodyTypeDef> bodyTypeWhitelistFemale = new List<BodyTypeDef>(); //TODO

        // Styles
        public bool allowHair = true;
        public List<string> hairTagWhitelist = new List<string>(); //TODO
        public bool allowBeards = true;
        public List<string> beardTagWhitelist = new List<string>(); //TODO
        public bool allowTattoos = true;
        public List<string> tattooTagWhitelist = new List<string>(); //TODO

        // Thoughts
        public bool restrictThoughtsToRace = false;
        public bool restrictRaceThoughts = false;
        public List<ThoughtDef> thoughtBlacklist = new List<ThoughtDef>(); //TODO
        public List<ThoughtDef> thoughtWhitelist = new List<ThoughtDef>(); //TODO

        // Traits
        public bool restrictTraitsToRace = false;
        public bool restrictRaceTraits = false;
        public List<TraitDef> traitBlacklist = new List<TraitDef>(); //TODO
        public List<TraitDef> traitWhitelist = new List<TraitDef>(); //TODO
        public IntRange traitCount = new IntRange(2, 3);

        // Weapons
        public bool restrictWeaponsToRace = false;
        public bool restrictRaceWeapons = false;
        public List<ThingDef> weaponBlacklist = new List<ThingDef>(); //TODO
        public List<ThingDef> weaponWhitelist = new List<ThingDef>(); //TODO

        // Apparel
        public bool restrictApparelToRace = false;
        public bool restrictRaceApparel = false;
        public List<ThingDef> apparelBlacklist = new List<ThingDef>(); //TODO
        public List<ThingDef> apparelWhitelist = new List<ThingDef>(); //TODO

        // Food
        public bool restrictFoodToRace = false;
        public bool restrictRaceFood = false;
        public List<ThingDef> foodBlacklist = new List<ThingDef>(); //TODO
        public List<ThingDef> foodWhiteList = new List<ThingDef>(); //TODO

        // Buildings
        public bool restrictBuildingsToRace = false;
        public bool restrictRaceBuildings = false;
        public List<ThingDef> buildingBlacklist = new List<ThingDef>(); //TODO
        public List<ThingDef> buildingWhitelist = new List<ThingDef>(); //TODO

        // Backstories
        public bool restrictBackstoriesToRace = false;
        public bool restrictRaceToBackstories = false;
        public List<string> backstoryBlacklist = new List<string>(); //TODO
        public List<string> backstoryWhitelist = new List<string>(); //TODO
    }
}
