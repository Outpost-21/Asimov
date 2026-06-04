using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace Asimov
{
    public class PawnColumnWorker_SkillCheckbox : PawnColumnWorker
    {
        public const int LabelRowHeight = 50;

        public Vector2 cachedWorkLabelSize;

        public override bool VisibleCurrently => def.workType.VisibleCurrently;

        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
            {
                return;
            }
            Text.Font = GameFont.Medium;
            float x = rect.x + (rect.width - 25f) / 2f;
            float y = rect.y + 2.5f;
            bool incapable = IsIncapableOfWholeWorkType(pawn, def.workType);
            DrawWorkBoxFor(x, y, pawn, def.workType, incapable);
            Rect rect2 = new Rect(x, y, 25f, 25f);
            if (Mouse.IsOver(rect2))
            {
                TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, def.workType, incapable), pawn.thingIDNumber ^ def.workType.GetHashCode());
            }
            Text.Font = GameFont.Small;
        }

        public override int GetMinHeaderHeight(PawnTable table)
        {
            return 50;
        }

        public override int GetMinWidth(PawnTable table)
        {
            return Mathf.Max(base.GetMinWidth(table), 32);
        }

        public override int GetOptimalWidth(PawnTable table)
        {
            return Mathf.Clamp(39, GetMinWidth(table), GetMaxWidth(table));
        }

        public override int GetMaxWidth(PawnTable table)
        {
            return Mathf.Min(base.GetMaxWidth(table), 80);
        }

        public bool IsIncapableOfWholeWorkType(Pawn p, WorkTypeDef work)
        {
            for (int i = 0; i < work.workGiversByPriority.Count; i++)
            {
                bool flag = true;
                for (int j = 0; j < work.workGiversByPriority[i].requiredCapacities.Count; j++)
                {
                    PawnCapacityDef capacity = work.workGiversByPriority[i].requiredCapacities[j];
                    if (!p.health.capacities.CapableOf(capacity))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return false;
                }
            }
            return true;
        }

        public override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
        {
            return GetLabelRect(headerRect);
        }

        public override int Compare(Pawn a, Pawn b)
        {
            return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
        }

        public float GetValueToCompare(Pawn pawn)
        {
            if (pawn.workSettings == null || !pawn.workSettings.EverWork)
            {
                return -2f;
            }
            if (pawn.WorkTypeIsDisabled(def.workType))
            {
                return -1f;
            }
            return pawn.skills.AverageOfRelevantSkillsFor(def.workType);
        }

        public Rect GetLabelRect(Rect headerRect)
        {
            float x = headerRect.center.x;
            Rect result = new Rect(x - cachedWorkLabelSize.x / 2f, headerRect.y, cachedWorkLabelSize.x, cachedWorkLabelSize.y);
            if (def.moveWorkTypeLabelDown)
            {
                result.y += 20f;
            }
            return result;
        }

        public override string GetHeaderTip(PawnTable table)
        {
            TaggedString taggedString = def.workType.gerundLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + def.workType.description + "\n\n" + SpecificWorkListString(def.workType);
            taggedString += "\n";
            if (def.sortable)
            {
                taggedString += "\n" + "ClickToSortByThisColumn".Translate().Colorize(ColoredText.SubtleGrayColor);
            }
            if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
            {
                if (AsimovMod.settings.doManualPriorities)
                {
                    taggedString += "\n" + "WorkPriorityShiftClickTip".Translate().Colorize(ColoredText.SubtleGrayColor);
                }
                else
                {
                    taggedString += "\n" + "WorkPriorityShiftClickEnableDisableTip".Translate().Colorize(ColoredText.SubtleGrayColor);
                }
            }
            return taggedString.Resolve();
        }

        public static string SpecificWorkListString(WorkTypeDef def)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < def.workGiversByPriority.Count; i++)
            {
                stringBuilder.Append(" - " + def.workGiversByPriority[i].LabelCap);
                if (def.workGiversByPriority[i].emergency)
                {
                    stringBuilder.Append(" (" + "EmergencyWorkMarker".Translate() + ")");
                }
                if (i < def.workGiversByPriority.Count - 1)
                {
                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString();
        }

        public override void HeaderClicked(Rect headerRect, PawnTable table)
        {
            base.HeaderClicked(headerRect, table);
            if (!Event.current.shift)
            {
                return;
            }
            List<Pawn> pawnsListForReading = table.PawnsListForReading;
            for (int i = 0; i < pawnsListForReading.Count; i++)
            {
                Pawn pawn = pawnsListForReading[i];
                if (pawn.workSettings == null || !pawn.workSettings.EverWork || pawn.WorkTypeIsDisabled(def.workType))
                {
                    continue;
                }
                if (AsimovMod.settings.doManualPriorities)
                {
                    int priority = pawn.workSettings.GetPriority(def.workType);
                    if (Event.current.button == 0 && priority != 1)
                    {
                        int num = priority - 1;
                        if (num < 0)
                        {
                            num = 4;
                        }
                        pawn.workSettings.SetPriority(def.workType, num);
                    }
                    if (Event.current.button == 1 && priority != 0)
                    {
                        int num2 = priority + 1;
                        if (num2 > 4)
                        {
                            num2 = 0;
                        }
                        pawn.workSettings.SetPriority(def.workType, num2);
                    }
                }
                else if (pawn.workSettings.GetPriority(def.workType) > 0)
                {
                    if (Event.current.button == 1)
                    {
                        pawn.workSettings.SetPriority(def.workType, 0);
                    }
                }
                else if (Event.current.button == 0)
                {
                    pawn.workSettings.SetPriority(def.workType, 3);
                }
            }
            if (AsimovMod.settings.doManualPriorities)
            {
                SoundDefOf.DragSlider.PlayOneShotOnCamera();
            }
            else if (Event.current.button == 0)
            {
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            }
            else if (Event.current.button == 1)
            {
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }
        }

        public static void DrawWorkBoxFor(float x, float y, Pawn p, WorkTypeDef wType, bool incapableBecauseOfCapacities)
        {
            if (p.WorkTypeIsDisabled(wType))
            {
                return;
            }
            Rect rect2 = new Rect(x, y, 25f, 25f);
            if (incapableBecauseOfCapacities)
            {
                GUI.color = new Color(1f, 0.3f, 0.3f);
            }
            WidgetsWork.DrawWorkBoxBackground(rect2, p, wType);
            GUI.color = Color.white;
            if (AsimovMod.settings.doManualPriorities)
            {
                int priority = p.workSettings.GetPriority(wType);
                if (priority > 0)
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    GUI.color = WidgetsWork.ColorOfPriority(priority);
                    Widgets.Label(rect2.ContractedBy(-3f), priority.ToStringCached());
                    GUI.color = Color.white;
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                if (Event.current.type != EventType.MouseDown || !Mouse.IsOver(rect2))
                {
                    return;
                }
                bool num = p.workSettings.WorkIsActive(wType);
                if (Event.current.button == 0)
                {
                    int num2 = p.workSettings.GetPriority(wType) - 1;
                    if (num2 < 0)
                    {
                        num2 = 4;
                    }
                    p.workSettings.SetPriority(wType, num2);
                    SoundDefOf.DragSlider.PlayOneShotOnCamera();
                }
                if (Event.current.button == 1)
                {
                    int num3 = p.workSettings.GetPriority(wType) + 1;
                    if (num3 > 4)
                    {
                        num3 = 0;
                    }
                    p.workSettings.SetPriority(wType, num3);
                    SoundDefOf.DragSlider.PlayOneShotOnCamera();
                }
                if (!num && p.workSettings.WorkIsActive(wType) && wType.relevantSkills.Any() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
                {
                    SoundDefOf.Crunch.PlayOneShotOnCamera();
                }
                if (!num && p.workSettings.WorkIsActive(wType) && p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
                {
                    Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, historical: false);
                    SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera();
                }
                Event.current.Use();
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ManualWorkPriorities, KnowledgeAmount.SmallInteraction);
                return;
            }
            if (p.workSettings.GetPriority(wType) > 0)
            {
                GUI.DrawTexture(rect2, WidgetsWork.WorkBoxCheckTex);
            }
            if (!Widgets.ButtonInvisible(rect2))
            {
                return;
            }
            if (p.workSettings.GetPriority(wType) > 0)
            {
                p.workSettings.SetPriority(wType, 0);
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }
            else
            {
                p.workSettings.SetPriority(wType, 3);
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                if (wType.relevantSkills.Any() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
                {
                    SoundDefOf.Crunch.PlayOneShotOnCamera();
                }
                if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
                {
                    Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, historical: false);
                    SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera();
                }
            }
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
        }
    }
}
