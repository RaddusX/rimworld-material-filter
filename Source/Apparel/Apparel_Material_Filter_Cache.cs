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
         * Add the item to the cache.
         *
         * The cache will store items which matched a material of a DISABLED filter.
         *
         * @public
         *
         * @static
         *
         * @param SpecialThingFilterDef  filterDef  The filter
         * @param Thing                  thing      The thing
         *
         * @return void
        */
        public static void Add(SpecialThingFilterDef filterDef, Thing thing)
        {
            ushort filterHash = filterDef.shortHash;
            ushort defHash = thing.def.shortHash;
            ushort stuffHash = thing.Stuff?.shortHash ?? 0;
            long key = ((long)filterHash << 32) | ((long)defHash << 16) | stuffHash;

            if (_cache.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName} already exists in the cache.");

                return;
            }

            Logging_Utility.LogMessage($"---- {thing.def.defName} does not exist in the cache. Adding it.");

            _cache.Add(key);
        }

        /**
         * Check if an item exists in the cache.
         *
         * The cache stores items which matched a material of a DISABLED filter.
         *
         * @public
         *
         * @static
         *
         * @param SpecialThingFilterDef  filterDef  The filter
         * @param Thing                  thing      The thing
         *
         * @return bool
        */
        public static bool Has(SpecialThingFilterDef filterDef, Thing thing)
        {
            ushort filterHash = filterDef.shortHash; 
            ushort defHash = thing.def.shortHash;
            ushort stuffHash = thing.Stuff?.shortHash ?? 0;
            long key = ((long)filterHash << 32) | ((long)defHash << 16) | stuffHash;

            if (_cache.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName} exists in the the cache.");

                return true;
            }
    
            Logging_Utility.LogMessage($"---- {thing.def.defName} does not exist in the cache.");

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