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

        public WorldComp_EnergyNeed(World world) : base(world)
        {

        }

        public void AddHibernationSpot(Thing building)
        {
            hibernationSpots.Add(building);
        }

        public void RemoveHibernationSpot(Thing building)
        {
            hibernationSpots.Remove(building);
        }

        public void AddSocketCharger(Thing building)
        {
            chargingSockets.Add(building);
        }

        public void RemoveSocketCharger(Thing building)
        {
            chargingSockets.Remove(building);
        }

        public void AddWirelessCharger(Thing building, bool global)
        {
            wirelessChargers.Add(building);
            if (global)
            {
                wirelessChargersGlobal.Add(building);
            }
        }

        public void RemoveWirelessCharger(Thing building, bool global)
        {
            wirelessChargers.Remove(building);
            if (global)
            {
                wirelessChargersGlobal.Remove(building);
            }
        }
    }
}
