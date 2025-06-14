using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Polarities.Effects;
using Polarities.Core;
using Polarities.Global;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Accessories.ExpertMode.Hardmode;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Books.Hardmode
{
    public class BeltramiElementsSystem : ModSystem
    {
        public override void Load()
        {
            On_Player.PickTile += MiningWisp;
        }

        public static int brokenBlocks = 0;
        public static void MiningWisp(On_Player.orig_PickTile orig, Player player, int x, int y, int pickPower)
        {
            if (player.HasBuff(BuffType<SelfsimilarSentinelBookBuff>()))
            {
                if (brokenBlocks++ >= 3)
                {
                    brokenBlocks = 0;
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, new Vector2(Main.rand.NextFloat(1, 4), 0).RotatedByRandom(MathHelper.TwoPi), ProjectileType<SentinelHeartWisp>(), 40, 2f, player.whoAmI, ai1: Main.rand.Next(1000));
                }
            }
            orig(player, x, y, pickPower);
        }
    }

    public class SelfsimilarSentinelBook : BookBase
    {
        public override int BuffType => BuffType<SelfsimilarSentinelBookBuff>();
        public override int BookIndex => 22;
    }

    public class SelfsimilarSentinelBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<SelfsimilarSentinelBook>();

        private static int trailTimer;

        public override void Update(Player player, ref int buffIndex)
        {

            base.Update(player, ref buffIndex);
        }
    }
}