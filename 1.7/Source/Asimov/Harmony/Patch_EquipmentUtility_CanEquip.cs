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
    [HarmonyPatch]
    public static class Patch_EquipmentUtility_CanEquip
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() => AccessTools.Method(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new[] { typeof(Thing), typeof(Pawn), typeof(string).MakeByRefType(), typeof(bool) });

        [HarmonyPostfix]
        public static void Postfix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            if (__result)
            {
                if (thing.def.IsApparel)
                {
                    // If apparel is restricted, check restriction.
                    if (AsimovStartup.apparelRaceRestrictions.ContainsKey(thing.def))
                    {
                        // Race cannot use.
                        if (!AsimovStartup.apparelRaceRestrictions[thing.def].Contains(pawn.def.defName))
                        {
                            __result = false;
                            cantReason = "Asimov.RaceCannotWear".Translate();
                            return;
                        }
                    }
                    PawnDef pawnDef = pawn.def as PawnDef;
                    if(pawnDef != null)
                    {
                        if (pawnDef.pawnSettings.onlyRestrictedApparel)
                        {

                        }
                    }
                }
            }
        }
    }
}
