using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
    public class LightningCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.damage = 30;
            Item.width = 11;
            Item.height = 22;
            Item.accessory = true;
            Item.value = 12500;
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PolaritiesPlayer>().lightningCore = true;
        }
    }

    public class LightningCoreMinion : ModProjectile
	{
		private bool leaving;
		private int atkCooldown;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lightningfish");
			Main.projFrames[Projectile.type] = 7;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			DrawOffsetX = -12;
			DrawOriginOffsetX = 7;
			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.minionSlots = 0.25f;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			if (Projectile.velocity.X > 0)
			{
				Projectile.rotation = (float)Math.Atan(Projectile.velocity.Y / Projectile.velocity.X);
			}
			else
			{
				Projectile.rotation = (float)(Math.PI + Math.Atan(Projectile.velocity.Y / Projectile.velocity.X));
			}

			atkCooldown -= 1;


			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (player.dead)
			{
				player.GetModPlayer<PolaritiesPlayer>().lightningCore = false;
			}
			if (player.GetModPlayer<PolaritiesPlayer>().lightningCore)
			{
				leaving = false;
				Projectile.timeLeft = 120;
				Projectile.tileCollide = true;
			}
			else if ((Main.rand.NextBool(300) || leaving) && Main.myPlayer == Projectile.owner)
			{
				leaving = true;
				Projectile.tileCollide = false;
				Projectile.velocity.Y -= 0.2f;

				Projectile.netUpdate = true;

				return;
			}
			else if (!leaving)
			{
				Projectile.timeLeft = 120;
			}

			if (Math.Sqrt((Projectile.Center.X - player.Center.X) * (Projectile.Center.X - player.Center.X) + (Projectile.Center.Y - player.Center.Y) * (Projectile.Center.Y - player.Center.Y)) > 2400)
			{
				Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);

				float length = (Projectile.position - Projectile.oldPosition).Length();
				for (int i = 0; i < length; i += 8)
				{
					Dust.NewDustPerfect((Projectile.position * i + Projectile.oldPosition * (length - i)) / length, DustID.Electric, Scale: 0.75f);
				}
			}

			int targetID = -1;
			Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
			NPC target = null;
			if (targetID != -1)
			{
				target = Main.npc[targetID];
			}
			if (Main.rand.NextBool(240) && Main.netMode != 1)
			{
				targetID = -1;
				Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
				if (targetID != -1)
				{
					target = Main.npc[targetID];
				}

				if (target != null)
				{
					Projectile.position = target.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
					float length = (Projectile.position - Projectile.oldPosition).Length();
					for (int i = 0; i < length; i += 8)
					{
						Dust.NewDustPerfect((Projectile.position * i + Projectile.oldPosition * (length - i)) / length, DustID.Electric, Scale: 0.75f);
					}
				}
			}
			Projectile.netUpdate = true;

			float targetX = player.Center.X;
			float targetY = player.Center.Y - 100;
			float maxSpeed = 12;
			float maxAcc = 0.6f;
			Projectile.friendly = target != null;
			Projectile.light = 0f;

			if (target != null)
			{
				Projectile.light = 1f;
				targetX = target.Center.X;
				targetY = target.Center.Y;
				maxSpeed = 16;
				maxAcc = 1f;

				Projectile.frameCounter++;
				if (Projectile.frameCounter == 2)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame == 7)
					{
						Projectile.frame = 1;
					}
					if (Main.rand.NextBool(4))
					{
						Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Scale: 0.75f);
					}
				}
			}
			else
			{
				Projectile.frame = 0;
			}

			if (((Projectile.Center.X - targetX) * (Projectile.Center.X - targetX)) + ((Projectile.Center.Y - targetY) * (Projectile.Center.Y - targetY)) > 1000)
			{
				float aX = (targetX - Projectile.Center.X) - Projectile.velocity.X;
				float aY = (targetY - Projectile.Center.Y) - Projectile.velocity.Y;

				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;

					Projectile.velocity.X += (float)Main.rand.NextDouble() * maxAcc * aX / (float)Math.Sqrt(aX * aX + aY * aY);
					Projectile.velocity.Y += (float)Main.rand.NextDouble() * maxAcc * aY / (float)Math.Sqrt(aX * aX + aY * aY);
				}
			}

			if (Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y > maxSpeed * maxSpeed)
			{
				float newVX = Projectile.velocity.X / (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
				float newVY = Projectile.velocity.Y / (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);

				Projectile.velocity.X = maxSpeed * newVX;
				Projectile.velocity.Y = maxSpeed * newVY;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			atkCooldown = 6;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return atkCooldown <= 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			return true;
		}
	}
}

