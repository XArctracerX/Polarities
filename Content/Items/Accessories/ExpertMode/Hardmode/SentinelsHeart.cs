using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Polarities.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.ExpertMode.Hardmode
{
	public class SentinelsHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sentinel's Heart");
			// Tooltip.SetDefault("Grants the player additional health, and a boost to defense while this additional health is retained\nWhen this additional health is lost, releases homing wisps to damage enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 46;

			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 = (int)(1.12f * player.statLifeMax2);
			player.GetModPlayer<PolaritiesPlayer>().hasSentinelHearts = true;
		}

		public override void UpdateInventory(Player player)
		{
			Item.rare = ItemRarityID.Expert;
		}
	}

	public class SentinelHeartWisp : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.RainbowCrystalExplosion;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Homing Wisp");

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 10;
			Projectile.height = 10;

			Projectile.alpha = 0;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Projectile.velocity = Projectile.velocity.RotatedBy((float)Math.Sqrt(Projectile.velocity.Length()) * (0.02f * (float)Math.Cos(0.1f * Projectile.ai[0] + Projectile.ai[1])));

			//only home if ai[0] >= 30
			if (Projectile.ai[0] >= 30)
			{
                int targetID = -1;
                Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
                if (targetID != -1)
				{
					NPC target = Main.npc[targetID];

					float homingAmount = 12f;

					Vector2 goalPosition = target.Center;
					Vector2 goalVelocity = homingAmount * (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero);
					Projectile.velocity += (goalVelocity - Projectile.velocity) / 60f;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			Projectile.ai[0]++;
		}

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
        {
			return Projectile.ai[0] >= 30;
        }

        public override bool? CanCutTiles()
        {
			return false;
        }

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 center = frame.Center();

			const float scaleMultiplier = 0.3f;

			//only draw large enough segments
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				float progress = 4 * (1 - i / (float)Projectile.oldPos.Length) * (i / (float)Projectile.oldPos.Length);
				Color color = new Color((int)(243 + 8 * progress), (int)(112 + 102 * progress), 255); //12, 143
				Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.position - Projectile.Center - Main.screenPosition, frame, color, Projectile.oldRot[i] + MathHelper.PiOver2, center, new Vector2(progress * Projectile.scale, 1) * scaleMultiplier, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}