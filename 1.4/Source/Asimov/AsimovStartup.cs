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
    [StaticConstructorOnStartup]
    public static class AsimovStartup
    {
        public static Dictionary<TraitDef, List<ThingDef>> traitRaceRestrictions = new Dictionary<TraitDef, List<ThingDef>>();

        static AsimovStartup()
        {
            CheckIfBuildingsNeeded();
            CatalogRestrictions();
        }

        public static void CatalogRestrictions()
        {
            foreach(PawnDef def in DefDatabase<PawnDef>.AllDefs)
            {
                // Trait Restrictions
                if (!def.pawnSettings.traits.NullOrEmpty())
                {
                    foreach(TraitDef trait in def.pawnSettings.traits)
                    {
                        if (!traitRaceRestrictions.ContainsKey(trait))
                        {
                            traitRaceRestrictions.Add(trait, new List<ThingDef>() { def });
                        }
                        else
                        {
                            traitRaceRestrictions[trait].Add(def);
                        }
                    }
                }
            }
        }

        public static void CheckIfBuildingsNeeded()
        {
            bool anyNeedHibernationSpots = false;
            bool anyNeedSockets = false;
            bool anyNeedWireless = false;
            bool anyNeedChargepacks = false;
            foreach(ThingDef thing in DefDatabase<ThingDef>.AllDefs)
            {
                DefModExt_EnergyNeed modExt = thing.GetModExtension<DefModExt_EnergyNeed>();
                if(modExt != null)
                {
                    if (modExt.canChargeFromChargepacks) { anyNeedChargepacks = true; } 
                    if (modExt.canChargeFromSocket) { anyNeedSockets = true; }
                    if (modExt.canChargeWirelessly) { anyNeedWireless = true; }
                }
                if (thing.HasComp(typeof(Comp_Hibernation)))
                {
                    anyNeedHibernationSpots = true;
                }
            }
            if (!anyNeedHibernationSpots)
            {
                AsimovDefOf.Asimov_HibernationSpot.designationCategory = null;
            }
            if (!anyNeedSockets)
            {
                AsimovDefOf.Asimov_ChargePad.designationCategory = null;
            }
            if (!anyNeedWireless)
            {
                AsimovDefOf.Asimov_WirelessCharger.designationCategory = null;
                AsimovDefOf.Asimov_LongRangeWirelessCharger.designationCategory = null;
            }
            if (!anyNeedChargepacks)
            {
                AsimovDefOf.Asimov_Chargepack.recipeMaker.recipeUsers.Clear();
            }
        }
    }
}
