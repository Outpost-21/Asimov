﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class Automaton : Pawn
    {
        public Comp_Automaton compAutomaton;

        public Comp_Automaton CompAutomaton { get { if (compAutomaton == null) { compAutomaton = this.TryGetComp<Comp_Automaton>(); } return compAutomaton; } }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            bool vanillaDraftButton = false;
            IEnumerable<Gizmo> originalGizmos = base.GetGizmos();
            for (int i = 0; i < originalGizmos.Count(); i++)
            {
                Gizmo gizmo = originalGizmos.ElementAt(i);
                if (gizmo is Command_Toggle command && command.defaultDesc == "CommandToggleDraftDesc".Translate())
                {
                    vanillaDraftButton = true;
                }
                yield return gizmo;
            }

            if(!vanillaDraftButton && drafter != null && (Faction?.IsPlayer ?? false))
            {
                yield return new Command_Toggle
                {
                    toggleAction = delegate
                    {
                        drafter.Drafted = !drafter.Drafted;
                    },
                    isActive = () => drafter.Drafted,
                    defaultLabel = (drafter.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate(),
                    hotKey = KeyBindingDefOf.Command_ColonistDraft,
                    defaultDesc = "CommandToggleDraftDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("ui/commands/Draft"),
                    turnOnSound = SoundDefOf.DraftOn,
                    groupKey = 81729172,
                    turnOffSound = SoundDefOf.DraftOff
                };
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (workSettings == null)
            {
                UpdateWorkPriorities();
            }
        }

        public void UpdateWorkPriorities()
        {
            InitialisePawnData();
            if (training != null && Faction != null && Faction.IsPlayer)
            {
                if (!compAutomaton.EnabledWorkTypes.NullOrEmpty())
                {
                    for (int i = 0; i < compAutomaton.EnabledWorkTypes.Count; i++)
                    {
                        workSettings.SetPriority(compAutomaton.EnabledWorkTypes[i], 3);
                    }
                }
            }
        }

        public void InitialisePawnData()
        {

            Initialise_Naming();
            Initialise_Training();
            Initialise_Drafter();
            Initialise_Equipment();
            Initialise_Skills();
            Initialise_Story();
            Initialise_WorkSettings();
        }

        public void Initialise_Naming()
        {
            if(Name == null)
            {
                Name = new NameSingle(Label);
            }
        }

        public void Initialise_Training()
        {
            if (training != null)
            {
                foreach (TrainableDef trainable in DefDatabase<TrainableDef>.AllDefs)
                {
                    if (training.CanAssignToTrain(trainable))
                    {
                        training.SetWanted(trainable, true);
                        training.Train(trainable, null, true);
                    }
                }
            }
        }

        public void Initialise_Drafter()
        {
            if (drafter == null)
            {
                drafter = new Pawn_DraftController(this);
            }
        }

        public void Initialise_Equipment()
        {
            if (equipment == null)
            {
                equipment = new Pawn_EquipmentTracker(this);
            }
            if (outfits == null)
            {
                outfits = new Pawn_OutfitTracker(this);
            }
            if (apparel == null)
            {
                apparel = new Pawn_ApparelTracker(this);
            }
        }

        public void Initialise_Skills()
        {
            if (skills == null)
            {
                skills = new Pawn_SkillTracker(this);
                foreach (SkillRecord skill in skills.skills)
                {
                    skill.Level = 0;
                }
                for (int i = 0; i < CompAutomaton.Props.skillSettings.Count; i++)
                {
                    skills.skills.Find(sk => sk.def == compAutomaton.Props.skillSettings[i].skill).Level = CompAutomaton.Props.skillSettings[i].level;
                }
            }
        }

        public void Initialise_Story()
        {
            if (story == null)
            {
                story = new Pawn_StoryTracker(this)
                {
                    bodyType = BodyTypeDefOf.Thin
                };
            }
        }

        public void Initialise_WorkSettings()
        {
            if (workSettings == null)
            {
                workSettings = new Pawn_WorkSettings(this);
                if (workSettings.priorities == null)
                {
                    workSettings.priorities = new DefMap<WorkTypeDef, int>();
                }
                // Doing the necessary parts manually since this otherwise errors about setting Doctor even when it's obviously disabled.
                //workSettings.EnableAndInitialize();
                //workSettings.DisableAll();
            }
        }
    }
}
