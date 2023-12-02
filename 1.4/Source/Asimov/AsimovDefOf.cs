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

        public static JobDef Asimov_Hibernate;
        public static JobDef Asimov_HibernateTillRepaired;
        public static JobDef Asimov_RepairAutomaton;
        public static JobDef Asimov_ConsumeEnergySource;
        public static JobDef Asimov_RechargeFromSocket;
        public static JobDef Asimov_InsertChargepacks;
        public static JobDef Asimov_RemoveChargepacks;
        //public static JobDef Asimov_ChargeOther;

        public static HediffDef Asimov_EmergencyPower;

        public static StatDef Asimov_EnergyMultiplier;

        public static FleshTypeDef Asimov_Automaton;

        public static ThingDef Asimov_HibernationSpot;
        public static ThingDef Asimov_ChargePad;
        public static ThingDef Asimov_WirelessCharger;
        public static ThingDef Asimov_LongRangeWirelessCharger;


        public static ThingDef Asimov_Chargepack;
        public static ThingDef Asimov_Chargepack_Empty;

        public static RecipeDef Asimov_RechargeChargepack;
        public static RecipeDef Asimov_RechargeChargepackBulk;

        // Vanilla

        public static ThingDef FabricationBench;

        public static ResearchProjectDef Electricity;
    }
}
