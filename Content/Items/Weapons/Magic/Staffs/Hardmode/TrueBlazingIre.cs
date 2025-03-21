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
    public class TrueBlazingIre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            //DisplayName.SetDefault("True Blazing Ire");
            //Tooltip.SetDefault("Left click to use as a javelin, right click to use as a spear" + "\nCritical strikes and melee hits summon additional javelins");
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 68;
            Item.height = 68;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(gold:10);
            Item.rare = 8;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<TrueBlazingIreJavelin>();
            Item.shootSpeed = 30f;
            Item.crit = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<BlazingIre>())
                .AddIngredient(ItemType<Materials.Hardmode.EclipxieDust>(), 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = 3;
                Item.shootSpeed = 2.5f;
                Item.shoot = ProjectileType<TrueBlazingIreSpear>();
            }
            else
            {
                Item.useStyle = 1;
                Item.shootSpeed = 30f;
                Item.shoot = ProjectileType<TrueBlazingIreJavelin>();
            }
            return player.ownedProjectileCounts[ProjectileType<TrueBlazingIreSpear>()] < 1 && base.CanUseItem(player);
        }
    }

    public class TrueBlazingIreJavelin : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Magic/Staffs/Hardmode/TrueBlazingIre";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("True Blazing Javelin");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            DrawOffsetX = -52;
            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 26;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 4;
            if (Projectile.timeLeft < 575)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y += 0.6f;
            }
            if (!Projectile.friendly)
            {
                Projectile.alpha += 26;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate++;
            Projectile.friendly = false;

            target.AddBuff(BuffID.OnFire, 120);
            if (hit.Crit && !Main.rand.NextBool(20) && Main.myPlayer == Projectile.owner)
            {
                Vector2 offset = Vector2.One.RotatedBy(Main.rand.NextFloat() * (float)Math.PI * 2);
                Projectile shot = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + 200 * offset, -20 * offset + target.velocity, ProjectileType<TrueBlazingIreJavelin>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
                shot.rotation = offset.ToRotation() + (float)Math.PI / 4;
                shot.netUpdate = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
        {
            target.AddBuff(BuffID.OnFire, 120);

            Projectile.penetrate++;
            Projectile.friendly = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            Vector2 dustPos = Projectile.Center;

            for (int i = 0; i < 9; ++i)
            {
                float r = Main.rand.NextFloat(6f);
                float theta = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 dustVel = new Vector2((float)Math.Cos(theta) * r, -(float)Math.Sin(theta) * r);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 133, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
            }

            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D trailTexture = Request<Texture2D>("Polarities/Content/Items/Weapons/Magic/Staffs/Hardmode/TrueBlazingIreJavelinTrail").Value;

            float alpha = ((255 - Projectile.alpha) / 255f);

            Color mainColor;
            Color mainColorA = new Color(231, 181, 65);
            Color mainColorB = Color.White;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k >= 599 - Projectile.timeLeft)
                {
                    continue;
                }

                float gradient = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                mainColor = new Color((int)((int)mainColorA.R * gradient + (int)mainColorB.R * (1 - gradient)), (int)((int)mainColorA.G * gradient + (int)mainColorB.G * (1 - gradient)), (int)((int)mainColorA.B * gradient + (int)mainColorB.B * (1 - gradient)));

                Color color = mainColor * gradient * alpha;
                float scale = Projectile.scale * gradient * alpha;

                float rotation;
                if (k + 1 >= Projectile.oldPos.Length)
                {
                    rotation = (Projectile.oldPos[k] - Projectile.position).ToRotation() + MathHelper.PiOver2;
                }
                else
                {
                    rotation = (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2;
                }
                rotation -= MathHelper.PiOver4;

                Main.spriteBatch.Draw(trailTexture, new Vector2(0, DrawOriginOffsetY) + Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2 + DrawOriginOffsetX, Projectile.height / 2), scale, SpriteEffects.None, 0f);
            }

            mainColor = Color.White;
            mainColor *= alpha;
            Main.spriteBatch.Draw(texture, new Vector2(0, DrawOriginOffsetY) + Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2 + DrawOriginOffsetX, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class TrueBlazingIreSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("True Blazing Spear");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            DrawOffsetX = 18 * 2 - 66 * 2;
            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 66 - 16;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        // In here the AI uses this example, to make the code more organized and readable
        // Also showcased in ExampleJavelinProjectile.cs
        public float movementFactor // Change this value to alter how fast the spear moves
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[Projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movementFactor = 3f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    movementFactor -= 3f;
                }
                else // Otherwise, increase the movement factor
                {
                    movementFactor += 2.8f;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            Projectile.position += Projectile.velocity * movementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            // do whatever custom stuff here
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate++;
            Projectile.friendly = false;

            target.AddBuff(BuffID.OnFire, 120);
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 offset = Vector2.One.RotatedBy(Main.rand.NextFloat() * (float)Math.PI * 2);
                    Projectile shot = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + 200 * offset, -20 * offset + target.velocity, ProjectileType<TrueBlazingIreJavelin>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
                    shot.rotation = offset.ToRotation() + (float)Math.PI / 4;
                    shot.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D trailTexture = Request<Texture2D>("Polarities/Content/Items/Weapons/Magic/Staffs/Hardmode/TrueBlazingIreSpearTrail").Value;
            float alpha = ((255 - Projectile.alpha) / 255f);

            Color mainColor;
            Color mainColorA = new Color(231, 181, 65);
            Color mainColorB = Color.White;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k >= 3599 - Projectile.timeLeft)
                {
                    continue;
                }

                float gradient = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                mainColor = new Color((int)((int)mainColorA.R * gradient + (int)mainColorB.R * (1 - gradient)), (int)((int)mainColorA.G * gradient + (int)mainColorB.G * (1 - gradient)), (int)((int)mainColorA.B * gradient + (int)mainColorB.B * (1 - gradient)));

                Color color = mainColor * gradient * alpha;
                float scale = Projectile.scale * gradient * alpha;

                float rotation = Projectile.rotation;

                Main.spriteBatch.Draw(trailTexture, new Vector2(0, DrawOriginOffsetY) + Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2 + DrawOriginOffsetX, Projectile.height / 2), scale, SpriteEffects.None, 0f);
            }

            mainColor = Color.White;
            mainColor *= alpha;
            Main.spriteBatch.Draw(texture, new Vector2(0, DrawOriginOffsetY) + Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2 + DrawOriginOffsetX, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}