using Verse;
using System.Collections.Generic;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    [StaticConstructorOnStartup]
    public static class Apparel_Material_Filter_Cache
    {
        /**
         * @var HashSet<long> The cache will store items which matched a material of a DISABLED filter.
         * @private
         * @static
         * @readonly
        */
        private readonly static HashSet<long> _cache = new HashSet<long>();

        /**
         * Constructor.
         *
         * @public
         *
         * @return void
        */
        static Apparel_Material_Filter_Cache()
        {
            Clear();
        }

        /**
         * Add the item of the specified material to the cache.
         *
         * @public
         *
         * @static
         *
         * @param Thing    thing    The thing
         * @param ThingDef material The material
         *
         * @return void
        */
        public static void Add(Thing thing, ThingDef material)
        {
            long key = ((long)thing.thingIDNumber << 32) | (long)material.shortHash;

            if (_cache.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName}-{material.defName} already exists in the cache.");

                return;
            }

            Logging_Utility.LogMessage($"---- {thing.def.defName}-{material.defName} does not exist in the cache. Adding it.");

            _cache.Add(key);
        }

        /**
         * Check if an item of the specified material exists in the cache.
         *
         * The cache stores items which matched a material of a DISABLED filter.
         *
         * @public
         *
         * @static
         *
         * @param Thing    thing    The thing
         * @param ThingDef material The material
         *
         * @return bool
        */
        public static bool Has(Thing thing, ThingDef material)
        {
            long key = ((long)thing.thingIDNumber << 32) | (long)material.shortHash;

            if (_cache.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName}-{material.defName} exists in the the cache.");

                return true;
            }
    
            Logging_Utility.LogMessage($"---- {thing.def.defName}-{material.defName} does not exist in the cache.");

            return false;
    
        }

        /**
         * Clear the cache.
         *
         * @public
         *
         * @static
         *
         * @return void
        */
        public static void Clear()
        {
            Logging_Utility.LogMessage("Clearing cache...");
            _cache.Clear();
        }
    }
}