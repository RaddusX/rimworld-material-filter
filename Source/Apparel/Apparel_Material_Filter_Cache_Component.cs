
using Verse;
using System.Reflection;
using System.Linq;
using RimWorld.Planet;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    public class Apparel_Material_Filter_Cache_Component : WorldComponent
    {
        public Apparel_Material_Filter_Cache_Component(World world) : base(world) { }

        public override void FinalizeInit(bool fromLoad)
        {
            Logging_Utility.LogMessage("Allow_Apparel_Material_Cache_Component.FinalizeInit() called.");
            Apparel_Material_Filter_Cache.Clear();
        }
    }
}