using Polarities.Content.Buffs.Hardmode;
using Polarities.Core;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace Polarities
{
    public partial class PolaritiesPlayer : ModPlayer
    {
        public bool hasBeenInFractalDimension;

        public int fractalization;

        public int fractalSubworldDebuffRate = 3;
        public int fractalSubworldDebuffResistance;
        public float fractalSubworldDebuffLifeLossResistance = 1f;
        public int fractalSubworldDebuffIgnoreTicks;
        public float fractalLifeMaxMultiplier = 1f;
        public bool suddenFractalizationChange;
        public int originalStatLifeMax2;
        public bool noFractalAntidote;

        public bool fractalDimensionRespawn = false;

        public void UpdateFractalizationTimer()
        {

        }

        public void UpdateFractalHP()
        {
            int fractalization = Math.Max(0, Player.GetFractalization() - fractalSubworldDebuffIgnoreTicks);
            if (fractalization > 0)
            {
                float fractalizationKillTime = 18000 * fractalSubworldDebuffLifeLossResistance;

                float goalLifeMaxMultiplier = Math.Min(1, 1f - (fractalization - fractalSubworldDebuffResistance) / fractalizationKillTime);

                float maxMultiplierChange = 0.0025f;
                if (fractalLifeMaxMultiplier > goalLifeMaxMultiplier + maxMultiplierChange && !suddenFractalizationChange)
                {
                    fractalLifeMaxMultiplier -= maxMultiplierChange;
                }
                else
                {
                    fractalLifeMaxMultiplier = goalLifeMaxMultiplier;
                }

                Player.statLifeMax2 = Math.Max(1, (int)Math.Ceiling(Player.statLifeMax2 * fractalLifeMaxMultiplier));

                if (fractalLifeMaxMultiplier <= 0)
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + "'s physics broke."), 1.0, 0, false);
                }
            }
            else
            {
                fractalLifeMaxMultiplier = 1f;
            }
            fractalSubworldDebuffResistance = 0;
            fractalSubworldDebuffLifeLossResistance = 1f;
        }
    }
}