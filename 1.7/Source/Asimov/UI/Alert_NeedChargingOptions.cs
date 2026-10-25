using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Asimov
{
    public class Alert_NeedChargingOptions : Alert
    {
        public Alert_NeedChargingOptions()
        {
            defaultLabel = "Asimov.NeedChargingOptions".Translate();
            defaultExplanation = "Asimov.NeedChargingOptionsExplanation".Translate();
            defaultPriority = AlertPriority.High;
        }

        public override AlertReport GetReport()
        {
            if (!AsimovMod.settings.chargingOptionsAlert) { return false; }
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                if (maps[i].IsPlayerHome && AnyEnergyAutomatons(maps[i]) && !AnyGlobalWireless() && !AnyChargepackChargers(maps[i]) && !AnyChargepads(maps[i]) && !AnyWireless(maps[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnyEnergyAutomatons(Map map)
        {
            IEnumerable<Pawn> automatons = from p in map.mapPawns.PawnsInFaction(Faction.OfPlayer) 
                                           where p.IsAutomaton() || p.IsHumanlikeAutomaton()
                                           orderby p.kindDef.label, p.Label
                                           select p;
            if (!automatons.EnumerableNullOrEmpty())
            {
                List<Pawn> automatonsForReading = automatons.ToList();
                for (int i = 0; automatonsForReading.Count() < i; i++)
                {
                    Need_Energy need = (Need_Energy)automatonsForReading[i].needs?.TryGetNeed(AsimovDefOf.Asimov_EnergyNeed);
                    if (need != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AnyChargepackChargers(Map map)
        {
            WorldComp_EnergyNeed comp = EnergyUtil.GetEnergyNeedWorldComp;
            if (comp.chargepackChargers.ContainsKey(map))
            {
                if (!comp.chargepackChargers[map].NullOrEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnyChargepads(Map map)
        {
            if (EnergyUtil.GetEnergyNeedWorldComp.chargingSockets.Any(cs => cs.Map == map))
            {
                return true;
            }
            return false;
        }

        public bool AnyWireless(Map map)
        {
            if (EnergyUtil.GetEnergyNeedWorldComp.wirelessChargers.Any(wc => wc.Map == map))
            {
                return true;
            }
            return false;
        }

        public bool AnyGlobalWireless()
        {
            if (!EnergyUtil.GetEnergyNeedWorldComp.wirelessChargersGlobal.NullOrEmpty())
            {
                return true;
            }
            return false;
        }
    }
}
