using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Projectiles;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Summon.Minions.Hardmode
{
	public class HemorrphminiBody : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Hemorrphmini");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			DrawOffsetX = 0;
			DrawOriginOffsetY = -11;
			DrawOriginOffsetX = 0;

			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (player.dead)
			{
				player.ClearBuff(BuffType<HemorrphminiBuff>());
			}
			if (player.HasBuff(BuffType<HemorrphminiBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			int targetID = -1;
            Projectile.Minion_FindTargetInRange(750, ref targetID, true);
            NPC target = null;
            if (targetID != -1)
            {
                target = Main.npc[targetID];
            } 

			Vector2 targetPosition = player.Center - new Vector2(player.direction * 64, 64);
			Vector2 targetVelocity = player.velocity;

			if (target != null)
			{
				targetPosition = target.Center - new Vector2(0, 84);
				targetVelocity = target.velocity;

				if (player.ownedProjectileCounts[ProjectileType<HemorrphminiLeg>()] >= 6 && (targetPosition - Projectile.Center).Length() < 144)
				{
					Projectile.ai[0]++;
					if (Main.netMode != 1 && Projectile.ai[0] % 10 == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 8f), 8), new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 4), ProjectileType<BloodDropHemorrphmini>(), 1, 0, Projectile.owner);
					}
					if (Projectile.ai[0] % 10 == 0)
					{
						SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
					}
				}
				else
				{
					Projectile.ai[0] = 0;
				}
			}
			else
			{
				Projectile.ai[0] = 0;
			}

			Vector2 goalVelocity = targetVelocity + (targetPosition - Projectile.Center) / 15;
			Projectile.velocity += (goalVelocity - Projectile.velocity) / 15;

			Projectile.rotation = Projectile.velocity.X * 0.08f;
			//Projectile.frame = Projectile.velocity.X > 0 ? 0 : 1;

			if ((Projectile.Center - player.Center).Length() > 1600)
			{
				Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
				Projectile.velocity = Vector2.Zero;
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
	}

	public class HemorrphminiLeg : ModProjectile
	{
		private int attackCooldown;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Hemorrphmini Claw");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			DrawOffsetX = -2;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;

			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.minionSlots = 0.75f;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (player.dead)
			{
				player.ClearBuff(BuffType<HemorrphminiBuff>());
			}
			if (player.HasBuff(BuffType<HemorrphminiBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Projectile body = Main.projectile[(int)Projectile.ai[0]];

			Projectile.velocity.Y += 0.3f;

			if ((Projectile.Center + new Vector2(0,-36) - body.Center).Length() > 120)
			{
				Projectile.velocity = body.velocity + (body.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 7 + new Vector2(2, 0).RotatedByRandom(Math.PI);
			}

			if (attackCooldown > 0)
			{
				attackCooldown--;
			}

			if ((body.Center - player.Center).Length() > 1600 || (body.Center - Projectile.Center).Length() > 800)
			{
				Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
				Projectile.velocity = Vector2.Zero;
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return attackCooldown == 0;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			attackCooldown = 10;
		}

		public override bool PreDraw(ref Color drawColor)
		{
			Projectile body = Main.projectile[(int)Projectile.ai[0]];
			Texture2D legTexture = Request<Texture2D>("Polarities/Content/Items/Weapons/Summon/Minions/Hardmode/HemorrphminiLegChain").Value;

			Vector2 mainCenter = body.Center;
			Vector2 center = Projectile.Center + new Vector2(0, -36);
			Vector2 intermediateCenter = center + (mainCenter - Projectile.Center) / 2;


			Vector2 distToNPC = intermediateCenter - center;
			float projRotation = distToNPC.ToRotation() + MathHelper.PiOver2;
			float distance = distToNPC.Length();

			int tries = 100;
			while (distance > 6f && !float.IsNaN(distance) && tries > 0)
			{
				tries--;
				//Draw chain
				Main.spriteBatch.Draw(legTexture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 10, 6), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), projRotation,
					new Vector2(10 * 0.5f, 6 * 0.5f), 1f, SpriteEffects.None, 0f);

				distToNPC.Normalize();                 //get unit vector
				distToNPC *= 6f;                      //speed = 24
				center += distToNPC;                   //update draw position
				distToNPC = intermediateCenter - center;    //update distance
				distance = distToNPC.Length();
			}
			Main.spriteBatch.Draw(legTexture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
				new Rectangle(0, 0, 10, 6), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), projRotation,
				new Vector2(10 * 0.5f, 6 * 0.5f), 1f, SpriteEffects.None, 0f);

			center = intermediateCenter;

			distToNPC = mainCenter - center;
			projRotation = distToNPC.ToRotation() + MathHelper.PiOver2;
			distance = distToNPC.Length();

			tries = 100;
			while (distance > 6f && !float.IsNaN(distance) && tries > 0)
			{
				tries--;
				//Draw chain
				Main.spriteBatch.Draw(legTexture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 10, 6), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), projRotation,
					new Vector2(10 * 0.5f, 6 * 0.5f), 1f, SpriteEffects.None, 0f);

				distToNPC.Normalize();                 //get unit vector
				distToNPC *= 6f;                      //speed = 24
				center += distToNPC;                   //update draw position
				distToNPC = mainCenter - center;    //update distance
				distance = distToNPC.Length();
			}

			for (int i = 1; i <= 6; i++)
			{
				center = Projectile.Center + new Vector2(0, -i * 6);
				Main.spriteBatch.Draw(legTexture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 10, 6), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), 0,
					new Vector2(10 * 0.5f, 6 * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindProjectiles.Add(index);
		}
	}

	public class BloodDropHemorrphmini : ModProjectile
	{
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Blood Drop");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.alpha = 0;
			Projectile.timeLeft = 20;
			Projectile.penetrate = -1;
			Projectile.hide = true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center - new Vector2(3, 3), 0, 0, DustID.Blood, Scale: 1.4f);
			Main.dust[dust].velocity = Main.rand.NextFloat(0.5f, 1f) * Projectile.velocity + new Vector2(Main.rand.NextFloat(0f, 1f), 0).RotatedByRandom(MathHelper.Pi);
			Main.dust[dust].noGravity = true;
			dust = Dust.NewDust(Projectile.Center - new Vector2(3, 3), 0, 0, DustID.Blood, Scale: 1.4f);
			Main.dust[dust].velocity = Main.rand.NextFloat(0.5f, 1f) * Projectile.velocity + new Vector2(Main.rand.NextFloat(0f, 1f), 0).RotatedByRandom(MathHelper.Pi);
			Main.dust[dust].noGravity = true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CheckAABBvAABBCollision(target.position, new Vector2(target.width, target.height), Projectile.position, new Vector2(Projectile.width, Projectile.height)))
			{
				target.AddBuff(BuffID.Bleeding, 120, true);
			}
			return false;
		}
	}
}