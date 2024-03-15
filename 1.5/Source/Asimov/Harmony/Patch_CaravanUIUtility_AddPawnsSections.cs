using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld.Planet;

namespace Asimov
{
    [HarmonyPatch(typeof(CaravanUIUtility), "AddPawnsSections")]
    public static class Patch_CaravanUIUtility_AddPawnsSections
    {
        [HarmonyPostfix]
        public static void Postfix(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
        {
            IEnumerable<TransferableOneWay> source = transferables.Where((TransferableOneWay x) => x.ThingDef.category == ThingCategory.Pawn);
            widget.AddSection("Asimov.Automatons".Translate(), source.Where((TransferableOneWay x) => ((Pawn)x.AnyThing).IsAutomaton()));
        }
    }
}
