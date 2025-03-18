using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Magic.Books.Hardmode
{
	public class MagneticStreams : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Magnetic Streams");
			//Tooltip.SetDefault("Fires arcing beams of light");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 360;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 34;
			Item.mana = 10;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = true;
			Item.useStyle = 5;
			Item.UseSound = SoundID.Item93;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ProjectileType<MagneticStreamsLaser>();
		}

		private int time;

		public override void HoldItem(Player player) {
			if (player.channel)
			{
				player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
				time++;
				if (!player.ItemTimeIsZero) player.itemTime = player.itemTimeMax;
				player.itemAnimation = player.itemAnimationMax;
				player.manaRegen = Math.Min(player.manaRegen,0);
				if (time % 10 == 0) {
					if (!player.CheckMana(player.inventory[player.selectedItem].mana, true)) {
						player.channel = false;
					}
				}
				if (time % 20 == 0)
                {
					SoundEngine.PlaySound(Item.UseSound, player.position);
                }
			}
		}
        public override System.Boolean Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, System.Int32 type, System.Int32 damage, System.Single knockback)
        {
			for (int i = 0; i < 4; i++)
			{
				Projectile.NewProjectile(source, player.Center, new Vector2(1, 0).RotatedBy(i * MathHelper.Pi / 4), type, damage, knockback, player.whoAmI, player.whoAmI);
			}
			return false;
		}
    }

	public class MagneticStreamsLaser : ModProjectile
	{
		private int owner {
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private int target {
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		private Vector2 targetCenter;
		private Vector2 circleCenter;
		
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Magnetic Stream");
		}

		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.timeLeft = 5;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override bool PreDraw(ref Color lightColor) {
			if (Projectile.timeLeft < 3) {
				Color c = Color.White;
				c.A = 127;
				float r = 0;
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				float radius = (Projectile.Center-circleCenter).Length();
				float initRotation = (Projectile.Center-circleCenter).ToRotation();

				for (int i=0; i<Math.Min(MathHelper.TwoPi*radius, 10000); i += 11) {
					for (int j=0; j<2; j++) {
						float laserOffset = (float)Math.Sin(MathHelper.TwoPi*j/2+Projectile.timeLeft/5f);
						Main.spriteBatch.Draw(texture, new Vector2(radius,0).RotatedBy(initRotation + (i-5000)/radius) + new Vector2(laserOffset,0).RotatedBy(initRotation + (i-5000)/radius) + circleCenter - Main.screenPosition,
							new Rectangle(6-2*Projectile.timeLeft, Projectile.frame*12, 4*Projectile.timeLeft, 12), c, initRotation + (i-5000)/radius + r,
							new Vector2(4*Projectile.timeLeft * .5f, 12 * .5f), Projectile.scale, 0, 0);
					}
				}
			} else {
				Color c = Color.White;
				c.A = 127;
				float r = 0;
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				float radius = (Projectile.Center-circleCenter).Length();
				float initRotation = (Projectile.Center-circleCenter).ToRotation();

				for (int i=0; i<Math.Min(MathHelper.TwoPi*radius, 10000); i += 11) {
					for (int j=0; j<2; j++) {
						float laserOffset = (float)Math.Sin(MathHelper.TwoPi*j/2+Projectile.timeLeft/5f);
						Main.spriteBatch.Draw(texture, new Vector2(radius,0).RotatedBy(initRotation + (i-5000)/radius) + new Vector2(laserOffset,0).RotatedBy(initRotation + (i-5000)/radius) + circleCenter - Main.screenPosition,
							new Rectangle(0, 0, 12, 12), c, initRotation + (i-5000)/radius + r,
							new Vector2(12 * .5f, 12 * .5f), Projectile.scale, 0, 0);
					}
				}
			}

			return false;
		}


        public override void DrawBehind(System.Int32 index, System.Collections.Generic.List<System.Int32> behindNPCsAndTiles, System.Collections.Generic.List<System.Int32> behindNPCs, System.Collections.Generic.List<System.Int32> behindProjectiles, System.Collections.Generic.List<System.Int32> overPlayers, System.Collections.Generic.List<System.Int32> overWiresUI)
        {
			behindNPCs.Add(index);
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float nearestX = Math.Max(targetHitbox.X, Math.Min(circleCenter.X, targetHitbox.X + targetHitbox.Size().X));
			float nearestY = Math.Max(targetHitbox.Y, Math.Min(circleCenter.Y, targetHitbox.Y + targetHitbox.Size().Y));
			bool isInside = (new Vector2(circleCenter.X-nearestX,circleCenter.Y-nearestY)).Length() < (Projectile.Center-circleCenter).Length() + 6;

			float furthestX = targetHitbox.X + targetHitbox.Size().X / 2 - circleCenter.X > 0 ? targetHitbox.X + targetHitbox.Size().X : targetHitbox.X;
			float furthestY = targetHitbox.Y + targetHitbox.Size().Y / 2 - circleCenter.Y > 0 ? targetHitbox.Y + targetHitbox.Size().Y : targetHitbox.Y;
			bool isOutside = (new Vector2(circleCenter.X-furthestX,circleCenter.Y-furthestY)).Length() > (Projectile.Center-circleCenter).Length() - 6;

			return isInside && isOutside;
		}

		private void ReTarget() {
			target = -1;
            float distance = 4000f;
            bool isTarget = false;
            int targetID = -1;
            for (int k = 0; k < 200; k++) {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable) {
                    Vector2 newMove = Main.npc[k].Center - Main.MouseWorld;
                    float distanceTo = newMove.Length();
                    if (distanceTo < distance) {
                        targetID = k;
                        distance = distanceTo;
                        isTarget = true;
                    }
                }
            }

            if (isTarget) {
                target = targetID;
            }
        }

		// The AI of the projectile
		public override void AI() {
			Player player = Main.player[owner];

			if (Main.myPlayer == Projectile.owner) {
				if (Projectile.timeLeft == 5) {
					targetCenter = Main.MouseWorld;
				}

				ReTarget();

				Vector2 targetCenterOffset = Main.MouseWorld-targetCenter;
				if (target != -1)
                {
					targetCenterOffset = Main.npc[target].Center - targetCenter;
				}
				if (targetCenterOffset.Length() > 24)
                {
					targetCenterOffset.Normalize();
					targetCenterOffset *= 24;
                }
				targetCenter += targetCenterOffset;
			}
			Projectile.netUpdate = true;

			if (!player.dead && player.active && player.channel && Projectile.timeLeft >= 3 ) {Projectile.timeLeft = 4;} else {player.channel = false;}
			
			Projectile.position = player.Center - Projectile.Hitbox.Size()/2f;

			//circle center = intersection of bisector between m & e, and perpendicular at m to v
			float a1 = player.Center.X-targetCenter.X;
			float b1 = player.Center.Y-targetCenter.Y;
			float c1 = (player.Center.X*player.Center.X+player.Center.Y*player.Center.Y-targetCenter.X*targetCenter.X-targetCenter.Y*targetCenter.Y)/2;
			float a2 = Projectile.velocity.X;
			float b2 = Projectile.velocity.Y;
			float c2 = Projectile.velocity.X*player.Center.X+Projectile.velocity.Y*player.Center.Y;
			circleCenter = new Vector2(
				(b2*c1-b1*c2)/(a1*b2-a2*b1),
				(a2*c1-a1*c2)/(a2*b1-a1*b2)
			);
		}

		public override bool ShouldUpdatePosition() => false;

        public override void SendExtraAI(System.IO.BinaryWriter writer) {
            writer.WriteVector2(targetCenter);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader) {
            targetCenter = reader.ReadVector2();
        }
	}
}