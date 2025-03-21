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
	public class JuliaShard : ModItem
	{
		public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 56;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item9;
			Item.shoot = ProjectileType<JuliaShardProjectile>();
			Item.shootSpeed = 10f;
		}

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 12)
                .AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 3)
                .AddIngredient(ItemID.Glass, 16)
                .AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
                .Register();
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            //if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
            //{
            //    mult = 0;
            //}
        }

        public override void UpdateInventory(Player player)
        {
            Item.autoReuse = false;
        }
    }

    public class JuliaShardProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Julia Shard");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
        }

        public override void AI() {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;

                for (int k=0; k<Projectile.oldPos.Length; k++)
                {
                    Projectile.oldPos[k] = Projectile.position;
                }
            }

            Projectile.frameCounter++;

            if (Main.player[Projectile.owner].channel) {
                Projectile.timeLeft = 2;
                Projectile.velocity = Main.MouseWorld-Projectile.Center;

                if (Projectile.velocity.Length() > 16) {
                    Projectile.velocity.Normalize();
                    Projectile.velocity*=16;
                } else {
                    Projectile.Kill();
                }
                Projectile.netUpdate = true;
            }

            //oldPos trail diffusion thing
            for (int k=1; k<Projectile.oldPos.Length-1; k++)
            {
                Projectile.oldPos[k] = (Projectile.oldPos[k - 1] + Projectile.oldPos[k + 1]) / 2f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 thing) {
            width = 2;
            height = 2;
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>("Terraria/Images/Projectile_644").Value;//Main.projectileTexture[Projectile.type];//Main.projectileTexture[ProjectileID.RainbowCrystalExplosion];

            Color mainColor = new Color(181, 248, 255);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                float scale = Projectile.scale * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float rotation;
                if (k + 1 >= Projectile.oldPos.Length)
                {
                    rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }
                else
                {
                    rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }

                Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, new Vector2(36, 36), new Vector2(scale, Projectile.scale), SpriteEffects.None, 0f);
            }


            int numDraws = 11;
            for (int i = 0; i < numDraws; i++)
            {
                float scale = 0.5f + (1 + (float)Math.Sin(Projectile.frameCounter * 0.1f + (MathHelper.TwoPi * i) / numDraws)) / 4f;
                Color color = new Color((int)(82 * scale + 512 * (1 - scale)), (int)(179 * scale + 512 * (1 - scale)), (int)(203 * scale + 512 * (1 - scale)));
                float alpha = 0.3f;
                float rotation = Projectile.rotation;
                Vector2 positionOffset = new Vector2(4, 0).RotatedBy(6 * i * MathHelper.TwoPi / numDraws + Projectile.frameCounter * 0.2f);
                scale *= 1.25f;

                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center + positionOffset - Main.screenPosition, new Rectangle(0, 0, 36, 36), color * alpha, rotation, new Vector2(18, 18), Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return false;
        }

        public override void OnKill(int timeLeft) {
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
            if (Main.myPlayer == Projectile.owner){
                Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center,Vector2.Zero,ProjectileType<JuliaShardExplosion>(),Projectile.damage,Projectile.knockBack,Projectile.owner)].netUpdate = true;
            }
        }
    }

    public class JuliaShardExplosion : ModProjectile
	{
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 1;
            Projectile.tileCollide = true;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Vector2.Zero, Scale: 0.75f);
            dust.noGravity = true;
            for (int radius = 8; radius <= 128; radius += 8)
            {
                for (int i = 0; i < (radius/2); i++)
                {
                    dust = Dust.NewDustPerfect(Projectile.Center + new Vector2(radius, 0).RotatedBy(i * MathHelper.TwoPi / (radius / 2)), DustID.Electric, Vector2.Zero, Scale: 0.75f);
                    dust.noGravity = true;
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 thing) {
            width = 2;
            height = 2;
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float nearestX = Math.Max(targetHitbox.X, Math.Min(Projectile.Center.X, targetHitbox.X + targetHitbox.Size().X));
            float nearestY = Math.Max(targetHitbox.Y, Math.Min(Projectile.Center.Y, targetHitbox.Y + targetHitbox.Size().Y));
            return (new Vector2(Projectile.Center.X - nearestX, Projectile.Center.Y - nearestY)).Length() < Projectile.width / 2;
        }
    }
}