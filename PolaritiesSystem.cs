using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
//using Polarities.Biomes;
//using Polarities.Items;
//using Polarities.Items.Placeable.Blocks;
//using Polarities.Items.Placeable.Furniture.Salt;
//using Polarities.Items.Placeable.Walls;
//using Polarities.NPCs.ConvectiveWanderer;
//using Polarities.NPCs.RiftDenizen;
//using Polarities.NPCs.TownNPCs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace Polarities
{
    public class PolaritiesSystem : ModSystem
    {
        public static bool downedStormCloudfish;
        public static bool downedStarConstruct;
        public static bool downedGigabat;
        public static bool downedRiftDenizen;
        public static bool downedSunPixie;
        public static bool downedEsophage;
        public static bool downedConvectiveWanderer;
        public static bool downedSelfsimilarSentinel;
        public static bool downedEclipxie;
        public static bool downedHemorrphage;
        public static bool downedPolarities;

        public static bool downedEaterOfWorlds;
        public static bool downedBrainOfCthulhu;

        public static bool hallowInvasion;
        public static bool downedHallowInvasion;
        public static int sunPixieSpawnTimer;
        public static int hallowInvasionSize;
        public static int hallowInvasionSizeStart;

        public static bool worldEvilInvasion;
        public static bool downedWorldEvilInvasion;
        public static int esophageSpawnTimer;
        public static int worldEvilInvasionSize;
        public static int worldEvilInvasionSizeStart;

        public static bool disabledEvilSpread;
        public static bool disabledHallowSpread;

        public static int convectiveWandererSpawnTimer;

        public static int timer;

        public override void OnWorldLoad()
        {
            //DrawCacheProjsBehindWalls.Clear();
            //sentinelCaves?.Clear();

            downedStormCloudfish = false;
            downedStarConstruct = false;
            downedGigabat = false;
            downedRiftDenizen = false;
            downedSunPixie = false;
            downedEsophage = false;
            downedSelfsimilarSentinel = false;
            downedConvectiveWanderer = false;
            downedEclipxie = false;
            downedHemorrphage = false;
            downedPolarities = false;

            downedEaterOfWorlds = false;
            downedBrainOfCthulhu = false;

            hallowInvasion = false;
            downedHallowInvasion = false;
            sunPixieSpawnTimer = 0;
            hallowInvasionSize = 0;
            hallowInvasionSizeStart = 0;

            worldEvilInvasion = false;
            downedWorldEvilInvasion = false;
            esophageSpawnTimer = 0;
            worldEvilInvasionSize = 0;
            worldEvilInvasionSizeStart = 0;

            disabledEvilSpread = false;
            disabledHallowSpread = false;

            convectiveWandererSpawnTimer = 0;
        }

        public override void OnWorldUnload()
        {
            //DrawCacheProjsBehindWalls.Clear();
            //sentinelCaves?.Clear();

            downedStormCloudfish = false;
            downedStarConstruct = false;
            downedGigabat = false;
            downedRiftDenizen = false;
            downedSunPixie = false;
            downedEsophage = false;
            downedConvectiveWanderer = false;
            downedSelfsimilarSentinel = false;
            downedEclipxie = false;
            downedHemorrphage = false;
            downedPolarities = false;

            downedEaterOfWorlds = false;
            downedBrainOfCthulhu = false;

            hallowInvasion = false;
            downedHallowInvasion = false;
            sunPixieSpawnTimer = 0;
            hallowInvasionSize = 0;
            hallowInvasionSizeStart = 0;

            worldEvilInvasion = false;
            downedWorldEvilInvasion = false;
            esophageSpawnTimer = 0;
            worldEvilInvasionSize = 0;
            worldEvilInvasionSizeStart = 0;

            disabledEvilSpread = false;
            disabledHallowSpread = false;

            convectiveWandererSpawnTimer = 0;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedStormCloudfish) tag["downedStormCloudfish"] = true;
            if (downedStarConstruct) tag["downedStarConstruct"] = true;
            if (downedGigabat) tag["downedGigabat"] = true;
            if (downedRiftDenizen) tag["downedRiftDenizen"] = true;
            if (downedSunPixie) tag["downedSunPixie"] = true;
            if (downedEsophage) tag["downedEsophage"] = true;
            if (downedConvectiveWanderer) tag["downedConvectiveWanderer"] = true;
            if (downedSelfsimilarSentinel) tag["downedSelfsimilarSentinel"] = true;
            if (downedEclipxie) tag["downedEclipxie"] = true;
            if (downedHemorrphage) tag["downedHemorrphage"] = true;
            if (downedPolarities) tag["downedPolarities"] = true;

            if (downedEaterOfWorlds) tag["downedEaterOfWorlds"] = true;
            if (downedBrainOfCthulhu) tag["downedBrainOfCthulhu"] = true;

            if (hallowInvasion) tag["hallowInvasion"] = true;
            if (downedHallowInvasion) tag["downedHallowInvasion"] = true;
            tag["hallowInvasionSize"] = hallowInvasionSize;
            tag["hallowInvasionSizeStart"] = hallowInvasionSizeStart;

            if (worldEvilInvasion) tag["worldEvilInvasion"] = true;
            if (downedWorldEvilInvasion) tag["downedWorldEvilInvasion"] = true;
            tag["worldEvilInvasionSize"] = worldEvilInvasionSize;
            tag["worldEvilInvasionSizeStart"] = worldEvilInvasionSizeStart;

            if (disabledEvilSpread) tag["disabledEvilSpread"] = true;
            if (disabledHallowSpread) tag["disabledHallowSpread"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedStormCloudfish = tag.ContainsKey("downedStormCloudfish");
            downedStarConstruct = tag.ContainsKey("downedStarConstruct");
            downedGigabat = tag.ContainsKey("downedGigabat");
            downedRiftDenizen = tag.ContainsKey("downedRiftDenizen");
            downedSunPixie = tag.ContainsKey("downedSunPixie");
            downedEsophage = tag.ContainsKey("downedEsophage");
            downedConvectiveWanderer = tag.ContainsKey("downedConvectiveWanderer");
            downedSelfsimilarSentinel = tag.ContainsKey("downedSelfsimilarSentinel");
            downedEclipxie = tag.ContainsKey("downedEclipxie");
            downedHemorrphage = tag.ContainsKey("downedHemorrphage");
            downedPolarities = tag.ContainsKey("downedPolarities");

            downedEaterOfWorlds = tag.ContainsKey("downedEaterOfWorlds");
            downedBrainOfCthulhu = tag.ContainsKey("downedBrainOfCthulhu");

            hallowInvasion = tag.ContainsKey("hallowInvasion");
            downedHallowInvasion = tag.ContainsKey("downedHallowInvasion");
            hallowInvasionSize = tag.ContainsKey("hallowInvasionSize") ? tag.GetAsInt("hallowInvasionSize") : 0;
            hallowInvasionSizeStart = tag.ContainsKey("hallowInvasionSizeStart") ? tag.GetAsInt("hallowInvasionSizeStart") : 0;

            worldEvilInvasion = tag.ContainsKey("worldEvilInvasion");
            downedWorldEvilInvasion = tag.ContainsKey("downedWorldEvilInvasion");
            worldEvilInvasionSize = tag.ContainsKey("worldEvilInvasionSize") ? tag.GetAsInt("worldEvilInvasionSize") : 0;
            worldEvilInvasionSizeStart = tag.ContainsKey("worldEvilInvasionSizeStart") ? tag.GetAsInt("worldEvilInvasionSizeStart") : 0;

            disabledHallowSpread = tag.ContainsKey("disabledHallowSpread");
            disabledEvilSpread = tag.ContainsKey("disabledEvilSpread");
        }

        public static bool timeAccelerate = true;
        private float timeRateMultiplier;
        public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
        {
            if (!timeAccelerate)
            {
                timeRateMultiplier = 1f;
            }
            else
            {
                timeRateMultiplier += 1 / 5f;
            }
            timeAccelerate = false;

            timeRate *= timeRateMultiplier;
        }
    }
}