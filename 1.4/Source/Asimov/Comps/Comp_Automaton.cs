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

        public Color? skinFirst;
        public Color? skinSecond;

        public bool resolved;

        public List<WorkTypeDef> enabledWorkTypes = new List<WorkTypeDef>();
        public List<WorkTypeDef> cachedDisabledWorkTypes = new List<WorkTypeDef>();

        public List<WorkTypeDef> DisabledWorkTypes
        {
            get
            {
                if (cachedDisabledWorkTypes.NullOrEmpty())
                {
                    cachedDisabledWorkTypes = new List<WorkTypeDef>();
                    foreach (WorkTypeDef workType in DefDatabase<WorkTypeDef>.AllDefs)
                    {
                        if (!enabledWorkTypes.Contains(workType) && (bool)!Props.enabledWorkTypes?.Contains(workType) && !cachedDisabledWorkTypes.Contains(workType))
                        {
                            cachedDisabledWorkTypes.Add(workType);
                        }
                    }
                }
                return cachedDisabledWorkTypes;
            }
        }

        public void ResolveGraphics()
        {
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref skinFirst, "skinFirst");
            Scribe_Values.Look(ref skinSecond, "skinSecond");
            Scribe_Values.Look(ref resolved, "resolved");
            Scribe_Collections.Look(ref enabledWorkTypes, "enabledWorkTypes");
        }
    }
}
