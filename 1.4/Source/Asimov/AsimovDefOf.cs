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
    [DefOf]
    public static class AsimovDefOf
    {
        static AsimovDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AsimovDefOf));
        }

        public static ResearchProjectDef Asimov_WirelessCharging;

        public static PawnTableDef Asimov_Automatons;

        public static NeedDef Asimov_EnergyNeed;

        public static JobDef Asimov_Hibernate, Asimov_HibernateTillRepaired;
        public static JobDef Asimov_RepairAutomaton, Asimov_ConsumeEnergySource, Asimov_RechargeFromSocket, Asimov_InsertChargepacks, Asimov_RemoveChargepacks;
        //public static JobDef Asimov_ChargeOther;

        public static HediffDef Asimov_EmergencyPower;

        public static StatDef Asimov_EnergyMultiplier;

        public static FleshTypeDef Asimov_Automaton;

        public static ThingDef Asimov_HibernationSpot, Asimov_ChargePad, Asimov_WirelessCharger, Asimov_LongRangeWirelessCharger;


        public static ThingDef Asimov_Chargepack, Asimov_Chargepack_Empty;

        public static RecipeDef Asimov_CraftChargepacks, Asimov_RechargeChargepack, Asimov_RechargeChargepackBulk;

        // Vanilla

        public static WorkTypeDef BasicWorker, Cooking, Cleaning, Warden, Art, Tailoring;

        public static NeedDef DrugDesire;

        public static ThingDef FabricationBench, TableMachining;

        public static ResearchProjectDef Electricity;
    }
}
