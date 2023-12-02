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
    public class AsimovSettings : ModSettings
    {
        public bool verboseLogging = true;

        public float energyDesperate = 0.25f;

        public float energyNormal = 0.5f;

        public float energyDrainMultiplier = 1.0f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energyDesperate, "hungerDesperate");
            Scribe_Values.Look(ref energyNormal, "energyNormal");
            Scribe_Values.Look(ref energyDrainMultiplier, "energyDrainMultiplier");
        }

        public bool IsValidSetting(string input)
        {
            if (GetType().GetFields().Where(p => p.FieldType == typeof(bool)).Any(i => i.Name == input))
            {
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetEnabledSettings
        {
            get
            {
                return GetType().GetFields().Where(p => p.FieldType == typeof(bool) && (bool)p.GetValue(this)).Select(p => p.Name);
            }
        }
    }
}
