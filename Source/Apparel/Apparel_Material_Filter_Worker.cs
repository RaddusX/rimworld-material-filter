using Verse;
using System.Linq;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    [StaticConstructorOnStartup]
    public class Material_Filter_Extension : DefModExtension
    {
        /**
         * This filter extension's ThingDef. It's generated from the defName above, in the file 'Apparel_Material_Filter_Def_Generator.cs'.
         *
         * @public
         *
         * @var ThingDef
        */
        public ThingDef resolvedDef;

        /**
         * Constructor.
         *
         * @static
        */
        static Material_Filter_Extension()
        {
            Logging_Utility.LogMessage("Material_Filter_Extension ctr called.");
        }
    }

    public class Apparel_Material_Filter_Worker : SpecialThingFilterWorker
    {
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

        private SpecialThingFilterDef __filterDef;

        /**
         * Whether the filter should be displayed for a category.
         *
         * In this case they should only be displayed for the Apparel category.
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

            return false;
        }
        
        /**
         * Whether the filter applies to this item.
         *
         * NOTE: With the way the filters are setup this method will ONLY be called by filters which are DISABLED.
         *
         * @public
         *
         * @param Thing t The item
         *
         * @return bool
        */
        public override bool Matches(Thing t)
        {
            Logging_Utility.LogMessage($"\nFilter '{_filterDef.label}': Checking {t.def.defName}...");

            /*
                Not apparel
            */
            //if (!t.def.IsApparel)
            //{
            //    Logging_Utility.LogMessage("-- Item is not apparel. Skipping.");
            //    return false;
            //}

            /*
                Check filter mod extension
            */
            var filterExtension = _filterDef?.GetModExtension<Material_Filter_Extension>();

            if (filterExtension == null)
            {
                Logging_Utility.LogMessage("-- Item's filter extension is null. Skipping.");
                return false;
            }

            if (filterExtension.resolvedDef == null)
            {
                Logging_Utility.LogMessage("-- Item's filter extension _resolvedDef is null. Skipping.");
                return false;
            }

            /*
                Cached?
            */
            if (Apparel_Material_Filter_Cache.Has(t, filterExtension.resolvedDef))
            {
                Logging_Utility.LogMessage($"-- Found {t.def.defName}-{filterExtension.resolvedDef.defName} in cache. Returning true...");

                return true;
            }

            /*
                Check Stuff to see what it's made of (Note: not all apparel will have a Stuff property)
            */
            if (t.Stuff != null)
            {
                Logging_Utility.LogMessage("-- Stuff property exists.");

                if (t.Stuff == filterExtension.resolvedDef)
                {
                    Logging_Utility.LogMessage($"---- Does {t.Stuff.defName} match {filterExtension.resolvedDef.defName}? YES.");

                    Apparel_Material_Filter_Cache.Add(t, filterExtension.resolvedDef);

                    return true;
                }

                Logging_Utility.LogMessage($"---- Does {t.Stuff.defName} match {filterExtension.resolvedDef.defName}? No.");
                
            }

            /*
                Check cost list to see what it's made of (most commonly for armor and not Stuffable apparel)
            */
            if (t.def.costList == null)
            {
                Logging_Utility.LogMessage("-- No costList found. Skipping.");

                return false;
            }

            Logging_Utility.LogMessage("-- costList property exists.");

            foreach (ThingDefCountClass cost in t.def.costList)
            {
                if (cost.thingDef == filterExtension.resolvedDef)
                {
                    Logging_Utility.LogMessage($"---- Does {cost.thingDef.defName} match {filterExtension.resolvedDef.defName}? YES.");

                    Apparel_Material_Filter_Cache.Add(t, filterExtension.resolvedDef);

                    return true;
                }

                Logging_Utility.LogMessage($"---- Does {cost.thingDef.defName} match {filterExtension.resolvedDef.defName}? NO.");
            }

            Logging_Utility.LogMessage("-- None of the costList's matched the material. Skipping.");

            return false;
        }
    }
}