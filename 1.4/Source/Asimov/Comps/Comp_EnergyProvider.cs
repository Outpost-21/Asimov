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
    public class Comp_EnergyProvider : ThingComp
    {
        public CompProperties_EnergyProvider Props => (CompProperties_EnergyProvider)props;

        public CompPowerTrader powerComp;

        public float RechargeCostPerTick => Props.rechargeRate * Props.drainToRefill;

        public bool CanRechargeTick => powerComp.PowerNet.CurrentStoredEnergy() > RechargeCostPerTick;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if(powerComp == null)
            {
                powerComp = parent.TryGetComp<CompPowerTrader>();
            }

            Find.World.GetComponent<WorldComp_EnergyNeed>().AddSocketCharger(parent as Building);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            Find.World.GetComponent<WorldComp_EnergyNeed>().RemoveSocketCharger(parent as Building);
        }

        public void RechargePawn(Pawn pawn, float percentage)
        {
            Need_Energy energyNeed = (Need_Energy)pawn.needs.TryGetNeed(AsimovDefOf.Asimov_EnergyNeed);
            if(energyNeed != null)
            {
                if (CanRechargeTick)
                {
                    powerComp.PowerNet.DistributeEnergyAmongBatteries(-RechargeCostPerTick);
                    energyNeed.CurLevel += Props.rechargeRate;
                }
            }
        }
    }
}
