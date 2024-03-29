﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class CompProperties_AutoCrafter : CompProperties
    {
        public CompProperties_AutoCrafter()
        {
            compClass = typeof(Comp_AutoCrafter);
        }

        public float craftingTimeMulti = 1.0f;

        public List<AutomatonRecipeDef> recipes = new List<AutomatonRecipeDef>();
    }
}
