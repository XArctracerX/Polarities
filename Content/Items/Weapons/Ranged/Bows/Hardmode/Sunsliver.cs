using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Bows.Hardmode
{
	public class Sunsliver : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sunsliver");
			// Tooltip.SetDefault("Fires streams of arrows at varying speeds");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 56;
			Item.useTime = 10;
			Item.useAnimation = 30;
			Item.reuseDelay = 20;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 80000;
			Item.rare = 8;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 5f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			SoundEngine.PlaySound(SoundID.Item5, position);
			velocity *= 3;
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ProjectileType<EclipseArrow>();
			}
		}
	}

	public class EclipseArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 7;
			Projectile.height = 19;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
			//Projectile.extraUpdates = 1;
		}

		Vector2 ogVel = Vector2.Zero;
		public override void AI()
		{
			if (Projectile.ai[0] == 0) ogVel = Projectile.velocity;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 2;
			Projectile.velocity *= 0.95f;

			if (Projectile.ai[0]++ > 60)
			{
				Projectile.Kill();
			}
		}

		public override void OnKill(int timeLeft)
		{
			Split();
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			base.OnKill(timeLeft);
		}

		public void Split()
		{
			IEntitySource source = Projectile.GetSource_FromThis();
			Projectile.NewProjectileDirect(source, Projectile.position, ogVel, ProjectileType<SunArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			for (int i = -2; i < 3; i++)
			{
				if (i == 0) continue;
				float angle = MathHelper.ToRadians(10 * i);
				Projectile.NewProjectileDirect(source, Projectile.position, ogVel.RotatedBy(angle), ProjectileType<MoonArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float drawAlpha = 1f;
			//if (Projectile.timeLeft < FADE_TIME) drawAlpha = Projectile.timeLeft / (float)FADE_TIME;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 center = frame.Center();

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] != Vector2.Zero)
				{
					float progress = 1 - i / (float)Projectile.oldPos.Length;
					Vector2 scale = new Vector2(progress, progress);
					Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White, Projectile.oldRot[i], center, scale, SpriteEffects.None, 0);
				}
			}

			Texture2D mainTexture = TextureAssets.Projectile[Type].Value;
			Rectangle mainFrame = mainTexture.Frame();
			Vector2 mainCenter = mainFrame.Center();

			Main.EntitySpriteDraw(mainTexture, Projectile.Center - Main.screenPosition, mainFrame, Color.White * drawAlpha, Projectile.rotation, mainCenter, Projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}

	public class SunArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 7;
			Projectile.height = 19;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
			Projectile.light = 1f;
			Projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire3, 300);
			base.OnHitNPC(target, hit, damageDone);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.OnFire3, 300);
			base.OnHitPlayer(target, info);
		}

		public override void OnKill(System.Int32 timeLeft)
		{
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileID.DaybreakExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner);
			base.OnKill(timeLeft);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float drawAlpha = 1f;
			//if (Projectile.timeLeft < FADE_TIME) drawAlpha = Projectile.timeLeft / (float)FADE_TIME;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 center = frame.Center();

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] != Vector2.Zero)
				{
					float progress = 1 - i / (float)Projectile.oldPos.Length;
					Vector2 scale = new Vector2(progress, progress);
					Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White, Projectile.oldRot[i], center, scale, SpriteEffects.None, 0);
				}
			}

			Texture2D mainTexture = TextureAssets.Projectile[Type].Value;
			Rectangle mainFrame = mainTexture.Frame();
			Vector2 mainCenter = mainFrame.Center();

			Main.EntitySpriteDraw(mainTexture, Projectile.Center - Main.screenPosition, mainFrame, Color.White * drawAlpha, Projectile.rotation, mainCenter, Projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}

	public class MoonArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 7;
			Projectile.height = 19;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
			Projectile.light = 1f;
			Projectile.extraUpdates = 1;

			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
		{
			target.AddBuff(BuffID.Frostburn2, 300);
			base.OnHitNPC(target, hit, damageDone);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.Frostburn2, 300);
			base.OnHitPlayer(target, info);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float drawAlpha = 1f;
			//if (Projectile.timeLeft < FADE_TIME) drawAlpha = Projectile.timeLeft / (float)FADE_TIME;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 center = frame.Center();

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] != Vector2.Zero)
				{
					float progress = 1 - i / (float)Projectile.oldPos.Length;
					Vector2 scale = new Vector2(progress, progress);
					Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White, Projectile.oldRot[i], center, scale, SpriteEffects.None, 0);
				}
			}

			Texture2D mainTexture = TextureAssets.Projectile[Type].Value;
			Rectangle mainFrame = mainTexture.Frame();
			Vector2 mainCenter = mainFrame.Center();

			Main.EntitySpriteDraw(mainTexture, Projectile.Center - Main.screenPosition, mainFrame, Color.White * drawAlpha, Projectile.rotation, mainCenter, Projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}