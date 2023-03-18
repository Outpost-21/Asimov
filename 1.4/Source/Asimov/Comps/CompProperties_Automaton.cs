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

        public bool canUseWeapons = false;

        public List<WorkTypeDef> enabledWorkTypes = new List<WorkTypeDef>();

        public List<SkillLevelSetting> skillSettings = new List<SkillLevelSetting>();

        public float skillGainMulti = 0f;

        public float skillLossMulti = 0f;

        public bool corpseEdible = false;

        public bool corpseRots = false;

        public List<ThingDef> repairThings = new List<ThingDef>();

        // Only Applies to Humanlikes

        public bool colonyCaresIfDead = false;
    }
}
