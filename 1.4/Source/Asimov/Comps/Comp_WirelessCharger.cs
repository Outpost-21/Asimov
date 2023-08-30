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

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            EnergyUtil.GetEnergyNeedWorldComp.RemoveWirelessCharger(parent, Props.worldWide);
        }

        public float RechargePawn(Pawn pawn, float reqEnergy)
        {
            if(powerComp == null)
            {
                return reqEnergy;
            }
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
    }
}
