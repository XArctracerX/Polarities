using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Polarities.Content.Projectiles;
using Polarities.Content.Buffs.Hardmode;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Staffs.Hardmode
{
    public class BleedingSky : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bleeding Sky");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;

            Item.width = 68;
            Item.height = 70;

            Item.useTime = 9;
            Item.useAnimation = 10;

            Item.autoReuse = true;
            Item.noMelee = true;
            
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            
            Item.knockBack = 1f;
            
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            
            Item.shoot = ProjectileType<BloodDrop>();
        }

        //custom hold animation is done in PolaritiesPlayer PostUpdate

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //stalactites
            type = ProjectileType<BloodDrop>();

            for (int j = -3; j < 5; j++)
            {
                int positionX = j + ((int)(Main.MouseWorld.X)) / 16;
                int positionY = ((int)(Main.MouseWorld.Y)) / 16;

                float extraY = 0;

                for (int i = 0; i < Main.rand.Next(20, 50); i++)
                {
                    positionY--;
                    if (Main.tile[positionX, positionY].HasUnactuatedTile && Main.tileSolid[Main.tile[positionX, positionY].TileType] && !Main.tileSolidTop[Main.tile[positionX, positionY].TileType])
                    {
                        if (Main.tile[positionX, positionY].Slope == SlopeType.SlopeUpLeft || Main.tile[positionX, positionY].Slope == SlopeType.SlopeUpRight)
                        {
                            extraY = -8;
                        }
                        break;
                    }
                }
                Projectile.NewProjectile(source, new Vector2(16 * positionX + 8, 16 * positionY + 16 + extraY), Vector2.Zero, type, damage, knockback, Main.myPlayer);
            }
            return false;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }
    }

    public class BloodDrop : ModProjectile
    {
        public override string Texture => "Polarities/Assets/Invis";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            DrawOffsetX = -9;
            DrawOriginOffsetY = -38;
            DrawOriginOffsetX = 0;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.spriteDirection = Main.rand.NextBool() ? 1 : -1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(3);
            }

            if (Projectile.ai[0] == 4) Projectile.tileCollide = false;

            Projectile.ai[0]++;

            Projectile.velocity.Y += 0.2f;
            Projectile.velocity.X = 0;

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
    }

    public class FriendlyBloodRain : ModProjectile
    {
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Rain");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Projectile.oldPos[k] = Projectile.position;
                }
                Projectile.localAI[0] = 1;
            }

            if (Projectile.position.Y > Projectile.ai[0])
            {
                Projectile.tileCollide = true;
            }

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HitEffect();
            return false;
        }

        private void HitEffect()
        {
            Projectile.friendly = false;
            if (Projectile.timeLeft > Projectile.oldPos.Length + 2)
            {
                Projectile.timeLeft = Projectile.oldPos.Length + 2;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        private static Asset<Texture2D> texture;

        public override void Load()
        {
            texture = Request<Texture2D>("Polarities/Content/Projectiles/CallShootProjectile");
        }

        public override void Unload()
        {
            texture = null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Texture2D texture = mod.GetTexture("Projectiles/Content/CallShootProjectile");

                

            Color mainColor = new Color(168, 0, 0);

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                if (Projectile.oldPos.Length - k < Projectile.timeLeft)
                {
                    float amount = ((Projectile.oldPos.Length - k) / Projectile.oldPos.Length);

                    Color color = mainColor * (1 - Projectile.alpha / 255f);
                    float scale = 2f * Projectile.scale * amount;

                    float rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation();

                    Main.spriteBatch.Draw(texture.Value, Projectile.Center - (Projectile.position - Projectile.oldPos[k]) * 0.9f - Main.screenPosition, new Rectangle(0, 0, 1, 1), color, rotation, new Vector2(0, 0.5f), new Vector2((Projectile.oldPos[k + 1] - Projectile.oldPos[k]).Length(), scale), SpriteEffects.None, 0f);
                }
            }

            return false;
        }
    }
}