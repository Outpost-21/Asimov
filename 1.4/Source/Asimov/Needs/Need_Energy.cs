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
    public class Need_Energy : Need
    {
        private int lastNonEmergencyTick = -99999;

        public EnergyCategory CurCategory
        {
            get
            {
                if (CurLevelPercentage <= 0f)
                {
                    return EnergyCategory.EmergencyPower;
                }
                if (CurLevelPercentage < AsimovMod.settings.energyDesperate)
                {
                    return EnergyCategory.Desperate;
                }
                if (CurLevelPercentage < AsimovMod.settings.energyNormal)
                {
                    return EnergyCategory.GettingLow;
                }
                return EnergyCategory.Full;
            }
        }

        public bool EmergencyPower => CurCategory == EnergyCategory.EmergencyPower;

        public override int GUIChangeArrow => -1;

        public override bool ShowOnNeedList => !Disabled;

        public bool Disabled
        {
            get
            {
                if (!pawn.Dead && pawn.Spawned && (pawn.Faction?.IsPlayer ?? false || pawn.IsPrisonerOfColony))
                {
                    return !pawn.def.HasModExtension<DefModExt_EnergyNeed>();
                }
                return true;
            }
        }

        public float EnergyRate
        {
            get
            {
                float energyRate = 1f;
                energyRate *= pawn.GetStatValue(AsimovDefOf.Asimov_EnergyMultiplier);
                return energyRate;
            }
        }

        public Need_Energy(Pawn pawn) : base(pawn)
        {

        }

        public override void SetInitialLevel()
        {
            CurLevel = 1f;
        }

        public override void NeedInterval()
        {
            if (Disabled)
            {
                CurLevel = 1f;
                return;
            }
            if (!EmergencyPower)
            {
                lastNonEmergencyTick = Find.TickManager.TicksGame;
            }
            if (!IsFrozen)
            {
                CurLevel -= GetPawnEnergyConsumption() * 200f;

                if (EmergencyPower)
                {
                    HealthUtility.AdjustSeverity(pawn, AsimovDefOf.Asimov_EmergencyPower, 0.0113333331f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(pawn.thingIDNumber ^ 0x26EF7A)));
                }
                else
                {
                    HealthUtility.AdjustSeverity(pawn, AsimovDefOf.Asimov_EmergencyPower, 0f - (0.0113333331f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(pawn.thingIDNumber ^ 0x26EF7A))));
                }
            }
        }

        public float GetPawnEnergyConsumption()
        {
            DefModExt_EnergyNeed modExt = pawn.def.GetModExtension<DefModExt_EnergyNeed>();
            return 2.66666666E-05f * EnergyRate;
        }
    }
}
