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
    public class RecipeProps
    {
        public List<ThingDefCountClass> costList = new List<ThingDefCountClass>();

        public int workToMake = 0;

        public List<ResearchProjectDef> researchPrerequisites = new List<ResearchProjectDef>();

        public List<MemeDef> memePrerequisites = new List<MemeDef>();
    }
}
