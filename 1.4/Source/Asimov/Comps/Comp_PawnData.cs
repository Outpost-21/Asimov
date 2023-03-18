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
    public class Comp_PawnData : ThingComp
    {
        public CompProperties_PawnData Props => (CompProperties_PawnData)props;

        public Color? skinFirst;
        public Color? skinSecond;
    }
}
