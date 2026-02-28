using Verse;
using System.Collections.Generic;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    [StaticConstructorOnStartup]
    public static class Apparel_Material_Filter_Cache
    {
        private readonly static HashSet<long> _matched = new HashSet<long>();
        private readonly static HashSet<long> _unmatched = new HashSet<long>();

        static Apparel_Material_Filter_Cache()
        {
            Clear();
        }

        public static void Add(SpecialThingFilterDef filterDef, Thing thing, bool matched)
        {
            ushort filterHash = filterDef.shortHash;
            ushort defHash = thing.def.shortHash;
            ushort stuffHash = thing.Stuff?.shortHash ?? 0;
            long key = ((long)filterHash << 32) | ((long)defHash << 16) | stuffHash;

            if (matched)
            {
                if (_matched.Contains(key))
                {
                    Logging_Utility.LogMessage($"---- {thing.def.defName} already exists in the matched cache.");

                    return;
                }

                Logging_Utility.LogMessage($"---- {thing.def.defName} does not exist in the matched cache. Adding it.");

                _matched.Add(key);
                _unmatched.Remove(key);
            }
            else
            {
                if (_unmatched.Contains(key))
                {
                    Logging_Utility.LogMessage($"---- {thing.def.defName} already exists in the unmatched cache.");

                    return;
                }

                Logging_Utility.LogMessage($"---- {thing.def.defName} does not exist in the unmatched cache. Adding it.");

                _unmatched.Add(key);
                _matched.Remove(key);
            }
        }

        public static bool Has(SpecialThingFilterDef filterDef, Thing thing, out bool matched)
        {
            ushort filterHash = filterDef.shortHash; 
            ushort defHash = thing.def.shortHash;
            ushort stuffHash = thing.Stuff?.shortHash ?? 0;
            long key = ((long)filterHash << 32) | ((long)defHash << 16) | stuffHash;

            if (_matched.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName} exists in the matched cache.");

                matched = true;

                return true;
            }

            if (_unmatched.Contains(key))
            {
                Logging_Utility.LogMessage($"---- {thing.def.defName} exists in the unmatched cache.");

                matched = false;

                return true;
            }
    
            Logging_Utility.LogMessage($"---- {thing.def.defName} does not exist in either cache");

            matched = false;

            return false;
    
        }

        public static void Clear()
        {
            Logging_Utility.LogMessage("Clearing cache...");
            _matched.Clear();
            _unmatched.Clear();
        }
    }
}