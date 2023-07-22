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
    public class CompProperties_RecolourablePawn : CompProperties
    {
        public bool channelOne = false;

        public bool channelTwo = false;

        public CompProperties_RecolourablePawn()
        {
            compClass = typeof(Comp_RecolourablePawn);
        }
    }
}
