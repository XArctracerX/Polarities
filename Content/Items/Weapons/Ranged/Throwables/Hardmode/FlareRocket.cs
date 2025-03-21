using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Projectiles;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Throwables.Hardmode
{
	public class FlareRocket : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 200;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 42;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1f;
			Item.value = 1000;
			Item.rare = 8;
			Item.shoot = ProjectileType<FlareRocketProjectile>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
		}

		public override void AddRecipes() {
            CreateRecipe(100)
                .AddIngredient(ItemType<Flarecaller>(), 100)
                .AddIngredient(ItemType<Materials.Hardmode.EclipxieDust>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
	}

    public class FlareRocketProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Ranged/Throwables/Hardmode/FlareRocket";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flare Rocket");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            DrawOffsetX = -22;
            DrawOriginOffsetX = 11;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                float dAngle = (Main.MouseWorld - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
                while (dAngle > MathHelper.Pi)
                {
                    dAngle -= MathHelper.TwoPi;
                }
                while (dAngle < -MathHelper.Pi)
                {
                    dAngle += MathHelper.TwoPi;
                }
                float maxTurn = 1 / Projectile.velocity.Length();
                if (dAngle > maxTurn)
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(maxTurn);
                }
                else if (dAngle < -maxTurn)
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(-maxTurn);
                }
                else
                {
                    Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                }
            }
            Projectile.netUpdate = true;

            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2 - Projectile.direction * (float)Math.PI / 2;
            Projectile.spriteDirection = (Projectile.velocity.X > 0) ? 1 : -1;
            Projectile.velocity *= 1.03f;

            int dustIndex = Dust.NewDust(Projectile.position - (Projectile.position - Projectile.Center) / 2, Projectile.width / 2, Projectile.height / 2, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position + Projectile.velocity);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center + Projectile.velocity + new Vector2(0, Projectile.height / 2 + 2), Vector2.Zero, ProjectileType<EclipseFlareFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position + Projectile.velocity);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center + Projectile.velocity + new Vector2(0, Projectile.height / 2 + 2), Vector2.Zero, ProjectileType<EclipseFlareFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Projectile.Kill();
        }

        public class EclipseFlareFriendly : ModProjectile
		{
			private float Distance;
			private int frame;
			private int timer;
			public override void SetStaticDefaults()
			{
				//DisplayName.SetDefault("Eclipse Flare");
			}

            public override void SetDefaults()
            {
                Projectile.width = 10;
                Projectile.height = 10;
                Projectile.friendly = true;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.hide = false;
                Projectile.timeLeft = 60;
                Projectile.DamageType = DamageClass.Ranged;

                Projectile.GetGlobalProjectile<PolaritiesProjectile>().ForceDraw = true;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                DrawLaser(TextureAssets.Projectile[Type], Projectile.Center, new Vector2(0, -1), 64, Projectile.damage, (float)Math.PI / 2);
                return false;
            }

            // The core function of drawing a laser
            public void DrawLaser(Asset<Texture2D> texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 0)
            {
                float r = unit.ToRotation() + rotation;

                // Draws the laser 'body'
                for (float i = transDist + step; i <= Distance; i += step)
                {
                    Color c = Color.White;
                    var origin = start + i * unit;
                    Main.EntitySpriteDraw(texture.Value, origin - Main.screenPosition,
                        new Rectangle(0, 256 * frame, 120, 128), i < transDist ? Color.Transparent : c, r,
                        new Vector2(120 * .5f, 128), scale, 0, 0);
                }

                Main.EntitySpriteDraw(texture.Value, start - Main.screenPosition,
                    new Rectangle(0, 128 + 256 * frame, 120, 128), Color.White, r, new Vector2(120 * .5f, 128), scale, 0, 0);
            }

            // Change the way of collision check of the projectile
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                Rectangle projHitboxInflated = new Rectangle(projHitbox.X - 40, projHitbox.Y - 46, projHitbox.Width + 80, projHitbox.Height + 46);
                if (projHitboxInflated.Intersects(targetHitbox)) return true;

                Vector2 unit = new Vector2(0, -1);
                float point = 0f;
                // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
                // It will look for collisions on the given line using AABB
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                    Projectile.Center + unit * Distance, 22, ref point);
            }

            // The AI of the projectile
            public override void AI()
            {
                Distance = Projectile.position.Y;
                SpawnDusts();
                CastLights();
                timer = (timer + 1) % 5;
                if (timer == 0)
                {
                    frame++;
                }
                if (frame == 8)
                {
                    Projectile.Kill();
                }
            }

            public override void OnHitPlayer(Player target, Player.HurtInfo info)
            {
                target.AddBuff(BuffID.OnFire3, 600, true);
            }

            private void SpawnDusts()
            {
                Vector2 dustPos = Projectile.Center;

                for (int i = 0; i < 2; ++i)
                {
                    float r = Main.rand.NextFloat() * 6f;
                    float theta = (float)Math.PI * Main.rand.NextFloat();
                    Vector2 dustVel = new Vector2((float)Math.Cos(theta) * r, -(float)Math.Sin(theta) * r);
                    Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, DustID.Firework_Yellow, dustVel.X, dustVel.Y)];
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }

            private void CastLights()
            {
                // Cast a light along the line of the laser
                DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
                Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - 0), 26, DelegateMethods.CastLight);
            }

            public override bool ShouldUpdatePosition() => false;
        }
	}
}
