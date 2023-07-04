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
        public List<Building> chargingSockets = new List<Building>();

        public List<Building> wirelessChargers = new List<Building>();

        public List<Building> wirelessChargersGlobal = new List<Building>();

        public WorldComp_EnergyNeed(World world) : base(world)
        {

        }

        public void AddSocketCharger(Building building)
        {
            chargingSockets.Add(building);
        }

        public void RemoveSocketCharger(Building building)
        {
            chargingSockets.Remove(building);
        }

        public void AddWirelessCharger(Building building, bool global)
        {
            wirelessChargers.Add(building);
            if (global)
            {
                wirelessChargersGlobal.Add(building);
            }
        }

        public void RemoveWirelessCharger(Building building, bool global)
        {
            wirelessChargers.Remove(building);
            if (global)
            {
                wirelessChargersGlobal.Remove(building);
            }
        }
    }
}
