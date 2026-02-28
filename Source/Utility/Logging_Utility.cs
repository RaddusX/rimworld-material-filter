using Verse;

using RaddusX.MaterialFilter.ModSettings;

namespace RaddusX.MaterialFilter.Utility
{
    public static class Logging_Utility
    {
        public static void LogMessage(string message)
        {
            if (Mod_Settings_Utility.IsLoggingEnabled())
            {
                Log.Message("RaddusX's Material Filter: " + message);
            }
        }

        public static void LogWarning(string message)
        {
            if (Mod_Settings_Utility.IsLoggingEnabled())
            {
                Log.Warning("RaddusX's Material Filter: " + message);
            }
        }

        public static void LogError(string message)
        {
            if (Mod_Settings_Utility.IsLoggingEnabled())
            {
                Log.Error("RaddusX's Material Filter: " + message);
            }
        }
    }
}