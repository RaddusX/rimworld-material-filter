
using Verse;
using System.Reflection;
using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter
{
    [StaticConstructorOnStartup]
    public static class Material_Filter
    {
        static Material_Filter()
        {
            Logging_Utility.LogMessage("Mod loaded.");
        }
    }
}