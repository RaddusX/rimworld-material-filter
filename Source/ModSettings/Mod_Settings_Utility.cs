using Verse;

namespace RaddusX.MaterialFilter.ModSettings
{
    public static class Mod_Settings_Utility
    {
        private static Mod_Settings modSettings;

        static Mod_Settings_Utility()
        {
            modSettings = LoadedModManager.GetMod<Mod>().GetSettings<Mod_Settings>();
        }

        /**
         * Whether logging is enabled.
         *
         * @public
         *
         * @return bool
        */
        public static bool IsLoggingEnabled()
        {
            return modSettings.loggingEnabled;
        }
    }
}