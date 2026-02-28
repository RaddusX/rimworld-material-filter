
using Verse;
using System.Reflection;
using System.Linq;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    [StaticConstructorOnStartup]
    public class Material_Filter_Extension : DefModExtension
    {
        public string defName;

        public ThingDef resolvedDef // We will generate this from the string defName above
        {
            get
            {
                if (_resolvedDef == null)
                {
                    _resolvedDef = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
                }
                return _resolvedDef;
            }
        }

        private ThingDef _resolvedDef;

        static Material_Filter_Extension()
        {
            Logging_Utility.LogMessage("Material_Filter_Extension ctr called.");
        }
    }

    public class Apparel_Material_Filter_Worker : SpecialThingFilterWorker
    {
        private SpecialThingFilterDef __filterDef;

        private SpecialThingFilterDef _filterDef
        {
            get
            {
                if (__filterDef == null)
                {
                    __filterDef = DefDatabase<SpecialThingFilterDef>.AllDefs.FirstOrDefault(d => d.Worker == this);
                }
                return __filterDef;
            }
        }

        /**
         * Whether the filter should be displayed for a category.
         *
         * In our case they should only if they are apparel.
         *
         * @public
         *
         * ThingDef def The thing's def
         *
         * @return bool
        */
        public override bool CanEverMatch(ThingDef def)
        {
            Logging_Utility.LogMessage("CanEverMatch() called.");

            if (!def.IsApparel)
            {
                return false;
            }

            return true;
        }

        /**
         * Whether all things should always match the filter.
         *
         * They should never always match. They should check each item (in Matches()) to make sure its made of the specified material.
         *
         * @public
         *
         * ThingDef def The thing's def
         *
         * @return bool
        */
        public override bool AlwaysMatches(ThingDef def)
        {
            Logging_Utility.LogMessage("AlwaysMatches() called.");

            return true;
        }
        
       /**
         * Whether the filter applies to this item.
         *
         * @public
         *
         * @param Thing t The item
         *
         * @return bool
        */
        public override bool Matches(Thing t)
        {
            Logging_Utility.LogMessage($"Filter '{_filterDef.label}': Checking {t.def.defName}...");

            /*
                Not apparel
            */
            if (!t.def.IsApparel)
            {
                Logging_Utility.LogMessage("---- Item is not apparel. Skipping.");
                return false;
            }

            /*
                Cached?
            */
            bool matched = false;
            if (Apparel_Material_Filter_Cache.Has(_filterDef, t, out matched))
            {
                Logging_Utility.LogMessage($"---- Found {t.def.defName} in cache. Returning {matched}...");

                return matched;
            }

            /*
                Check filter mod extension - extra precaution
            */
            var filterExtension = _filterDef?.GetModExtension<MaterialFilterExtension>();

            if (filterExtension == null)
            {
                Logging_Utility.LogMessage("---- Item's filter extension is null. Skipping.");
                return false;
            }

            if (filterExtension.resolvedDef == null)
            {
                Logging_Utility.LogMessage("---- Item's filter extension resolvedDef is null. Skipping.");
                return false;
            }

            /*
                Check Stuff to see what it's made of (Note: not all apparel will have a Stuff property)
            */
            if (t.Stuff != null)
            {
                if (t.Stuff == filterExtension.resolvedDef)
                {
                    Logging_Utility.LogMessage($"---- Does {t.Stuff.defName} match {filterExtension.defName}? YES.");

                    Apparel_Material_Filter_Cache.Add(_filterDef, t, matched: true);

                    return true;
                }

                Logging_Utility.LogMessage($"---- Does {t.Stuff.defName} match {filterExtension.defName}? No.");
                
            }

            /*
                Check cost list to see what it's made of (most commonly for armor and not Stuffable apparel)
            */
            if (t.def.costList == null)
            {
                Logging_Utility.LogMessage("---- No costList found. Skipping.");
                
                Apparel_Material_Filter_Cache.Add(_filterDef, t, matched: false);

                return false; // clothing doesn't use costList so we can skip checking it below.
            }

            foreach (ThingDefCountClass cost in t.def.costList)
            {
                if (cost.thingDef == filterExtension.resolvedDef)
                {
                    Logging_Utility.LogMessage($"---- Does {cost.thingDef.defName} match {filterExtension.defName}? YES.");

                    Apparel_Material_Filter_Cache.Add(_filterDef, t, matched: true);

                    return true;
                }

                Logging_Utility.LogMessage($"---- Does {cost.thingDef.defName} match {filterExtension.defName}? NO.");
            }

            Apparel_Material_Filter_Cache.Add(_filterDef, t, matched: false);

            Logging_Utility.LogMessage("---- costList was present but didn't match the filter. Skipping.");

            return false;
        }
    }
}