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
    public class CompProperties_EnergyProvider : CompProperties
    {
        public float drainToRefill = 1000f;

        public float rechargeRate = 0.01f;

        

        public CompProperties_EnergyProvider()
        {
            compClass = typeof(Comp_EnergyProvider);
        }
    }
}
