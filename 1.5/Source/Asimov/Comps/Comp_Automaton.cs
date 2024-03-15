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
        public Pawn pawn => parent as Pawn;

        public Color? skinFirst;
        public Color? skinSecond;

        public bool resolved;

        public List<WorkTypeDef> extraEnabledWorkTypes = new List<WorkTypeDef>();

        public List<WorkTypeDef> cachedEnabledWorkTypes;

        public List<WorkTypeDef> EnabledWorkTypes
        {
            get
            {
                if (cachedEnabledWorkTypes == null)
                {
                    cachedEnabledWorkTypes = new List<WorkTypeDef>();
                    foreach (WorkTypeDef workType in DefDatabase<WorkTypeDef>.AllDefs)
                    {
                        if ((Props.enableAllWorkTypes || extraEnabledWorkTypes.Contains(workType) || (!Props.enabledWorkTypes.NullOrEmpty() && Props.enabledWorkTypes.Contains(workType))) && !cachedEnabledWorkTypes.Contains(workType))
                        {
                            if (pawn.RaceProps.Humanlike)
                            {
                                pawn.WorkTypeIsDisabled(workType);
                                cachedEnabledWorkTypes.Add(workType);
                            }
                            else if (workType == WorkTypeDefOf.Crafting || workType == WorkTypeDefOf.Mining || workType == WorkTypeDefOf.Hauling || workType == WorkTypeDefOf.Doctor || workType == WorkTypeDefOf.Hunting || workType == WorkTypeDefOf.Construction || workType == WorkTypeDefOf.Growing || workType == AsimovDefOf.BasicWorker || workType == AsimovDefOf.Cooking || workType == WorkTypeDefOf.PlantCutting || workType == WorkTypeDefOf.Research || workType == AsimovDefOf.Cleaning || workType == WorkTypeDefOf.Firefighter || workType == AsimovDefOf.Tailoring || workType == AsimovDefOf.Art || workType == WorkTypeDefOf.Smithing || workType == AsimovDefOf.Warden || workType == WorkTypeDefOf.Handling)
                            {
                                cachedEnabledWorkTypes.Add(workType);
                            }
                        }
                    }
                }
                return cachedEnabledWorkTypes;
            }
        }

        public List<WorkTypeDef> cachedDisabledWorkTypes;

        public List<WorkTypeDef> DisabledWorkTypes
        {
            get
            {
                if (cachedDisabledWorkTypes == null)
                {
                    cachedDisabledWorkTypes = new List<WorkTypeDef>();
                    if (!Props.enableAllWorkTypes)
                    {
                        foreach (WorkTypeDef workType in DefDatabase<WorkTypeDef>.AllDefs)
                        {
                            if ((!EnabledWorkTypes.Contains(workType)) && !cachedDisabledWorkTypes.Contains(workType))
                            {
                                cachedDisabledWorkTypes.Add(workType);
                            }
                        }
                    }
                }
                return cachedDisabledWorkTypes;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
        }

        public void ResolveGraphics()
        {
            Pawn pawn = parent as Pawn;
            if (pawn != null)
            {
                pawn.Drawer.renderer.SetAllGraphicsDirty();
            }
        }

        public void ResolveWorkRestrictions()
        {
            cachedDisabledWorkTypes = null;
            cachedEnabledWorkTypes = null;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref skinFirst, "skinFirst");
            Scribe_Values.Look(ref skinSecond, "skinSecond");
            Scribe_Values.Look(ref resolved, "resolved");
            Scribe_Collections.Look(ref extraEnabledWorkTypes, "extraEnabledWorkTypes");
        }
    }
}
