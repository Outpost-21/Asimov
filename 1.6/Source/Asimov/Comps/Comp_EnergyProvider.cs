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

            EnergyUtil.GetEnergyNeedWorldComp.AddSocketCharger(parent);
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);
            EnergyUtil.GetEnergyNeedWorldComp.RemoveSocketCharger(parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            EnergyUtil.GetEnergyNeedWorldComp.RemoveHibernationSpot(parent);
        }

        public void RechargePawn(Pawn pawn)
        {
            Need_Energy energyNeed = (Need_Energy)pawn.needs.TryGetNeed(AsimovDefOf.Asimov_EnergyNeed);
            if(energyNeed != null)
            {
                if (energyNeed.CurLevelPercentage < 0.99f && CanRechargeTick)
                {
                    powerComp.PowerNet.ChangeStoredEnergy(-RechargeCostPerTick);
                    energyNeed.CurLevel += Props.rechargeRate;
                }
            }
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
