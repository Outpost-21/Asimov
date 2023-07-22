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
    public static class AutomatonUtil
    {
        public static bool IsAutomaton(this Pawn pawn)
        {
            if(pawn as Automaton != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsHumanlikeAutomaton(this Pawn pawn)
        {
            if(pawn.def as PawnDef != null)
            {
                return true;
            }
            return false;
        }

        public static AcceptanceReport CanDraftAutomaton(Pawn pawn)
        {
            if (pawn.Faction != null && pawn.Faction.IsPlayer && pawn.drafter != null)
            {
                if (pawn.Downed)
                {
                    return "Asimov.CannotDraftDownedAutomaton".Translate(pawn.Named("PAWN"));
                }
                return true;
            }
            return false;
        }
    }
}
