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
    public class PlaceWorker_NeedChargepacks : PlaceWorker
    {
        public override bool IsBuildDesignatorVisible(BuildableDef def)
        {
            return !AsimovStartup.buildingHideFlag_Chargepacks;
        }
    }
}
