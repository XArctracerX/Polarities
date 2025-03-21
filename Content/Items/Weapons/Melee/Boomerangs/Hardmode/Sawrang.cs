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

namespace Polarities.Content.Items.Weapons.Melee.Boomerangs.Hardmode
{
	public class Sawrang : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Sawrang");
		}

		public override void SetDefaults() {
			Item.damage = 60;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<SawrangProjectile>();
			Item.shootSpeed = 8f;
            Item.noUseGraphic = true;
		}

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[ProjectileType<SawrangProjectile>()] <= 0;
        }
	}
	public class SawrangProjectile : ModProjectile
    {
        private int targetNPC;
		public override string Texture => "Polarities/Content/Items/Weapons/Melee/Boomerangs/Hardmode/Sawrang";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sawrang");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7,Projectile.position);
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 60) {
                    Projectile.ai[0] = 1f;
                }
                float angle = Projectile.rotation;
            } else if (Projectile.ai[0] == 1f) {
                Projectile.tileCollide = false;
                float num49 = 9f;
                float num50 = 0.4f;
                Vector2 vector3 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num51 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector3.X;
                float num52 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector3.Y;
                float num53 = (float)Math.Sqrt((double)(num51 * num51 + num52 * num52));
                num53 = num49 / num53;
                num51 *= num53;
                num52 *= num53;

                if (Projectile.velocity.X < num51)
                {
                    Projectile.velocity.X = Projectile.velocity.X + num50;
                    if (Projectile.velocity.X < 0f && num51 > 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X + num50;
                    }
                }
                else if (Projectile.velocity.X > num51)
                {
                    Projectile.velocity.X = Projectile.velocity.X - num50;
                    if (Projectile.velocity.X > 0f && num51 < 0f)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - num50;
                    }
                }
                if (Projectile.velocity.Y < num52)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + num50;
                    if (Projectile.velocity.Y < 0f && num52 > 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + num50;
                    }
                }
                else if (Projectile.velocity.Y > num52)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - num50;
                    if (Projectile.velocity.Y > 0f && num52 < 0f)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - num50;
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                    {
                        Projectile.Kill();
                    }
                }
            } else {
                Projectile.ai[0]--;
                if(Main.npc[targetNPC].active) {
                    Projectile.velocity = Main.npc[targetNPC].position - Main.npc[targetNPC].oldPosition;
                } else {
                    Projectile.ai[0]=1;
                }
            }

            if (Projectile.ai[1] == 0) {
                Projectile.rotation = (float)(Main.rand.NextDouble()*2*Math.PI);
            }
            Projectile.rotation += 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.ai[0] == 0) {
                Projectile.ai[0] = 120f;
                targetNPC = target.whoAmI;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurt) {
            Projectile.ai[0] = 1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.ai[0] = 1f;
			if (Projectile.velocity.X != oldVelocity.X) {
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y) {
				Projectile.velocity.Y = -oldVelocity.Y;
			}
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 thing) {
            width = 2;
            height = 2;
            return Projectile.tileCollide;
        }
    }
}