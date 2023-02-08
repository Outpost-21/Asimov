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

        public static AcceptanceReport CanDraftAutomaton(Pawn pawn)
        {
            if (pawn.Faction != null && pawn.Faction.IsPlayer && pawn.drafter != null)
            {
                //Need_Energy need = pawn.needs.TryGetNeed<Need_Energy>();
                //if (need != null && need.EmergencyPower)
                //{
                //    return "Asimov.CannotDraftEmergencyPower".Translate(pawn.Named("PAWN"));
                //}
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
