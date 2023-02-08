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

        public AsimovSettingsPage currentPage = AsimovSettingsPage.General;
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
            listing.SettingsDropdown("Current Page", "", ref currentPage, listing.ColumnWidth);
            listing.Note("You will need to restart the game for most of these settings to take effect.", GameFont.Tiny);
            listing.GapLine();
            if (currentPage == AsimovSettingsPage.General)
            {
                DoOptions_General(listing);
            }
        }

        public void DoOptions_General(Listing_Standard listing)
        {

        }
    }
}
