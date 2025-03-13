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

namespace Polarities.Content.Items.Weapons.Melee.Boomerangs.Hardmode
{
	public class MoonDisc : ModItem
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Lunarang");
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults() {
			Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
			Item.width = 88;
			Item.height = 88;
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 80000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<Lunarang>();
			Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
			{
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.active)
                    {
                        if (projectile.type == ProjectileType<Lunarang>() && projectile.owner == player.whoAmI)
                        {
                            projectile.ai[0] = 2;
                        }
                    }
                }
            }
			else
			{
				if (player.ownedProjectileCounts[ProjectileType<Lunarang>()] <= 0)
				{
					Projectile.NewProjectile(source, position, velocity, ProjectileType<Lunarang>(), damage, knockback, player.whoAmI);
				}
			}
			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				return player.ownedProjectileCounts[ProjectileType<Lunarang>()] > 0;
			}
			else
			{
				return player.ownedProjectileCounts[ProjectileType<Lunarang>()] <= 0;
			}
		}
	}
    public class Lunarang : ModProjectile
    {
        private int attackTimer;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Moon Disc");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 30f)
                {
                    Projectile.ai[0] = 1f;
                }
            }
            else if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 1800f)
                {
                    Projectile.ai[0] = 2f;
                }

                int targetID = -1;
                Projectile.Minion_FindTargetInRange(400000, ref targetID, skipIfCannotHitWithOwnBody: false);
                NPC target = null;
                if (targetID != -1)
                {
                    target = Main.npc[targetID];
                }
                else
                {
                    Projectile.ai[0] = 2f;
                    return;
                }
                Vector2 acc = (target.velocity - Projectile.velocity) / 10 + (target.Center - Projectile.Center) / 300;
                if (acc.Length() > 1)
                {
                    acc.Normalize();
                }
                if (Math.Abs(acc.X) <= 2 && Math.Abs(acc.Y) <= 2)
                {
                    Projectile.velocity += acc;
                    if (Projectile.ai[1] % 40 == 0)
                    {
                        int projectiles = 8;
                        float startingAngle = Main.rand.NextFloat() * (float)Math.PI / (projectiles / 2);
                        for (int i = 0; i < projectiles; i++)
                        {
                            float angle = ((float)Math.PI * 2f * (1f * i / projectiles)) + startingAngle;
                            Vector2 shotDirection = 3 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shotDirection, 85, Projectile.damage, 0, Projectile.owner).ai[0] = 1;
                            SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);
                        }
                    }
                }
                if (Projectile.velocity.Length() > 16)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 16;
                }
            }
            else
            {
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
            }
            if (Projectile.ai[1] == 0)
            {
                Projectile.rotation = (float)(Main.rand.NextDouble() * 2 * Math.PI);
            }
            Projectile.rotation += 0.5f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 2;
            height = 2;
            return Projectile.tileCollide;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = Math.Max(Projectile.ai[0], 1f);
            target.AddBuff(BuffID.Frostburn2, 600);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
        {
            Projectile.ai[0] = Math.Max(Projectile.ai[0], 1f);
            target.AddBuff(BuffID.Frostburn2, 600);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = Math.Max(Projectile.ai[0], 1f);
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
    }

    // deprecated?
    /*public class SunDiscFire : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.alpha = 0;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 127;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: 2.5f).noGravity = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
        {
            target.AddBuff(BuffID.OnFire3, 600);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 600);
        }
    }*/
}