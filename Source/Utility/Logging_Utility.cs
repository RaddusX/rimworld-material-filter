using Verse;

namespace RaddusX.MaterialFilter.Utility
{
    public static class Logging_Utility
    {
        public static void LogMessage(string message)
        {
            Log.Message("RaddusX's Material Filter: " + message);
        }

        public static void LogWarning(string message)
        {
            Log.Warning("RaddusX's Material Filter: " + message);
        }

        public static void LogError(string message)
        {
            Log.Error("RaddusX's Material Filter: " + message);
        }
    }
}