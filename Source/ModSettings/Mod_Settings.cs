using UnityEngine;
using Verse;
using RimWorld;

using RaddusX.MaterialFilter.Utility;
using RaddusX.MaterialFilter.Apparel;

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

            // Generate Filter Definitions

            if (listingStandard.ButtonTextLabeled("RaddusX.MaterialFilter.Settings.GenerateFilterDefs.Label".Translate(), "RaddusX.MaterialFilter.Settings.GenerateFilterDefs.ButtonLabel".Translate()))
            {
                Apparel_Material_Filter_Def_Generator.Generate();

                Messages.Message("RaddusX.MaterialFilter.Settings.GenerateFilterDefs.SuccessMessage".Translate(), MessageTypeDefOf.PositiveEvent, false);
            }

            listingStandard.SubLabel("RaddusX.MaterialFilter.Settings.GenerateFilterDefs.Description".Translate(), 1f);

            listingStandard.Gap(5f);

            // Clear Cache

            if (listingStandard.ButtonTextLabeled("RaddusX.MaterialFilter.Settings.ClearCache.Label".Translate(), "RaddusX.MaterialFilter.Settings.ClearCache.ButtonLabel".Translate()))
            {
                Apparel_Material_Filter_Cache.Clear();

                Messages.Message("RaddusX.MaterialFilter.Settings.ClearCache.SuccessMessage".Translate(), MessageTypeDefOf.PositiveEvent, false);
            }

            listingStandard.SubLabel("RaddusX.MaterialFilter.Settings.ClearCache.Description".Translate(), 1f);

            /*
                Advanced Settings
            */

            Text.Font = GameFont.Medium;
            listingStandard.Label("RaddusX.MaterialFilter.AdvancedSettings.Label".Translate());
            Text.Font = GameFont.Small;

            listingStandard.Gap(5f);

            // "Logging Enabled?"
            listingStandard.CheckboxLabeled("RaddusX.MaterialFilter.Settings.LoggingEnabled.Label".Translate(), ref settings.loggingEnabled);
            listingStandard.SubLabel("RaddusX.MaterialFilter.Settings.LoggingEnabled.Description".Translate(), 1f);

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