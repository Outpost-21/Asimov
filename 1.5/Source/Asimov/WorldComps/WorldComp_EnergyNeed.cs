using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld.Planet;

namespace Asimov
{
    public class WorldComp_EnergyNeed : WorldComponent
    {
        public List<Thing> hibernationSpots = new List<Thing>();

        public List<Thing> chargingSockets = new List<Thing>();

        public List<Thing> wirelessChargers = new List<Thing>();

        public List<Thing> wirelessChargersGlobal = new List<Thing>();

        public Dictionary<Map, HashSet<Thing>> chargepackChargers = new Dictionary<Map, HashSet<Thing>>();

        public WorldComp_EnergyNeed(World world) : base(world)
        {

        }

        public void AddChargepackCharger(Thing building, Map map)
        {
            if (!chargepackChargers.ContainsKey(map))
            {
                chargepackChargers.Add(map, new HashSet<Thing>());
                chargepackChargers[map].Add(building);
            }
            else if (!chargepackChargers[map].Contains(building))
            {
                chargepackChargers[map].Add(building);
            }
        }

        public void RemoveChargepackCharger(Thing building, Map map)
        {
            if (chargepackChargers.ContainsKey(map))
            {
                if (chargepackChargers[map].Contains(building))
                {
                    chargepackChargers[map].Remove(building);
                }
            }
        }

        public void AddHibernationSpot(Thing building)
        {
            if (!hibernationSpots.Contains(building))
            {
                hibernationSpots.Add(building);
            }
        }

        public void RemoveHibernationSpot(Thing building)
        {
            if (hibernationSpots.Contains(building))
            {
                hibernationSpots.Remove(building);
            }
        }

        public void AddSocketCharger(Thing building)
        {
            if (!chargingSockets.Contains(building))
            {
                chargingSockets.Add(building);
            }
        }

        public void RemoveSocketCharger(Thing building)
        {
            if (chargingSockets.Contains(building))
            {
                chargingSockets.Remove(building);
            }
        }

        public void AddWirelessCharger(Thing building, bool global)
        {
            if (!wirelessChargers.Contains(building))
            {
                wirelessChargers.Add(building);
            }
            if (global)
            {
                if (!wirelessChargersGlobal.Contains(building))
                {
                    wirelessChargersGlobal.Add(building);
                }
            }
        }

        public void RemoveWirelessCharger(Thing building, bool global)
        {
            if (wirelessChargers.Contains(building))
            {
                wirelessChargers.Remove(building);
            }
            if (global)
            {
                if (wirelessChargersGlobal.Contains(building))
                {
                    wirelessChargersGlobal.Remove(building);
                }
            }
        }
    }
}
