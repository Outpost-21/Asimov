using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Asimov
{
    public class ITab_AutoCrafter : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(420f, 300f);

        [TweakValue("Interface", 0f, 128f)]
        private static float PasteX = 48f;
        [TweakValue("Interface", 0f, 128f)]
        private static float PasteY = 3f;
        [TweakValue("Interface", 0f, 32f)]
        private static float PasteSize = 24f;

        public ITab_AutoCrafter()
        {
            this.size = ITab_AutoCrafter.WinSize;
            this.labelKey = "Asimov.TabAutoCraft";
        }

        public Building_AutoCrafter SelTable
        {
            get
            {
                return (Building_AutoCrafter)base.SelThing;
            }
        }

        public override void FillTab()
        {
            Rect rect = new Rect(ITab_AutoCrafter.WinSize.x - ITab_AutoCrafter.PasteX, ITab_AutoCrafter.PasteY, ITab_AutoCrafter.PasteSize, ITab_AutoCrafter.PasteSize);
            Rect rect2 = new Rect(0f, 0f, ITab_AutoCrafter.WinSize.x, ITab_AutoCrafter.WinSize.y).ContractedBy(10f);
            Func<List<FloatMenuOption>> recipeOptionsMaker = delegate ()
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (AutomatonRecipeDef recipe in SelTable.def.GetCompProperties<CompProperties_AutoCrafter>().recipes)
                {
                    if (recipe.researchPrerequisites.NullOrEmpty() || recipe.researchPrerequisites.All(rpd => rpd.IsFinished))
                    {
                        list.Add(new FloatMenuOption(recipe.LabelCap, delegate ()
                        {
                            Comp_AutoCrafter comp = SelTable.GetComp<Comp_AutoCrafter>();
                            if (comp.curRecipe != recipe)
                            {
                                comp.curRecipe = recipe;
                                comp.repeatTarget = 0;
                                SelTable.GetComp<Comp_AutoCrafter>().ResetWorkTick();
                            }
                        }, MenuOptionPriority.Default, null, null, 29f, (Rect rect3) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe), null));
                    }
                }
                if (!list.Any<FloatMenuOption>())
                {
                    list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                return list;
            };
            DrawRecipeCard(rect2, recipeOptionsMaker);
        }

        public void DrawRecipeCard(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker)
        {
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            Rect rect2 = new Rect(0f, 0f, 150f, 29f);
            if (Widgets.ButtonText(rect2, "Asimov.SetAutoBill".Translate(), true, false, true))
            {
                Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
            }
            UIHighlighter.HighlightOpportunity(rect2, "Asimov.SetAutoBill");

            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Comp_AutoCrafter comp = SelTable.GetComp<Comp_AutoCrafter>();
            AutomatonRecipeDef recipe = comp.curRecipe;
            string producingItem = "";
            string producingDescription = "";
            string producedItems = "";
            string inputListing = "Input: ";
            string itemInfo = "No Item Being Produced";
            Texture itemTexture = ContentFinder<Texture2D>.Get("UI/Toolbox/NoRecipe", true);
            // Button - Repeat Recipe
            Rect rect6 = new Rect(2f, 30f, 250f, 30f);
            //Texture2D RepeatIcon = ContentFinder<Texture2D>.Get("UI/Toolbox/RepeatIcon", true);
            //if (Widgets.ButtonImage(rect6, RepeatIcon, Color.white, Color.white * GenUI.SubtleMouseoverColor))
            //{
            //    this.SelTable.GetComp<Comp_AutomatedProducer>().repeatCurrentRecipe = !this.SelTable.GetComp<Comp_AutomatedProducer>().repeatCurrentRecipe;
            //    SoundDefOf.Click.PlayOneShotOnCamera(null);
            //}
            //TooltipHandler.TipRegion(rect6, "Asimov.RepeatAutoBillTip".Translate());

            // Repeat Quantity Input
            Utility_AutoProducerCard.DoConfigInterface(rect6.AtZero(), Color.white, comp);

            // Button - Remove Recipe
            Rect rect5 = new Rect(rect.width - 26f, 28f, 24f, 24f);
            Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
            if (Widgets.ButtonImage(rect5, DeleteX, Color.white, Color.white * GenUI.SubtleMouseoverColor))
            {
                comp.CancelRecipe();
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }
            TooltipHandler.TipRegion(rect5, "Asimov.DeleteAutoBillTip".Translate());

            // Info - Recipe Info
            Rect rectRepeat = new Rect(4, 28f, 160f, 24f);
            //Widgets.Label(rectRepeat, this.SelTable.GetComp<Comp_AutomatedProducer>().RepeatString());
            Widgets.Label(rectRepeat, Utility_AutoProducerCard.RepeatInfoString(comp));
            if (recipe != null)
            {
                foreach (ThingDefCount thing in recipe.products)
                {
                    producedItems = producedItems + "\n" + thing.Count.ToString() + " " + thing.ThingDef.LabelCap;
                }
                producingItem = recipe.productionString + producedItems;
                if (recipe.costList != null)
                {
                    foreach (ThingDefCount thing in recipe.costList)
                    {
                        inputListing = inputListing + "\n" + thing.Count.ToString() + " " + thing.ThingDef.LabelCap;
                    }
                }
                producingDescription = recipe.description;
                itemTexture = ContentFinder<Texture2D>.Get("UI/Toolbox/UnknownItem", true);
                if (recipe.recipeIcon != null)
                {
                    itemTexture = ContentFinder<Texture2D>.Get(recipe.recipeIcon.ToString(), true);
                }
                itemInfo = producingDescription + "\n\n" + producingItem + "\n\n" + comp.CurrentStatusLabel();
            }
            Rect rect3 = new Rect(0f, 45f, rect.width, 260f);
            GUI.BeginGroup(rect3);
            Rect rectInfo = new Rect(136f, 4f, rect3.width - 128f, 260f);
            Widgets.Label(rectInfo, itemInfo);
            Rect rect4 = new Rect(4f, 4f, 128f, 128f);
            GUI.DrawTexture(rect4, itemTexture);
            GUI.EndGroup();
            GUI.EndGroup();
        }
    }
}
