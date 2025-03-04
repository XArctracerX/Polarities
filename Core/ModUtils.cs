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
        public static void SetMerge(int type1, int type2, bool merge = true)
        {
            if (type1 != type2)
            {
                Main.tileMerge[type1][type2] = merge;
                Main.tileMerge[type2][type1] = merge;
            }
        }
        public static void SetMerge<T>(int type2, bool merge = true) where T : ModTile
        {
            SetMerge(ModContent.TileType<T>(), type2, merge);
        }
        public static void SetMerge<T, T2>(int type2, bool merge = true) where T : ModTile where T2 : ModTile
        {
            SetMerge(ModContent.TileType<T>(), ModContent.TileType<T2>(), merge);
        }
        public static void SetMerge(this ModTile modTile, int type2, bool merge = true)
        {
            SetMerge(modTile.Type, type2, merge);
        }
        public static void SetMerge<T2>(this ModTile modTile, bool merge = true) where T2 : ModTile
        {
            SetMerge(modTile.Type, ModContent.TileType<T2>(), merge);
        }

        public static void SetModBiome<T, T2, T3>(this ModNPC modNPC) where T : ModBiome where T2 : ModBiome where T3 : ModBiome
        {
            modNPC.SpawnModBiomes = new int[] { ModContent.GetInstance<T>().Type, ModContent.GetInstance<T2>().Type, ModContent.GetInstance<T3>().Type };
        }
        public static void SetModBiome<T, T2>(this ModNPC modNPC) where T : ModBiome where T2 : ModBiome
        {
            modNPC.SpawnModBiomes = new int[] { ModContent.GetInstance<T>().Type, ModContent.GetInstance<T2>().Type, };
        }
        public static void SetModBiome<T>(this ModNPC modNPC) where T : ModBiome
        {
            modNPC.SpawnModBiomes = new int[] { ModContent.GetInstance<T>().Type, };
        }

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

        public static int SpawnSentry(this Player player, IEntitySource source, int ownerIndex, int sentryProjectileId, int originalDamageNotScaledByMinionDamage, float KnockBack, bool spawnWithGravity = true, Vector2 offsetFromDefaultPosition = default)
        {
            int num5 = (int)(Main.mouseX + Main.screenPosition.X) / 16;
            int num6 = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
            if (player.gravDir == -1f)
            {
                num6 = (int)(Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16;
            }
            if (spawnWithGravity)
            {
                for (; num6 < Main.maxTilesY - 10 && Main.tile[num5, num6] != null && !WorldGen.SolidTile2(num5, num6) && Main.tile[num5 - 1, num6] != null && !WorldGen.SolidTile2(num5 - 1, num6) && Main.tile[num5 + 1, num6] != null && !WorldGen.SolidTile2(num5 + 1, num6); num6++)
                {
                }
                num6--;
            }
            int num7 = Projectile.NewProjectile(source, Main.mouseX + Main.screenPosition.X + offsetFromDefaultPosition.X, num6 * 16 - 24 + offsetFromDefaultPosition.Y, 0f, spawnWithGravity ? 15f : 0f, sentryProjectileId, originalDamageNotScaledByMinionDamage, KnockBack, ownerIndex);
            Main.projectile[num7].originalDamage = originalDamageNotScaledByMinionDamage;
            player.UpdateMaxTurrets();
            return num7;
        }

        public static bool IsTypeSummon(this Projectile projectile)
        {
            return projectile.minion || projectile.sentry || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type] || projectile.DamageType == DamageClass.Summon || projectile.DamageType.GetEffectInheritance(DamageClass.Summon);
        }

        public static Color ColorLerpCycle(float time, float cycleTime, params Color[] colors)
        {
            if (colors.Length == 0) return default(Color);

            int index = (int)(time / cycleTime * colors.Length) % colors.Length;
            float lerpAmount = time / cycleTime * colors.Length % 1;

            return Color.Lerp(colors[index], colors[(index + 1) % colors.Length], lerpAmount);
        }

        public static float Lerp(float x, float y, float progress)
        {
            return x * (1 - progress) + y * progress;
        }

        public static Vector2 BezierCurve(Vector2[] bezierPoints, float bezierProgress)
        {
            if (bezierPoints.Length == 1)
            {
                return bezierPoints[0];
            }
            else
            {
                Vector2[] newBezierPoints = new Vector2[bezierPoints.Length - 1];
                for (int i = 0; i < bezierPoints.Length - 1; i++)
                {
                    newBezierPoints[i] = bezierPoints[i] * bezierProgress + bezierPoints[i + 1] * (1 - bezierProgress);
                }
                return BezierCurve(newBezierPoints, bezierProgress);
            }
        }

        public static Vector2 BezierCurveDerivative(Vector2[] bezierPoints, float bezierProgress)
        {
            if (bezierPoints.Length == 2)
            {
                return bezierPoints[0] - bezierPoints[1];
            }
            else
            {
                Vector2[] newBezierPoints = new Vector2[bezierPoints.Length - 1];
                for (int i = 0; i < bezierPoints.Length - 1; i++)
                {
                    newBezierPoints[i] = bezierPoints[i] * bezierProgress + bezierPoints[i + 1] * (1 - bezierProgress);
                }
                return BezierCurveDerivative(newBezierPoints, bezierProgress);
            }
        }

    }
}