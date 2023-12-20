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
    public class CompProperties_AutoCrafterOverlay : CompProperties
    {
        public CompProperties_AutoCrafterOverlay()
        {
            compClass = typeof(Comp_AutoCrafterOverlay);
        }

        public List<ProgressState> progressStates = new List<ProgressState>();

        public List<RecipeState> recipeStates = new List<RecipeState>();
    }

    public class ProgressState
    {
        public string texPath;

        public float progress;

        public bool flipForWest = true;
    }

    public class RecipeState
    {
        public string recipeDef;

        public List<ProgressState> states = new List<ProgressState>();
    }
}
