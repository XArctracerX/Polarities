using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
	public class FractalEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Fractal Eye");
			//Tooltip.SetDefault("Summons a ring of fractal eyes around you, filling up your available minion slots");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Summon;
			Item.damage = 30;
			Item.width = 30;
			Item.height = 30;
			Item.accessory = true;
			Item.value = 100000;
			Item.rare = 8;
		}

		int timer = 1;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for (int i = 0; i < player.maxMinions - player.slotsMinions; i++)
            {
				Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<FractalEyeProjectile>(), Item.damage, 0, player.whoAmI);
			}
		}
	}

	public class FractalEyeProjectile : ModProjectile
	{
		//public override string Texture => "Polarities/Content/Items/Accessories/Combat/Offense/Hardmode/FractalEye";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Eye");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		float distance;
		float speed;

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			DrawOffsetX = 0;
			DrawOriginOffsetX = 0;
			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			distance = Main.rand.NextFloat(0.75f, 1.25f);
			speed = Main.rand.NextFloat(0.75f, 1.25f);
		}

		int timer = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (Core.ModUtils.Contains(player.armor, ItemType<FractalEye>())) Projectile.timeLeft = 2;

			if (player.slotsMinions + Projectile.minionSlots <= player.maxMinions)
            {
				Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<FractalEyeProjectile>(), Projectile.damage, 0, player.whoAmI);
			}


			if ((Projectile.Center - player.Center).Length() > 1000)
			{
				Projectile.Kill();
			}

			int count = 1;
			if (player.ownedProjectileCounts[Projectile.type] > count) count = player.ownedProjectileCounts[Projectile.type];
			Projectile.ai[0] += 0.1f / count;

			int index = 0;
			for (int i = 0; i < Projectile.whoAmI; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
				{
					index++;

					//sync timers
					Projectile.ai[0] = Main.projectile[i].ai[0];
				}
			}

			Vector2 goalPos = player.Center + new Vector2(0, 60 * (1 + count / 10f) * distance).RotatedBy((Projectile.ai[0] * speed) + (MathHelper.TwoPi * index) / count);
			Vector2 goalVel = goalPos - Projectile.Center;
			if (goalVel.Length() > 6)
			{
				goalVel.Normalize();
				goalVel *= 6;
			}
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, goalVel + player.position - player.oldPosition, 0.2f);

			if (Main.mouseLeft && Main.myPlayer == Projectile.owner && Projectile.localAI[0] != 0)
			{
				if (timer > Projectile.ai[1] + (int)(40 * (1 / speed)))
				{
					Projectile.ai[1] = timer;
				}
				if (timer % (int)(40 * (1 / speed)) == Projectile.ai[1] % (int)(40 * (1 / speed)))
				{
					Projectile.ai[1] = timer;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 16, ProjectileType<Weapons.Summon.Minions.Hardmode.FractalFrondFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
			timer++;

			if (Projectile.localAI[0] == 0)
			{
				Projectile.localAI[0] = 1;
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return false;
		}

		public override bool PreDraw(ref Color drawColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D pupil = Request<Texture2D>("Polarities/Content/Items/Accessories/Combat/Offense/Hardmode/FractalEyePupil").Value;
			Vector2 drawOrigin = new Vector2(3, 3);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Vector2 drawOffset = Vector2.Zero;

			Vector2 eyeDrawOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);

			if (Main.mouseLeft)
			{
				drawOffset = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 4;
			}
			Main.spriteBatch.Draw(texture, Projectile.Center + eyeDrawOffset - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), drawColor, Projectile.rotation, new Vector2(32, 32), Projectile.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(pupil, drawPos + drawOffset, new Rectangle(0, 0, 6, 6), drawColor, 0, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class FractalEyeLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 127;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 999999;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 127;
			Projectile.light = 1f;
		}

		public override void AI()
		{
			//Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, newColor: Color.LightBlue, Scale: 1f)].noGravity = true;

			Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;

			Projectile.velocity.Y += 0.1f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 120;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor = Color.White;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float scale = Projectile.scale;

				float rotation = Projectile.rotation;

				Main.spriteBatch.Draw(texture, Projectile.Center + (-Projectile.position + Projectile.oldPos[k]) / 2f - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}