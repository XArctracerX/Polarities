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

namespace Polarities.Content.Items.Weapons.Magic.Staffs.Hardmode
{
	public class Radial : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Radial");
			//Tooltip.SetDefault("Creates a swarm of radial blades around the cursor which ignore up to 40 enemy defense");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 64;
			Item.height = 68;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold:20);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<RadialBlade>();
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.GetModPlayer<PolaritiesPlayer>().fractalization > 0)
			{
				mult = 0;
			}
		}

		private float angle;
		private int timer;

        public override void HoldItem(Player player)
        {
            if (player.channel)
			{
				if (player.GetModPlayer<PolaritiesPlayer>().fractalization <= 0)
					player.manaRegen = Math.Min(player.manaRegen, 0);
				player.itemTime = Item.useTime;
				player.itemAnimation = Item.useAnimation;
				player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;

				if (timer % 15 == 0)
				{
					SoundEngine.PlaySound(SoundID.Item15, player.position);
					if (player.GetModPlayer<PolaritiesPlayer>().fractalization <= 0)
					{
						if (!player.CheckMana((player.inventory[player.selectedItem].mana * player.ownedProjectileCounts[Item.shoot]) / 17, true))
						{
							player.channel = false;
						}
					}
					if (player.channel)
					{
						Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld, new Vector2(1, 0).RotatedBy(angle), Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
						angle += MathHelper.TwoPi * (17f / 180);
					}
				}

				player.itemRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();
				if (player.direction == -1) { player.itemRotation += (float)Math.PI; }

				timer++;
			}
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
				angle = 0f;
				timer = 0;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public bool Override()
		{
			return false;
		}

		public void Render(Player drawPlayer)
		{
			Texture2D texture = ModContent.Request<Texture2D>("Items/Weapons/Radial_Mask").Value;

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			float num79 = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
			int num78 = 0;
			int num77 = 0;
			Vector2 origin5 = new Vector2(0f, texture.Height);
			if (drawPlayer.gravDir == -1f)
			{
				if (drawPlayer.direction == -1)
				{
					num79 += 1.57f;
					origin5 = new Vector2(texture.Width, 0f);
					num78 -= texture.Width;
				}
				else
				{
					num79 -= 1.57f;
					origin5 = Vector2.Zero;
				}
			}
			else if (drawPlayer.direction == -1)
			{
				origin5 = new Vector2(texture.Width, texture.Height);
				num78 -= texture.Width;
			}
			Vector2 holdoutOrigin = Vector2.Zero;
			ItemLoader.HoldoutOrigin(drawPlayer, ref holdoutOrigin);
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + origin5.X + (float)num78), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + (float)num77)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, num79, origin5 + holdoutOrigin, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			//Main.playerDrawData.Add(drawData);
		}
	}


	public class RadialBlade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Radial Blade");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 900;
		}

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
				if (!Main.player[Projectile.owner].channel)
                {
					for (int i = 0; i < 4; i++) { Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric, Alpha: 0, newColor: Color.Transparent, Scale: 1f).noGravity = true; }
					Projectile.Kill();
                }
				float trueAngle = 2 * MathHelper.TwoPi * (900 - Projectile.timeLeft) / (180);
				float phaseDistance = 200 * (float)Math.Atan(8*Math.Sin(8f/5f * trueAngle));
				Projectile.position = (-Projectile.Size / 2) + Main.MouseWorld + Projectile.velocity * phaseDistance;
				Projectile.rotation = (Projectile.velocity * phaseDistance).ToRotation() - 3 * MathHelper.PiOver4;

			}
			Projectile.netUpdate = true;

			atkCooldown--;
        }

        public override bool ShouldUpdatePosition()
        {
			return false;
        }

        public override bool? CanCutTiles()
        {
			return false;
        }

		private int atkCooldown;

        public override bool? CanHitNPC(NPC target)
        {
			if (atkCooldown > 0)
            {
				return false;
            }
			else
            {
				return null;
            }
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += 40;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.immune[Projectile.owner] = 0;
			atkCooldown = 30;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float trueAngle = 2 * MathHelper.TwoPi * (900 - Projectile.timeLeft) / (180);
			float trueAnglePrev = 2 * MathHelper.TwoPi * (900 - Projectile.timeLeft + 1) / (180);
			float phaseDistance = 200 * (float)Math.Atan(8 * Math.Sin(8f / 5f * trueAngle));
			float phaseDistancePrev = 200 * (float)Math.Atan(8 * Math.Sin(8f / 5f * trueAnglePrev));
			Vector2 vel = (phaseDistance - phaseDistancePrev) * Projectile.velocity;
			float trailLength = 10f;
			for (int i = (int)trailLength - 1; i >= 0; i--)
			{
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - vel * (i / trailLength) - Main.screenPosition, new Rectangle(0, 0, 54, 54), Color.White * (1 - i / trailLength), Projectile.rotation, new Vector2(27, 27), Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}