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

        public static PawnTableDef Asimov_Automatons;

        public static NeedDef Asimov_EnergyNeed;

        public static JobDef Asimov_Hibernate;
        public static JobDef Asimov_ConsumeEnergySource;
        public static JobDef Asimov_RechargeFromSocket;

        public static HediffDef Asimov_EmergencyPower;

        public static StatDef Asimov_EnergyMultiplier;

        public static FleshTypeDef Asimov_Automaton;

        public static ThingDef Asimov_HibernationSpot;
        public static ThingDef Asimov_ChargePad;

        public static ThingDef Asimov_Chargepack;
        public static ThingDef Asimov_Chargepack_Empty;
    }
}
