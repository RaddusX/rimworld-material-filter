using RimWorld.Planet;

using RaddusX.MaterialFilter.Utility;
using RaddusX.MaterialFilter.Apparel;
using RaddusX.MaterialFilter.Cache;

namespace RaddusX.MaterialFilter.Cache
{
    public class Thing_Material_Filter_Cache_Component : WorldComponent
    {
        public Thing_Material_Filter_Cache_Component(World world) : base(world) { }

        /**
         * Called on new game / save load.
         *
         * We use this to perform any actions on new game / save load related to caching.
         * 
         * @public
         *
         * @param bool fromLoad true = loaded a save, false = new game
         *
         * @return void
        */
        public override void FinalizeInit(bool fromLoad)
        {
            Logging_Utility.LogMessage("Thing_Material_Filter_Cache_Component.FinalizeInit() called.");
            
            /*
                Regenerate Apparel Material Filter Definitions
                (Creates filters for any new materials that may have been added)
            */
            Apparel_Material_Filter_Def_Generator.Generate();

            /*
                Clear cache
            */
            Thing_Material_Cache.Clear();
        }
    }
}