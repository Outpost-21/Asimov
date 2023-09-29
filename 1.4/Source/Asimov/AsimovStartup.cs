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
        public static Dictionary<TraitDef, List<string>> traitRaceRestrictions = new Dictionary<TraitDef, List<string>>();

        public static bool buildingHideFlag_HibernationSpot = true;
        public static bool buildingHideFlag_Chargepad = true;
        public static bool buildingHideFlag_wirelessCharging = true;

        public static bool researchHideFlag_wirelessCharging = true;

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
                            traitRaceRestrictions.Add(trait, new List<string>() { def.defName });
                        }
                        else
                        {
                            traitRaceRestrictions[trait].Add(def.defName);
                        }
                    }
                }
            }
            bool logRestrictions = true;
            if (logRestrictions)
            {
                foreach(KeyValuePair<TraitDef, List<string>> kvp in traitRaceRestrictions)
                {
                    string traitMsg = "Trait Restricted: " + kvp.Key.defName;
                    foreach(string s in kvp.Value)
                    {
                        traitMsg += "\n- " + s;
                    }
                    LogUtil.LogMessage(traitMsg);
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
                    if (modExt.canChargeFromChargepacks) { anyNeedChargepacks = true; LogUtil.LogMessage("Found Chargepack Users..."); } 
                    if (modExt.canChargeFromSocket) { anyNeedSockets = true; LogUtil.LogMessage("Found Chargepad Users..."); }
                    if (modExt.canChargeWirelessly) { anyNeedWireless = true; LogUtil.LogMessage("Found Wireless Charger Users..."); }
                }
                if (thing.HasComp(typeof(Comp_Hibernation)))
                {
                    anyNeedHibernationSpots = true;
                    LogUtil.LogMessage("Found Hibernation Spot Users...");
                }
            }
            if (anyNeedHibernationSpots)
            {
                LogUtil.LogMessage("Showing Hibernation Spot");
                buildingHideFlag_HibernationSpot = false;
            }
            if (anyNeedSockets)
            {
                LogUtil.LogMessage("Showing Chargepad");
                buildingHideFlag_Chargepad = false;
                AsimovDefOf.Asimov_ChargePad.researchPrerequisites.Add(AsimovDefOf.Electricity);
            }
            if (anyNeedWireless)
            {
                LogUtil.LogMessage("Showing Wireless Chargers");
                researchHideFlag_wirelessCharging = false;
            }
            if (anyNeedChargepacks)
            {
                LogUtil.LogMessage("Showing Chargepacks");
                AsimovDefOf.FabricationBench.recipes.Add(AsimovDefOf.Asimov_RechargeChargepack);
                AsimovDefOf.FabricationBench.recipes.Add(AsimovDefOf.Asimov_RechargeChargepackBulk);
                AsimovDefOf.Asimov_Chargepack.recipeMaker.recipeUsers.Add(AsimovDefOf.FabricationBench);
            }
        }
    }
}
