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
    public class PawnDef : ThingDef
    {
        public CustomGraphics customGraphics = new CustomGraphics();
        public PawnSettings pawnSettings = new PawnSettings();
    }
}
