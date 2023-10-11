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
    public class CompProperties_Automaton : CompProperties
    {
        public CompProperties_Automaton()
        {
            compClass = typeof(Comp_Automaton);
        }

        public bool canUseRanged = true;
        public bool canUseMelee = true;
        public bool corpseEdible = true;
        public bool corpseRots = true;
        public bool affectedByEMP = false;
        public bool immuneToDisease = false;
        public bool huntTarget = true;
        public bool flattenSkills = false;
        public bool clearPassions = false;

        public float skillGainMulti = 1f;
        public float skillLossMulti = 1f;

        public int defaultSkillLevel = 0;

        public string workDisableTerm = "programming";

        public bool enableAllWorkTypes = false;
        public List<WorkTypeDef> enabledWorkTypes = new List<WorkTypeDef>();

        public List<SkillLevelSetting> skillSettings = new List<SkillLevelSetting>();

        public List<ThingDef> repairThings = new List<ThingDef>();

        public List<NeedDef> disabledNeeds = new List<NeedDef>();
    }
}
