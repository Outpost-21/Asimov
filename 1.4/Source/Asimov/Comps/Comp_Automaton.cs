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
    public class Comp_Automaton : ThingComp
    {
        public CompProperties_Automaton Props => (CompProperties_Automaton)props;
    }
}
