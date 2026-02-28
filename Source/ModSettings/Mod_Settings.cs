using UnityEngine;
using Verse;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.ModSettings
{
    public class Mod_Settings : Verse.ModSettings
    {
        /**
        * Whether logging is enabled
        * @param bool
        */
        public bool loggingEnabled = false;

        /**
         * Write our settings to file.
         * @return void
        */
        public override void ExposeData()
        {
            Scribe_Values.Look(ref loggingEnabled, "loggingEnabled");
            base.ExposeData();
        }
    }

    public class Mod : Verse.Mod
    {
        /**
        * Our mod settings reference
        * @param ModSettings
        */
        Mod_Settings settings;

        /**
         * Constructor
         * @return void
        */
        public Mod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Mod_Settings>();
        }

        /**
         * Add our settings.
         * @return void
        */
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            /*
                Settings
            */

            Text.Font = GameFont.Medium;
            listingStandard.Label("RaddusX.MaterialFilter.Settings.Label".Translate());
            Text.Font = GameFont.Small;

            listingStandard.Gap(5f);

            // "Logging Enabled?"
            listingStandard.CheckboxLabeled("RaddusX.MaterialFilter.Settings.LoggingEnabled.Label".Translate(), ref settings.loggingEnabled, "RaddusX.MaterialFilter.Settings.LoggingEnabled.Tooltip".Translate());

            listingStandard.End();

            base.DoSettingsWindowContents(inRect);
        }

        /**
         * The name of the settings in the Mod Options panel.
         * @return string
        */
        public override string SettingsCategory()
        {
            return "RaddusX - Material Filter";
        }
    }
}