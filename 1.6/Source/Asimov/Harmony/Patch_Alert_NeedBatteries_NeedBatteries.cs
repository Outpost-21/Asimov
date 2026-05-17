using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace Asimov
{
    [HarmonyPatch(typeof(Alert_NeedBatteries), "NeedBatteries")]
    public static class Patch_EquipmentUtility_CanEquip
    {
        [HarmonyPostfix]
        public static void Postfix(Map map, bool __result)
        {
            if (__result) { return; }
            if (map.listerBuildings.ColonistsHaveBuilding((Thing building) => building))
        }
    }
}
