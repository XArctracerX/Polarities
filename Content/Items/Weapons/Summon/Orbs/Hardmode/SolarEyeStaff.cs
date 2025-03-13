using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Summon.Minions.Hardmode
{
	public class SolarEyeStaff : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Solar Eye Staff");
			// Tooltip.SetDefault("Summons a solar eye to protect you");
		}

		public override void SetDefaults() {
			Item.damage = 89;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noMelee = true;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold:10);
			Item.rare = 6;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<SolarEye>();
			Item.buffType = BuffType<SolarEyeBuff>();
			Item.shootSpeed = 12f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 18000, true);
			return true;
		}
	}

	public class SolarEyeBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Eye");
			// Description.SetDefault("The solar eye will watch over you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<SolarEye>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	public class SolarEye : ModProjectile
	{
		private Vector2 playerDisplacement;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Eye");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			DrawOffsetX = -14;
			DrawOriginOffsetY = -14;
			DrawOriginOffsetX = 0;

			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.friendly = true;
			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
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
			if (player.dead)
			{
				player.ClearBuff(BuffType<SolarEyeBuff>());
			}
			if (player.HasBuff(BuffType<SolarEyeBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			int targetID = -1;
			Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
			NPC target = null;
			if (targetID != -1)
			{
				target = Main.npc[targetID];
			}

			if (Main.rand.NextBool(60))
			{
				playerDisplacement = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 0));
				Projectile.netUpdate = true;
			}

			Projectile.frameCounter++;

			if ((Projectile.Center - player.Center).Length() > 2000 && Projectile.ai[0] == 0)
			{
				Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
			}

			if (target == null)
			{
				if (Projectile.ai[0] == 1)
				{
					Projectile.ai[0] = 0;
				}
				Projectile.ai[1] = 0;

				int index = 0;
				for (int i = 0; i < Projectile.whoAmI; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
						index++;
				}

				Vector2 targetPos = player.MountedCenter - new Vector2(0, 104 + index * 52).RotatedBy(400 * timer++ / (float)((104 + index * 52) * (104 + index * 52)));

				if ((targetPos - Projectile.Center).Length() < 16 || Projectile.ai[0] == 2)
				{
					Projectile.ai[0] = 2;
					Projectile.velocity = targetPos - Projectile.Center;
					Projectile.frame = 0;
					Projectile.rotation -= 0.5f;
				}
				else
				{
					Vector2 targetAcceleration = 16 * (targetPos - Projectile.Center) / (targetPos - Projectile.Center).Length() - Projectile.velocity;
					if (targetAcceleration.Length() > 0.01f) { targetAcceleration = Math.Min(targetAcceleration.Length(), 0.5f) * targetAcceleration / targetAcceleration.Length(); } else { targetAcceleration = Vector2.Zero; }
					Projectile.velocity += targetAcceleration;
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
					Projectile.frame = 1;
				}
			}
			else
			{
				if (Projectile.ai[0] == 2)
					Projectile.ai[0] = 0;

				Vector2 targetPos = target.Center + new Vector2(Main.rand.Next(-target.width, target.width), Main.rand.Next(-target.height, target.height));

				if (Projectile.ai[0] == 0)
				{
					Vector2 targetAcceleration = 16 * (targetPos - Projectile.Center) / (targetPos - Projectile.Center).Length() - Projectile.velocity;
					if (targetAcceleration.Length() > 0.01f) { targetAcceleration = Math.Min(targetAcceleration.Length(), 0.5f) * targetAcceleration / targetAcceleration.Length(); } else { targetAcceleration = Vector2.Zero; }
					Projectile.velocity += targetAcceleration;
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
					Projectile.ai[1]--;
				}
				else
				{
					Projectile.rotation -= 0.5f;
					Projectile.velocity *= 0.975f;
					Projectile.ai[1]++;

					if (Projectile.ai[1] % 6 == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(4, 0).RotatedByRandom(MathHelper.TwoPi), ProjectileType<SolarEyeSpark>(), (2 * Projectile.damage) / 5, 0, Projectile.owner);
					}

					if (Projectile.ai[1] == 60f)
					{
						Projectile.ai[0] = 0f;
					}
				}
				Projectile.frame = 1 - (int)Projectile.ai[0];
			}
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3());
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return Projectile.ai[0] == 0 && Projectile.ai[1] <= 30;
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			writer.WriteVector2(playerDisplacement);
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			playerDisplacement = reader.ReadVector2();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1f;
				Projectile.ai[1] = 0f;
			}
			target.AddBuff(BuffID.OnFire, 120);
			target.AddBuff(BuffID.Frostburn, 120);
		}

		/*public override bool PreDraw(SpriteBatch Main.spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[Projectile.type];
			if (Projectile.ai[0] == 0) {
				float numImages = 16;
				for (int i = (int)numImages-1; i >= 0; i--)
				{
					Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.velocity * (i / numImages) * 8 - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / 2, texture.Width, texture.Height / 2), Color.White * (1 - i / (numImages)), Projectile.rotation, new Vector2(30, 30), Projectile.scale * (1 - i / numImages), SpriteEffects.None, 0f);
				}
			}
			else
            {
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / 2, texture.Width, texture.Height / 2), Color.White, Projectile.rotation, new Vector2(30, 30), Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}*/


		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor = Color.White;

			if (Projectile.ai[0] == 0)
			{
				for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
				{
					Color color = mainColor * ((float)(Projectile.oldPos.Length - k - 0.5f) / (float)Projectile.oldPos.Length);
					float scale = Projectile.scale * ((float)(Projectile.oldPos.Length - k - 0.5f) / (float)Projectile.oldPos.Length);

					Vector2 newerPos = Projectile.position;
					if (k + 1 < Projectile.oldPos.Length)
					{
						newerPos = Projectile.oldPos[k + 1];
					}

					float rotation = (Projectile.oldPos[k] - newerPos).ToRotation() + MathHelper.PiOver2;

					Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);

					color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					scale = Projectile.scale * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + (Projectile.oldPos[k] + newerPos) / 2 - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);
				}
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class SolarEyeSpark : ModProjectile
	{
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Spark");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.alpha = 0;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.15f;

			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: 1f);

			if (Projectile.wet)
			{
				Projectile.Kill();
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
		{
			target.AddBuff(BuffID.OnFire, 60);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 60);
		}


		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}