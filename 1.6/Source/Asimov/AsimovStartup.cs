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
        public static Dictionary<TraitDef, List<string>> traitRaceWhitelist = new Dictionary<TraitDef, List<string>>();

        public static Dictionary<ThingDef, List<string>> apparelRaceRestrictions = new Dictionary<ThingDef, List<string>>();
        public static Dictionary<ThingDef, List<string>> apparelRaceWhitelist = new Dictionary<ThingDef, List<string>>();

        public static Dictionary<ThingDef, List<string>> weaponRaceRestrictions = new Dictionary<ThingDef, List<string>>();
        public static Dictionary<ThingDef, List<string>> weaponRaceWhitelist = new Dictionary<ThingDef, List<string>>();


        public static bool buildingHideFlag_HibernationSpot = true;
        public static bool buildingHideFlag_Chargepacks = true;
        public static bool buildingHideFlag_Chargepad = true;
        public static bool buildingHideFlag_wirelessCharging = true;

        public static bool researchHideFlag_wirelessCharging = true;

        static AsimovStartup()
        {
            CheckIfBuildingsNeeded();
            CatalogRestrictions();
            DisableCorpseRottingAndEdibility();
        }

        public static void DisableCorpseRottingAndEdibility()
        {
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                CompProperties_Automaton comp = thingDef.GetCompProperties<CompProperties_Automaton>();
                if (comp != null)
                {
                    ThingDef corpseDef = thingDef?.race?.corpseDef;
                    if (corpseDef != null)
                    {
                        if (!comp.corpseRots)
                        {
                            corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_Rottable);
                            corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_SpawnerFilth);
                        }
                        if (!comp.corpseEdible)
                        {
                            if (corpseDef.modExtensions.NullOrEmpty())
                            {
                                corpseDef.modExtensions = new List<DefModExtension>();
                            }
                            corpseDef.modExtensions.Add(new DefModExt_NonIngestible());
                        }
                    }
                }
            }
        }

        public static void CatalogRestrictions()
        {
            CatalogRestrictions_Traits();
            CatalogRestrictions_Apparel();
            CatalogRestrictions_Weapons();
        }

        public static void CatalogRestrictions_Weapons()
        {
            foreach (PawnDef def in DefDatabase<PawnDef>.AllDefs)
            {
                if (!def.pawnSettings.weapons.NullOrEmpty())
                {
                    foreach (ThingDef weapon in def.pawnSettings.weapons)
                    {
                        // Restrictions
                        if (!weaponRaceRestrictions.ContainsKey(weapon))
                        {
                            weaponRaceRestrictions.Add(weapon, new List<string>() { def.defName });
                        }
                        else
                        {
                            weaponRaceRestrictions[weapon].Add(def.defName);
                        }
                    }
                }
                if (!def.pawnSettings.weaponsWhitelist.NullOrEmpty())
                {
                    foreach (ThingDef weapon in def.pawnSettings.weaponsWhitelist)
                    {
                        // Whitelist
                        if (!apparelRaceWhitelist.ContainsKey(weapon))
                        {
                            apparelRaceWhitelist.Add(weapon, new List<string>() { def.defName });
                        }
                        else
                        {
                            apparelRaceWhitelist[weapon].Add(def.defName);
                        }
                    }
                }
            }
            bool logRestrictions = false;
            if (logRestrictions)
            {
                foreach (KeyValuePair<TraitDef, List<string>> kvp in traitRaceRestrictions)
                {
                    string apparelMsg = "Apparel Restricted: " + kvp.Key.defName;
                    foreach (string s in kvp.Value)
                    {
                        apparelMsg += "\n- " + s;
                    }
                    LogUtil.LogDebug(apparelMsg);
                }
            }
        }

        public static void CatalogRestrictions_Apparel()
        {
            foreach (PawnDef def in DefDatabase<PawnDef>.AllDefs)
            {
                if (!def.pawnSettings.apparel.NullOrEmpty())
                {
                    foreach (ThingDef apparel in def.pawnSettings.apparel)
                    {
                        // Restrictions
                        if (!apparelRaceRestrictions.ContainsKey(apparel))
                        {
                            apparelRaceRestrictions.Add(apparel, new List<string>() { def.defName });
                        }
                        else
                        {
                            apparelRaceRestrictions[apparel].Add(def.defName);
                        }
                    }
                }
                if (!def.pawnSettings.apparelWhitelist.NullOrEmpty())
                {
                    foreach (ThingDef apparel in def.pawnSettings.apparelWhitelist)
                    {
                        // Whitelist
                        if (!apparelRaceWhitelist.ContainsKey(apparel))
                        {
                            apparelRaceWhitelist.Add(apparel, new List<string>() { def.defName });
                        }
                        else
                        {
                            apparelRaceWhitelist[apparel].Add(def.defName);
                        }
                    }
                }
            }
            bool logRestrictions = false;
            if (logRestrictions)
            {
                foreach (KeyValuePair<TraitDef, List<string>> kvp in traitRaceRestrictions)
                {
                    string apparelMsg = "Apparel Restricted: " + kvp.Key.defName;
                    foreach (string s in kvp.Value)
                    {
                        apparelMsg += "\n- " + s;
                    }
                    LogUtil.LogDebug(apparelMsg);
                }
            }
        }

        public static void CatalogRestrictions_Traits()
        {
            foreach (PawnDef def in DefDatabase<PawnDef>.AllDefs)
            {
                if (!def.pawnSettings.traits.NullOrEmpty())
                {
                    foreach (TraitDef trait in def.pawnSettings.traits)
                    {
                        // Restrictions
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
                if (!def.pawnSettings.traitsWhitelist.NullOrEmpty())
                {
                    foreach(TraitDef trait in def.pawnSettings.traitsWhitelist)
                    {
                        // Whitelist
                        if (!traitRaceWhitelist.ContainsKey(trait))
                        {
                            traitRaceWhitelist.Add(trait, new List<string>() { def.defName });
                        }
                        else
                        {
                            traitRaceWhitelist[trait].Add(def.defName);
                        }
                    }
                }
            }
            bool logRestrictions = false;
            if (logRestrictions)
            {
                foreach (KeyValuePair<TraitDef, List<string>> kvp in traitRaceRestrictions)
                {
                    string traitMsg = "Trait Restricted: " + kvp.Key.defName;
                    foreach (string s in kvp.Value)
                    {
                        traitMsg += "\n- " + s;
                    }
                    LogUtil.LogDebug(traitMsg);
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
            if (anyNeedHibernationSpots || AsimovMod.settings.forceShowEnergyStuff)
            {
                buildingHideFlag_HibernationSpot = false;
            }
            if (anyNeedSockets || AsimovMod.settings.forceShowEnergyStuff)
            {
                buildingHideFlag_Chargepad = false;
                AsimovDefOf.Asimov_ChargePad.researchPrerequisites.Add(AsimovDefOf.Electricity);
            }
            if (anyNeedWireless || AsimovMod.settings.forceShowEnergyStuff)
            {
				buildingHideFlag_wirelessCharging = false;
                researchHideFlag_wirelessCharging = false;
            }
            if (anyNeedChargepacks || AsimovMod.settings.forceShowEnergyStuff)
            {
                buildingHideFlag_Chargepacks = false;
				AsimovDefOf.TableMachining.recipes.Add(AsimovDefOf.Asimov_CraftChargepacks);
                AsimovDefOf.TableMachining.recipes.Add(AsimovDefOf.Asimov_RechargeChargepack);
                AsimovDefOf.TableMachining.recipes.Add(AsimovDefOf.Asimov_RechargeChargepackBulk);
				AsimovDefOf.FabricationBench.recipes.Add(AsimovDefOf.Asimov_CraftChargepacks);
                AsimovDefOf.FabricationBench.recipes.Add(AsimovDefOf.Asimov_RechargeChargepack);
                AsimovDefOf.FabricationBench.recipes.Add(AsimovDefOf.Asimov_RechargeChargepackBulk);
            }
        }
    }
}
