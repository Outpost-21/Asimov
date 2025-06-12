using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class AsimovMod : Mod
    {
        public static AsimovMod mod;
        public static AsimovSettings settings;

        public Vector2 optionsScrollPosition;
        public float optionsViewRectHeight;

        internal static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public AsimovMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<AsimovSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            LogUtil.LogMessage($"{CurrentVersion} ::");

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            Harmony harmony = new Harmony("Neronix17.Asimov.RimWorld");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory() => "Asimov";


        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            bool flag = optionsViewRectHeight > inRect.height;
            Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width - (flag ? 26f : 0f), optionsViewRectHeight);
            Widgets.BeginScrollView(inRect, ref optionsScrollPosition, viewRect);
            Listing_Standard listing = new Listing_Standard();
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
            listing.Begin(rect);
            // ============================ CONTENTS ================================
            DoOptionsCategoryContents(listing);
            // ======================================================================
            optionsViewRectHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }

        public void DoOptionsCategoryContents(Listing_Standard listing)
        {
            listing.Label("Asimov.Setting_EnergyNeedLabel".Translate());
            listing.GapLine();
            listing.Note("Asimov.Setting_EnergyNeedNote".Translate());
            listing.AddLabeledSlider("Asimov.Setting_LowThreshold".Translate(settings.energyNormal.ToStringPercent()), ref settings.energyNormal, 0f, 1f);
            listing.Note("Asimov.Setting_LowThresholdNote".Translate().Colorize(Color.grey));
            if(settings.energyNormal < settings.energyDesperate) { settings.energyDesperate = settings.energyNormal; }
            listing.AddLabeledSlider("Asimov.Setting_DesperateThreshold".Translate(settings.energyDesperate.ToStringPercent()), ref settings.energyDesperate, 0f, 1f);
            listing.Note("Asimov.Setting_DesperateThresholdNote".Translate().Colorize(Color.grey));
            if (settings.energyDesperate > settings.energyNormal) { settings.energyNormal = settings.energyDesperate; }
            listing.AddLabeledSlider("Asimov.Setting_EnergyDrainRate".Translate(settings.energyDrainMultiplier.ToStringPercent()), ref settings.energyDrainMultiplier, 0f, 2f);
            listing.CheckboxEnhanced("Asimov.Setting_ForceShowTechnology".Translate(), "Asimov.Setting_ForceShowTechnologyDesc", ref settings.forceShowEnergyStuff);
        }
    }
}
