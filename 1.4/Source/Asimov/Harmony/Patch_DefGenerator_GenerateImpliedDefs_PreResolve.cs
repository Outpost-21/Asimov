using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld.Planet;

namespace Asimov
{
    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class Patch_DefGenerator_GenerateImpliedDefs_PreResolve
	{
        [HarmonyPostfix]
        public static void Postfix()
		{
            foreach(AutomatonRecipeDef def in ImpliedAutomatonRecipeDefs())
            {
                DefGenerator.AddImpliedDef(def);
            }
		}

        public static IEnumerable<AutomatonRecipeDef> ImpliedAutomatonRecipeDefs()
        {
            foreach(ThingDef thing in DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.GetCompProperties<CompProperties_Automaton>() != null))
            {
                yield return CreateAutomatonRecipeFromDef(thing);
            }
        }

        public static AutomatonRecipeDef CreateAutomatonRecipeFromDef(ThingDef def)
        {
            CompProperties_Automaton comp = def.GetCompProperties<CompProperties_Automaton>();
            AutomatonRecipeDef recipeDef = new AutomatonRecipeDef();
            recipeDef.defName = "Make_" + def.defName;
            string text = def.label;
            recipeDef.label = "RecipeMake".Translate(text);
            recipeDef.jobString = "RecipeMakeJobString".Translate(text);
            recipeDef.modContentPack = def.modContentPack;
            recipeDef.workAmount = comp.recipeProps.workToMake;
            SetIngredients(comp.recipeProps.costList, recipeDef);
            recipeDef.researchPrerequisites = comp.recipeProps.researchPrerequisites;
            recipeDef.memePrerequisites = comp.recipeProps.memePrerequisites;
            recipeDef.memePrerequisitesAny = comp.recipeProps.memePrerequisitesAny;
            recipeDef.pawnKind = comp.recipeProps.pawnKind;
            AddRecipeToBuilding(recipeDef, comp.recipeProps.building);
            //recipeDef.description = "RecipeMakeDescription".Translate(recipeDef.pawnKind.race.label);
            //recipeDef.descriptionHyperlinks.Add(recipeDef.pawnKind.race);

            return recipeDef;
        }

        public static void AddRecipeToBuilding(AutomatonRecipeDef recipe, ThingDef building)
        {
            CompProperties_AutoCrafter props = building.GetCompProperties<CompProperties_AutoCrafter>();
            if(props == null)
            {
                LogUtil.LogError($"{recipe.defName} cannot be assigned to defined building as it does not have CompProperties_AutoCrafter defined.");
                return;
            }
            if (props.recipes.NullOrEmpty())
            {
                props.recipes = new List<AutomatonRecipeDef>();
            }
            props.recipes.Add(recipe);
        }

        public static void SetIngredients(List<ThingDefCountClass> costList, AutomatonRecipeDef recipeDef)
        {
            recipeDef.ingredients.Clear();
            recipeDef.adjustedCount = 1;
            if (costList.NullOrEmpty())
            {
                return;
            }
            foreach (ThingDefCountClass cost in costList)
            {
                IngredientCount ingredientCount2 = new IngredientCount();
                ingredientCount2.SetBaseCount(cost.count);
                ingredientCount2.filter.SetAllow(cost.thingDef, allow: true);
                recipeDef.ingredients.Add(ingredientCount2);
                recipeDef.fixedIngredientFilter.SetAllow(cost.thingDef, allow: true);
            }
        }
	}
}
