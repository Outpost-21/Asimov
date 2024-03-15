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
    public class DefModExt_EnergyNeed : DefModExtension
    {
        public bool canChargeFromChargepacks = true;
        public bool canChargeWirelessly = true;
        public bool canChargeFromSocket = true;
    }
}
