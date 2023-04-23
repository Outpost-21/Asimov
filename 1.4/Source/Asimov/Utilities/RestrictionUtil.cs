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
        public static List<GeneDef> restrictedGenes = new List<GeneDef>();

        public static bool CanHaveGene(ThingDef pawnDef, GeneDef geneDef)
        {
            if(geneDef.defName.StartsWith("Skin_Melanin"))
            {
                // Force allow skin colour, freaks out if no skin colour is applied.
                return true;
            }
            PawnSettings pawnSettings = (pawnDef as PawnDef)?.pawnSettings;
            if (pawnSettings != null)
            {
                if (pawnSettings.blockGeneMechanics)
                {
                    return false;
                }
                if (pawnSettings.geneBlacklist.Contains(geneDef))
                {
                    return false;
                }
                if (pawnSettings.restrictRaceGenes && !pawnSettings.geneWhitelist.Contains(geneDef))
                {
                    return false;
                }
                if (restrictedGenes.Contains(geneDef) && !pawnSettings.geneWhitelist.Contains(geneDef))
                {
                    return false;
                }
            }
            else if (restrictedGenes.Contains(geneDef))
            {
                return false;
            }
            return true;
        }
    }
}
