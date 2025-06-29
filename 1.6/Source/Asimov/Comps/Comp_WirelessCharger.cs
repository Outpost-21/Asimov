﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class Comp_WirelessCharger : ThingComp
    {
        public CompProperties_WirelessCharger Props => (CompProperties_WirelessCharger)props;

        public CompPowerTrader powerComp;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if (powerComp == null)
            {
                powerComp = parent.TryGetComp<CompPowerTrader>();
            }

            EnergyUtil.GetEnergyNeedWorldComp.AddWirelessCharger(parent, Props.worldWide);
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);
            EnergyUtil.GetEnergyNeedWorldComp.RemoveWirelessCharger(parent, Props.worldWide);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            EnergyUtil.GetEnergyNeedWorldComp.RemoveWirelessCharger(parent, Props.worldWide);
        }

        public override void CompTick()
        {
            base.CompTick();
        }

        public float RechargePawn(Pawn pawn, float reqEnergy)
        {
            if(powerComp == null) { return reqEnergy; }
            if (!powerComp.PowerOn) { return reqEnergy; }
            Need_Energy energyNeed = (Need_Energy)pawn.needs.TryGetNeed(AsimovDefOf.Asimov_EnergyNeed);
            float changeVal = Mathf.Min(reqEnergy, powerComp.PowerNet.CurrentStoredEnergy());
            if (energyNeed != null)
            {
                if (changeVal > 0f)
                {
                    powerComp.PowerNet.ChangeStoredEnergy(-changeVal);
                    energyNeed.CurLevel += changeVal;
                }
            }
            return reqEnergy - changeVal;
        }

        public override string CompInspectStringExtra()
        {
            string inspect = base.CompInspectStringExtra() ?? "";
            if (powerComp?.PowerNet?.batteryComps?.NullOrEmpty() ?? true)
            {
                inspect += "Asimov.NoConnectedBatteries".Translate();
            }
            return inspect;
        }
    }
}
