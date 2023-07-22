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
    public class Comp_Hibernation : ThingComp
    {
        public CompProperties_Hibernation Props => (CompProperties_Hibernation)props;

        Pawn pawn => parent as Pawn;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            yield return new Command_Action()
            {
                defaultLabel = "Asimov.GoHibernateLabel".Translate(),
                defaultDesc = "Asimov.GoHibernateDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("Asimov/UI/Hibernate"),
                action = delegate
                {
                    Thing hibernationSpot = EnergyUtil.GetClosestUnreservedHibernationSpot(pawn);
                    if(hibernationSpot == null)
                    {
                        Messages.Message("Asimov.NoHibernationSpot".Translate(), MessageTypeDefOf.NegativeEvent);
                    }
                    pawn.jobs.TryTakeOrderedJob(new Job(AsimovDefOf.Asimov_Hibernate, hibernationSpot), JobTag.Misc);
                }
            };
        }
    }
}
