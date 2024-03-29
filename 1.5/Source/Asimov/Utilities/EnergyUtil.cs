﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Asimov
{
    public static class EnergyUtil
    {
        public static WorldComp_EnergyNeed GetEnergyNeedWorldComp
        {
            get
            {
                WorldComp_EnergyNeed comp = Find.World.GetComponent(typeof(WorldComp_EnergyNeed)) as WorldComp_EnergyNeed;
                if (comp != null)
                {
                    return comp;
                }
                else
                {
                    LogUtil.LogError("Could not find WorldComponent_EnergyNeed.");
                }
                return null;
            }
        }

        public static bool NeedsChargepack(Pawn p)
        {
            if (p.needs != null)
            {
                Need_Energy need = (Need_Energy)p.needs.TryGetNeed(AsimovDefOf.Asimov_EnergyNeed);
                if (need != null)
                {
                    return need.CurLevelPercentage <= AsimovMod.settings.energyDesperate + 0.02f;
                }
            }
            return false;
        }

        public static List<Thing> GetChargepackChargersOnMap(Map map)
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            if (comp.chargepackChargers.ContainsKey(map))
            {
                return comp.chargepackChargers[map].ToList();
            }
            return new List<Thing>();
        }

        public static float PullEnergyFromChargers(this List<Thing> chargers, Pawn pawn, float reqEnergy)
        {
            float remaining = reqEnergy;

            foreach (Thing thing in chargers)
            {
                if(remaining <= 0f)
                {
                    return 0f;
                }
                Comp_WirelessCharger chargeComp = thing.TryGetComp<Comp_WirelessCharger>();
                if(chargeComp != null)
                {
                    remaining -= chargeComp.RechargePawn(pawn, remaining);
                }
            }
            
            return remaining;
        }

        public static List<Thing> GetGlobalChargers()
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            return comp.wirelessChargersGlobal;
        }

        public static List<Thing> GetWirelessChargersInRange(this Pawn pawn)
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            List<Thing> result = new List<Thing>();
            if (pawn.Spawned && !comp.wirelessChargers.NullOrEmpty())
            {
                List<Thing> chargersOnMap = comp.wirelessChargers.Where(wc => wc.Map != null && wc.Map == pawn.Map).ToList();
                if (!chargersOnMap.NullOrEmpty()) 
                {
                    foreach(Thing charger in chargersOnMap)
                    {
                        if (pawn.Position.DistanceTo(charger.Position) <= charger.TryGetComp<Comp_WirelessCharger>()?.Props?.range)
                        {
                            result.Add(charger);
                        }
                    }
                }
            }
            return result;
        }

        public static bool InWirelessChargerRange(this Pawn pawn)
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            if (pawn.Spawned && !comp.wirelessChargers.NullOrEmpty())
            {
                List<Thing> chargersOnMap = comp.wirelessChargers.Where(wc => wc.Map != null && wc.Map == pawn.Map).ToList();
                if (!chargersOnMap.NullOrEmpty() && chargersOnMap.Any(wc => pawn.Position.DistanceTo(wc.Position) <= wc.def.specialDisplayRadius))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Thing> GetLocalChargingSockets(Pawn pawn)
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            if (pawn.Spawned && !comp.chargingSockets.NullOrEmpty())
            {
                return comp?.chargingSockets?.Where(wc => wc.Map != null && wc.Map == pawn.Map)?.ToList() ?? new List<Thing>();
            }
            return new List<Thing>();
        }

        public static Thing GetClosestPowerSocket(Pawn pawn)
        {
            Thing building = null;
            List<Thing> localSockets = GetLocalChargingSockets(pawn);
            if (!localSockets.NullOrEmpty())
            {
                for (int i = 0; i < localSockets.Count(); i++)
                {
                    Thing curr = localSockets[i];
                    if ((building == null || building.Position.DistanceTo(pawn.Position) > curr.Position.DistanceTo(pawn.Position)) && curr.EnergyProvider().CanRechargeTick)
                    {
                        if (curr.Position.Walkable(pawn.Map) && curr.Position.InAllowedArea(pawn) && pawn.CanReserve(curr) && pawn.CanReach(curr.Position, PathEndMode.OnCell, Danger.Deadly))
                        {
                            building = curr;
                            break;
                        }
                    }
                }
            }
            return building;
        }

        public static Comp_EnergyProvider EnergyProvider(this Thing building)
        {
            return building.TryGetComp<Comp_EnergyProvider>();
        }

        public static Thing GetClosestUnreservedHibernationSpot(Pawn pawn)
        {
            Thing building = null;
            List<Thing> localSpots = GetLocalHibernationSpots(pawn);
            if (!localSpots.NullOrEmpty())
            {
                for (int i = 0; i < localSpots.Count(); i++)
                {
                    Thing curr = localSpots[i];
                    if (building == null || building.Position.DistanceTo(pawn.Position) > curr.Position.DistanceTo(pawn.Position))
                    {
                        if (curr.Position.Walkable(pawn.Map) && curr.Position.InAllowedArea(pawn) && pawn.CanReserve(curr) && pawn.CanReach(curr.Position, PathEndMode.OnCell, Danger.Deadly))
                        {
                            building = curr;
                            break;
                        }
                    }
                }
            }
            return building;
        }

        public static List<Thing> GetLocalHibernationSpots(Pawn pawn)
        {
            WorldComp_EnergyNeed comp = GetEnergyNeedWorldComp;
            if (pawn.Spawned && !comp.hibernationSpots.NullOrEmpty())
            {
                return comp?.hibernationSpots?.Where(wc => wc.Map != null && wc.Map == pawn.Map)?.ToList() ?? new List<Thing>();
            }
            return new List<Thing>();
        }
    }
}
