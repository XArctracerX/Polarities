using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Projectiles;
using System;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Flawless
{
	public class BeyondBow : ModItem
	{
		private int chargeTime;
		private int chargeParity;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults()
		{
			Item.damage = 12;
			Item.knockBack = 4f;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 46;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item5;
			Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 5);
			Item.channel = true;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Arrow;

            Item.rare = RarityType<EsophageFlawlessRarity>(); //get its own eventually
        }

		public override void HoldItem(Player player)
		{
			if (player.channel)
			{
                Vector2 velocity = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
                player.itemRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();
				if (player.direction == -1) { player.itemRotation += (float)Math.PI; }

				if (chargeTime == 0)
                {
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.MountedCenter + velocity.SafeNormalize(Vector2.Zero) * 60, velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.5f, 2f), ProjectileType<BeyondBowRift>(), Item.damage, Item.knockBack, player.whoAmI);
                }

				chargeTime++;
			}
			else
            {
				chargeTime = 0;
            }
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
            {
				type = ProjectileType<BeyondBowProjectile>();
            }

			float lineDistance = 40f;
			float lineRadius = Math.Min(62f, chargeTime / 20f) * 16f / 20f;
			float angleVariance = 400f / (chargeTime + 400f);

			position = player.Center;

			if (type == ProjectileType<BeyondBowProjectile>())
				position -= velocity * 2;

			chargeParity = (chargeParity + 1) % 2;

			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 0);
		}
	}

	public class BeyondBowRift : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.alpha = 0;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
		}

		public override void AI()
		{
			Player projOwner = Main.player[Projectile.owner];

			if (!projOwner.channel)
			{
				Projectile.Kill();
				return;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.Center = projOwner.Center + (Main.MouseWorld - projOwner.Center).SafeNormalize(Vector2.Zero) * 32f;
				Projectile.rotation = (Main.MouseWorld - projOwner.Center).ToRotation();
			}
			Projectile.netUpdate = true;

			Projectile.timeLeft = 2;
			Projectile.localAI[0]++;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float lineRadius = Math.Max(10f, Math.Min(62f, Projectile.localAI[0] / 20f)) * 16f / 20f;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + new Vector2(0, lineRadius).RotatedBy(Projectile.rotation), Projectile.Center - new Vector2(0, lineRadius).RotatedBy(Projectile.rotation), 16, ref point);
		}

        public override bool PreDraw(ref Color lightColor)
		{
			float lineRadius = Math.Min(62f, Projectile.localAI[0] / 20f);

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();

			float numImages = 3;
			for (int i = 1; i <= numImages; i++)
			{
				Main.spriteBatch.Draw(texture, (Projectile.Center * i + (numImages - i) * Main.player[Projectile.owner].Center) / numImages - Main.screenPosition, frame, Color.White * (i / numImages), Projectile.rotation, frame.Size() / 2, new Vector2(1, 2 * lineRadius / texture.Height) * (i / numImages), SpriteEffects.None, 0f);
			}

			return false;
        }
	}

	public class BeyondBowProjectile : ModProjectile
	{
        public override string Texture => "Polarities/Content/NPCs/Bosses/PreHardmode/RiftDenizen/RiftBolt";

        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rift Bolt");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.alpha = 0;
			Projectile.light = 0.5f;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 540 && Projectile.ai[0] == 0)
            {
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.position = Main.MouseWorld + new Vector2(300, 0).RotatedByRandom(MathHelper.TwoPi);
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
				}
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.velocity = oldVelocity;
            if (Projectile.ai[0] == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.position = Main.MouseWorld + new Vector2(300, 0).RotatedByRandom(MathHelper.TwoPi);
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
				}
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;

				Projectile.rotation = Projectile.velocity.ToRotation();

				return false;
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				return true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft >= 599)
            {
				return false;
            }

			Texture2D texture = ModContent.Request<Texture2D>("Polarities/Content/Projectiles/CallShootProjectile").Value;
			Rectangle frame = new Rectangle(0, 0, 1, 1);

			Texture2D texture2 = ModContent.Request<Texture2D>("Polarities/Content/Items/Weapons/Ranged/Flawless/BeyondBowRiftMini").Value;
            Rectangle frame2 = texture2.Frame();

			for (int i = 0; i < Projectile.oldPos.Length - 1; i++)
			{
				if (Projectile.timeLeft + i < 599)
				{
					if (Projectile.oldPos[i] != Projectile.oldPos[i + 1])
					{
						if ((Projectile.oldPos[i] - Projectile.oldPos[i + 1]).Length() >= Projectile.velocity.Length() * 2)
						{
							Main.spriteBatch.Draw(texture2, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame2, Color.White, Projectile.oldRot[i], frame2.Size() / 2, new Vector2(1, 1), SpriteEffects.None, 0f);
							Main.spriteBatch.Draw(texture2, Projectile.oldPos[i + 1] + Projectile.Center - Projectile.position - Main.screenPosition, frame2, Color.White, Projectile.oldRot[i + 1], frame2.Size() / 2, new Vector2(1, 1), SpriteEffects.None, 0f);
						}
					}
				}
			}

			for (int i = 0; i < Projectile.oldPos.Length - 1; i++)
			{
				if (Projectile.oldPos[i] != Projectile.oldPos[i + 1])
				{
					if ((Projectile.oldPos[i] - Projectile.oldPos[i + 1]).Length() < Projectile.velocity.Length() * 2)
					{
						Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White * (1 - i / (float)(Projectile.oldPos.Length - 1)), Projectile.oldRot[i], frame.Size() / 2, new Vector2(Projectile.velocity.Length() / frame.Width, 4f * (1 - i / (float)(Projectile.oldPos.Length - 1))), SpriteEffects.None, 0f);
					}
				}
			}

			texture = TextureAssets.Projectile[Projectile.type].Value;
			frame = texture.Frame();

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, new Vector2(Projectile.velocity.Length() / frame.Width, 1f), SpriteEffects.None, 0f);

			return false;
		}
	}
}