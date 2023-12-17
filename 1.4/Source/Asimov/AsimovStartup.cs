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
            bool logRestrictions = false;
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
                    if (modExt.canChargeFromChargepacks) { anyNeedChargepacks = true; } 
                    if (modExt.canChargeFromSocket) { anyNeedSockets = true; }
                    if (modExt.canChargeWirelessly) { anyNeedWireless = true; }
                }
                if (thing.HasComp(typeof(Comp_Hibernation)))
                {
                    anyNeedHibernationSpots = true;
                }
            }
            if (anyNeedHibernationSpots)
            {
                buildingHideFlag_HibernationSpot = false;
            }
            if (anyNeedSockets)
            {
                buildingHideFlag_Chargepad = false;
                AsimovDefOf.Asimov_ChargePad.researchPrerequisites.Add(AsimovDefOf.Electricity);
            }
            if (anyNeedWireless)
            {
                researchHideFlag_wirelessCharging = false;
            }
            if (anyNeedChargepacks)
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
