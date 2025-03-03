using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Polarities.Global;
//using Polarities.Items;
//using Polarities.NPCs;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Core
{
    public static class ModUtils
    {
        public static FlavorTextBestiaryInfoElement TranslatedBestiaryEntry(this ModNPC modNPC)
        {
            return new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Polarities.Bestiary." + modNPC.GetType().Name));
        }

        public static IItemDropRule MasterModeDropOnAllPlayersOrFlawless(int Type, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1)
        {
            return new DropBasedOnMasterMode(ItemDropRule.ByCondition(new FlawlessDropCondition(), Type, amountDroppedMinimum, amountDroppedMaximum), new FlawlessOrRandomDropRule(Type, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator));
        }

        //public static int GetFractalization(this Player player)
        //{
            //return player.Polarities().GetFractalization();
        //}

        public static PolaritiesPlayer Polarities(this Player player)
        {
            return player.GetModPlayer<PolaritiesPlayer>();
        }

        public static Color ColorLerpCycle(float time, float cycleTime, params Color[] colors)
        {
            if (colors.Length == 0) return default(Color);

            int index = (int)(time / cycleTime * colors.Length) % colors.Length;
            float lerpAmount = time / cycleTime * colors.Length % 1;

            return Color.Lerp(colors[index], colors[(index + 1) % colors.Length], lerpAmount);
        }
    }
}