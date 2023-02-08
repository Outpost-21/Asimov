using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Asimov
{
	public class Utility_AutoProducerCard
	{
		public static void DrawAutoBillCard(Rect rect, Building_AutoCrafter building)
		{

		}

		public static void DrawRepeatModeConfig(Comp_AutoCrafter comp)
		{
			List<FloatMenuOption> modeList = new List<FloatMenuOption>();
			modeList.Add(new FloatMenuOption("Don't Repeat", delegate ()
			{
				comp.repeatMode = RepeatMode.none;
			}));
			if (comp.curRecipe != null)
			{
				FloatMenuOption item = new FloatMenuOption("Repeat Until X", delegate ()
				{
					comp.repeatMode = RepeatMode.until;
					comp.repeatTarget = 0;
				});
				modeList.Add(item);
			}
			modeList.Add(new FloatMenuOption("Repeat X Times", delegate ()
			{
				comp.repeatMode = RepeatMode.times;
			}));
			modeList.Add(new FloatMenuOption("Repeat Forever", delegate ()
			{
				comp.repeatMode = RepeatMode.forever;
			}));
			Find.WindowStack.Add(new FloatMenu(modeList));
		}

		public static string RepeatInfoString(Comp_AutoCrafter comp)
		{
			string info = "";
			if (comp.repeatMode == RepeatMode.none)
			{
				info = "N/A";
			}
			else if (comp.repeatMode == RepeatMode.until)
			{
				if (comp.curRecipe != null)
				{
					info = comp.repeatTarget + " / " + comp.CheckRepeatCountProducts(comp.curRecipe);
				}
				else
				{
					info = "No Recipe";
				}
			}
			else if (comp.repeatMode == RepeatMode.times)
			{
				info = comp.repeatCount + " times";
			}
			return info;
		}

		public static void DoConfigInterface(Rect baseRect, Color baseColor, Comp_AutoCrafter comp)
		{
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenDown, 99999f, 4f);
			if (widgetRow.ButtonText(comp.RepeatString(), null, true, false))
			{
				DrawRepeatModeConfig(comp);
			}
			if (widgetRow.ButtonIcon(TexUtil.Plus, null, null))
			{
				if (comp.repeatMode == RepeatMode.forever)
				{
					comp.repeatMode = RepeatMode.times;
					comp.repeatCount = 1;
				}
				else if (comp.repeatMode == RepeatMode.until)
				{
					comp.repeatTarget ++;
				}
				else if (comp.repeatMode == RepeatMode.times)
				{
					comp.repeatCount++;
				}
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (widgetRow.ButtonIcon(TexUtil.Minus, null, null))
			{
				if (comp.repeatMode == RepeatMode.forever)
				{
					comp.repeatMode = RepeatMode.times;
					comp.repeatCount = 1;
				}
				else if (comp.repeatMode == RepeatMode.until)
				{
					if (comp.repeatTarget > 0)
					{
						comp.repeatTarget--;
					}
				}
				else if (comp.repeatMode == RepeatMode.times)
				{
					if (comp.repeatCount > 0)
					{
						comp.repeatCount--;
					}
				}
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}
	}
}
