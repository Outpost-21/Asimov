using RimWorld;
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
    public class ThinkNode_ConditionalAutomaton : ThinkNode_Conditional
    {
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalAutomaton obj = (ThinkNode_ConditionalAutomaton)base.DeepCopy(resolve);
            return obj;
        }

        public override bool Satisfied(Pawn pawn)
        {
            return pawn as Automaton != null;
        }
    }
}
