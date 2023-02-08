using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld.Planet;
using Verse.AI;

namespace Asimov
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "ChoicesAtFor")]
    public static class Patch_FloatMenuMakerMap_ChoicesAtFor
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<FloatMenuOption> __result, Vector3 clickPos, Pawn pawn, bool suppressAutoTakeableGoto = false)
		{
			if (pawn.IsAutomaton())
			{
				Comp_Automaton comp = pawn.GetComp<Comp_Automaton>();
				if (comp == null || !comp.Props.canUseWeapons)
				{
					return;
				}
				IntVec3 c = IntVec3.FromVector3(clickPos);
				ThingWithComps equipment = null;
				List<Thing> thingList = c.GetThingList(pawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].TryGetComp<CompEquippable>() != null)
					{
						equipment = (ThingWithComps)thingList[i];
						break;
					}
				}
				if (equipment == null)
				{
					return;
				}
				string labelShort = equipment.LabelShort;
				FloatMenuOption item;
				string cantReason;
				if (!pawn.CanReach(equipment, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					item = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
				}
				else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					item = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate(), null);
				}
				else if (equipment.IsBurning())
				{
					item = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null);
				}
				else if (pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(equipment, pawn))
				{
					item = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null);
				}
				else if (!EquipmentUtility.CanEquip(equipment, pawn, out cantReason, checkBonded: false))
				{
					item = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + cantReason.CapitalizeFirst(), null);
				}
				else
				{
					string text = "Equip".Translate(labelShort);
					if (EquipmentUtility.AlreadyBondedToWeapon(equipment, pawn))
					{
						text += " " + "BladelinkAlreadyBonded".Translate();
						TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(pawn.Named("PAWN"), equipment.Named("WEAPON"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
						item = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
						{
							Find.WindowStack.Add(new Dialog_MessageBox(dialogText));
						}, MenuOptionPriority.High), pawn, (LocalTargetInfo)equipment, "ReservedBy");
					}
					else
					{
						item = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
						{
							string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(equipment, pawn);
							if (!personaWeaponConfirmationText.NullOrEmpty())
							{
								Find.WindowStack.Add(new Dialog_MessageBox(personaWeaponConfirmationText, "Yes".Translate(), delegate
								{
									Equip();
								}, "No".Translate()));
							}
							else
							{
								Equip();
							}
						}, MenuOptionPriority.High), pawn, (LocalTargetInfo)equipment, "ReservedBy");
					}
					void Equip()
					{
						equipment.SetForbidden(value: false);
						pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Equip, equipment), JobTag.Misc);
						FleckMaker.Static(equipment.DrawPos, equipment.MapHeld, FleckDefOf.FeedbackEquip);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EquippingWeapons, KnowledgeAmount.Total);
					}
				}
				__result.Add(item);
			}
		}
    }
}
