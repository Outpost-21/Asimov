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
    public static class RestrictionUtil
    {
        public static bool CanHaveGene(ThingDef pawnDef, GeneDef geneDef)
        {
            if(geneDef.defName.StartsWith("Skin_Melanin"))
            {
                // Force allow skin colour, freaks out if no skin colour is applied.
                return true;
            }
            if (pawnDef as PawnDef != null)
            {
                return false;
            }
            return true;
        }
    }
}
