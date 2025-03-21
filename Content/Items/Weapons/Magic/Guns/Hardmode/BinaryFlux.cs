using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Guns.Hardmode
{
	public class BinaryFlux : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Binary Flux");
			//Tooltip.SetDefault("Shoots an irregularly-spaced row of energy vortices");
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 36;
			Item.height = 22;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item61;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<BinaryFluxShot>();
			Item.shootSpeed = 16f;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.rand.Next(65536));
			return false;
        }
    }

	public class BinaryFluxShot : ModProjectile
	{
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Orthogonal Energy");
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 2;
			Projectile.height = 2;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;

			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.extraUpdates = 7;
			Projectile.timeLeft = 360;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] == 2) {
				Projectile.ai[1] = 0;

				int i = (int)Projectile.ai[0];
				int x = 0;
				for (int a = 0; a < 32; a++)
				{
					x = x ^ ((i >> a) & 1);
				}
				Projectile.ai[0]++;

				if (x == 0)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<BinaryFluxProjectile>(), Projectile.damage, 0f, Projectile.owner, Projectile.velocity.X > 0 ? 1 : -1);
                }
			}
		}
	}

	public class BinaryFluxProjectile : ModProjectile
	{
		public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/SunPixie/SunPixieArena";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Energy Vortex");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 32;
			Projectile.height = 32;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;

			Projectile.alpha = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;

			Projectile.scale = 0.75f;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 0.75f)].noGravity = true;

			Projectile.frameCounter++;
			Projectile.spriteDirection = (int)Projectile.ai[0];
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int numDraws = 12;
			for (int i = 0; i < numDraws; i++)
			{
				float scale = (1 + (float)Math.Sin(Projectile.frameCounter * 0.1f + (MathHelper.TwoPi * i) / numDraws)) / 2f;
				Color color = new Color((int)(82 * scale + 512 * (1 - scale)), (int)(179 * scale + 512 * (1 - scale)), (int)(203 * scale + 512 * (1 - scale)));
				float alpha = 0.2f;
				float rotation = Projectile.frameCounter * 0.2f;

				if (Projectile.timeLeft < 30)
				{
					scale *= Projectile.timeLeft / 30f;
				}

				Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 64, 64), color * alpha, rotation, new Vector2(32, 32), Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			for (int i = 0; i < numDraws; i++)
			{
				float scale = (1 + (float)Math.Sin(Projectile.frameCounter * 0.1f + (MathHelper.TwoPi * i) / numDraws)) / 2f;
				scale *= 0.75f;
				Color color = new Color((int)(82 * scale + 512 * (1 - scale)), (int)(179 * scale + 512 * (1 - scale)), (int)(203 * scale + 512 * (1 - scale)));
				float alpha = 0.2f;
				float rotation = Projectile.frameCounter * 0.3f;

				if (Projectile.timeLeft < 45)
				{
					scale *= (Projectile.timeLeft - 15) / 30f;
				}
				if (Projectile.timeLeft > 15)
				{
					Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 64, 64), color * alpha, rotation, new Vector2(32, 32), Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 1f);
			}
		}
	}
}