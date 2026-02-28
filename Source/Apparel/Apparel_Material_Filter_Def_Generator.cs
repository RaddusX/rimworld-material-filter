using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

using RaddusX.MaterialFilter.Utility;

namespace RaddusX.MaterialFilter.Apparel
{
    [StaticConstructorOnStartup]
    public static class Apparel_Material_Filter_Def_Generator
    {
        /**
         * Constructor.
         * 
         * @public
         *
         * @static
         *
         * @return void
        */
        static Apparel_Material_Filter_Def_Generator()
        {
            Logging_Utility.LogMessage("Apparel_Material_Filter_Def_Generator() called.");

            Generate();
        }

        /**
         * Generate the filters for the materials.
         * 
         * @public
         *
         * @static
         *
         * @return void
        */
        public static void Generate()
        {
            Logging_Utility.LogMessage("Generate() called.");

            // Get materials, sort alphabetically
            var materials = DefDatabase<ThingDef>.AllDefs.Where(d =>
            {
                if (!d.IsStuff || d.stuffProps == null)
                {
                    return false;
                }

                return d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric) || 
                       d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery) || 
                       d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic) ||
                       d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody);

            }).OrderBy(d => d.label);

            Logging_Utility.LogMessage($"Materials found: {materials.Count()}");

            foreach (var material in materials)
            {
                Logging_Utility.LogMessage($"Adding material {material.defName}");

                string defName = "RaddusX_Material_Filter_Apparel_Allow_" + material.defName;

                // Filter already exists
                if (DefDatabase<SpecialThingFilterDef>.GetNamed(defName, false) != null)
                {
                    Logging_Utility.LogMessage($"defName '{defName}' already exists. Skipping.");
                    continue;
                }

                // Create filter
                SpecialThingFilterDef materialFilter = new SpecialThingFilterDef
                {
                    defName = defName,
                    label = "allow " + material.label,
                    description = "allow " + material.label,
                    parentCategory = ThingCategoryDefOf.Apparel,
                    allowedByDefault = true,
                    workerClass = typeof(Apparel_Material_Filter_Worker),
                    modExtensions = new List<DefModExtension> { 
                        new Material_Filter_Extension { resolvedDef = material } 
                    }
                };

                // Add filter
                DefDatabase<SpecialThingFilterDef>.Add(materialFilter);

                if (ThingCategoryDefOf.Apparel.childSpecialFilters == null)
                {
                    ThingCategoryDefOf.Apparel.childSpecialFilters = new List<SpecialThingFilterDef>();
                }
            
                ThingCategoryDefOf.Apparel.childSpecialFilters.Add(materialFilter);
            }

            // Update
            DefDatabase<SpecialThingFilterDef>.ResolveAllReferences();
            ThingCategoryDefOf.Apparel.ResolveReferences();
        }
    }
}