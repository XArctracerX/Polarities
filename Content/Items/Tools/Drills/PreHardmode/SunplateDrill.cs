using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Polarities.Content.Items.Placeable.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Drills.PreHardmode
{
    public class SunplateDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsDrill[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.knockBack = 0.5f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 42;
            Item.height = 20;
            
            Item.useTime = 4;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item23;

            Item.value = Item.buyPrice(gold: 12, silver: 60);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = ModContent.ProjectileType<SunplateDrillProj>(); 
            Item.shootSpeed = 32f;
            
            Item.noMelee = true; 
            Item.noUseGraphic = true;
            Item.channel = true; 

            Item.tileBoost = 1;
            Item.pick = 50;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SunplateBar>(), 14)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }

    public class SunplateDrillProj : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Tools/Drills/PreHardmode/SunplateDrillProjectile";

        public override void SetStaticDefaults()
        {
            // Prevents jitter when steping up and down blocks and half blocks
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 60;

            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                Projectile.soundDelay = 20;
            }

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
                    //holdoutOffset += new Vector2(baseOffset.X * player.direction, baseOffset.Y);
                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }

                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            if (Projectile.velocity.X > 0f)
            {
                player.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                player.ChangeDir(-1);
            }

            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            Projectile.Center = playerCenter;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

            Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.01f;
            Projectile.velocity.Y *= 1f + Main.rand.Next(-1, 1) * 0.01f;

            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity * Main.rand.Next(6, 10) * 0.15f, Projectile.width, Projectile.height, 15, 0f, 0f, 80, Color.White, 1f);
                dust.position.X -= 4f;
                dust.noGravity = true;
                dust.velocity.X *= 0.5f;
                dust.velocity.Y = -Main.rand.Next(3, 8) * 0.1f;
            }
        }
    }
}
