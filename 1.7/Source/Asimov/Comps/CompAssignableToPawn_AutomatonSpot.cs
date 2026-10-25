using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Asimov
{
    public class CompAssignableToPawn_AutomatonSpot : CompAssignableToPawn
    {
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                return from p in parent.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.IsAutomaton() || p.IsHumanlikeAutomaton()
                       orderby p.kindDef.label, p.Label
                       select p;
            }
        }

        public override string GetAssignmentGizmoDesc()
        {
            return "Asimov.CommandAutomatonSpotOwnerDesc".Translate();
        }

        public override bool AssignedAnything(Pawn pawn)
        {
            return pawn.AssignedAutomatonSpot() != null;
        }

        public override void TryAssignPawn(Pawn pawn)
        {
            pawn.AssignedAutomatonSpot(parent);
        }

        public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
        {
            pawn.UnclaimAutomationSpot();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            if (Scribe.mode == LoadSaveMode.PostLoadInit && assignedPawns.RemoveAll((Pawn x) => x.AssignedAutomatonSpot() != parent) > 0)
            {
                Log.Warning(parent.ToStringSafe() + " had pawns assigned that don't have it as an assigned automaton spot. Removing.");
            }
        }
    }
}
