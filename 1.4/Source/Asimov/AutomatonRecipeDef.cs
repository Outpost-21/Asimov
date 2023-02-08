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
    public class AutomatonRecipeDef : RecipeDef
    {
        public PawnKindDef pawnKind;

        public List<ThingDefCountClass> costList = new List<ThingDefCountClass>();

        public int workToMake = 0;

        public List<MemeDef> memePrerequisites = new List<MemeDef>();

        public string recipeIcon = null;

        public string productionString = "Fabricating: ";
    }
}
