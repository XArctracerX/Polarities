using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace Polarities
{
    public static class PolaritiesConditions
    {
        public static Condition DownedStormCloudfish = new("Mods.Polarities.Conditions.DownedStormCloudfish", () => PolaritiesSystem.downedStormCloudfish);
        public static Condition DownedStarConstruct = new("Mods.Polarities.Conditions.DownedStarConstruct", () => PolaritiesSystem.downedStarConstruct);
        public static Condition DownedGigabat = new("Mods.Polarities.Conditions.DownedGigabat", () => PolaritiesSystem.downedGigabat);
        public static Condition DownedRiftDenizen = new("Mods.Polarities.Conditions.DownedRiftDenizen", () => PolaritiesSystem.downedRiftDenizen);
        public static Condition DownedSunPixie = new("Mods.Polarities.Conditions.DownedSunPixie", () => PolaritiesSystem.downedSunPixie);
        public static Condition DownedEsophage = new("Mods.Polarities.Conditions.DownedEsophage", () => PolaritiesSystem.downedEsophage);
        public static Condition DownedConvectiveWanderer = new("Mods.Polarities.Conditions.DownedConvectiveWanderer", () => PolaritiesSystem.downedConvectiveWanderer);
        public static Condition DownedSelfsimilarSentinel = new("Mods.Polarities.Conditions.DownedSelfsimilarSentinel", () => PolaritiesSystem.downedSelfsimilarSentinel);
        public static Condition DownedEclipxie = new("Mods.Polarities.Conditions.DownedEclipxie", () => PolaritiesSystem.downedEclipxie);
        public static Condition DownedHemorrphage = new("Mods.Polarities.Conditions.DownedHemorrphage", () => PolaritiesSystem.downedHemorrphage);
        public static Condition DownedPolarities = new("Mods.Polarities.Conditions.DownedPolarities", () => PolaritiesSystem.downedPolarities);
    }
}