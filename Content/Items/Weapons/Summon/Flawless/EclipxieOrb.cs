using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Polarities.Global;
using Polarities.Core;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Summon.Flawless
{
    public class EclipxieOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(120, 4, 0);
            Item.DamageType = DamageClass.Summon;

            Item.width = 22;
            Item.height = 30;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;

            Item.shoot = ProjectileType<EclipxieOrbPlanet>();

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = RarityType<QueenBeeFlawlessRarity>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < player.GetModPlayer<PolaritiesPlayer>().orbMinionSlots; i++)
            {
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Mercury>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Venus>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Earth>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Mars>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Jupiter>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Saturn>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Uranus>(), damage, knockback, player.whoAmI, ai1: i);
                Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Neptune>(), damage, knockback, player.whoAmI, ai1: i);
                for (int j = 0; j < 12; j++)
                {
                    Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<Asteroid>(), damage / 4, knockback, player.whoAmI, ai1: i);
                }
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
                if (!player.ItemTimeIsZero) player.itemTime = player.itemTimeMax;
                player.itemAnimation = player.itemAnimationMax;
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-player.direction * 2, 8 - Item.height / 2);
        }
    }

    public class EclipxieOrbPlanet : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Mercury";

        string _glowOverride = "default";
        public virtual string GlowOverride
        {
            get { return _glowOverride; }
            set { _glowOverride = value; }
        }
        public virtual float Eccentricity { get; set; }
        public virtual float OrbitalSpeed { get; set; }
        public virtual float DistanceMultiplier { get; set; }
        public virtual Color[] OrbitColors => new Color[] { Color.Goldenrod, Color.Yellow, Color.Orange };

        public Vector2 focus1, focus2;

        private Vector2 GetEllipsePosition(Vector2 f1, Vector2 f2, float eccentricity, float t)
        {
            Vector2 center = new Vector2((f1.X + f2.X) / 2, (f1.Y + f2.Y) / 2);
            float semimajor = f1.Distance(f2) / 2;
            float a = semimajor / eccentricity;
            float b = (float)Math.Sqrt(a * a - semimajor * semimajor);
            float angle = (float)Math.Atan2(f2.Y - f1.Y, f2.X - f1.X);

            float x = center.X + a * (float)Math.Cos(t) * (float)Math.Cos(angle) - b * (float)Math.Sin(t) * (float)Math.Sin(angle);
            float y = center.Y + a * (float)Math.Cos(t) * (float)Math.Sin(angle) - b * (float)Math.Sin(t) * (float)Math.Cos(angle);
            return new Vector2(x, y);
        }

        private void DrawLoop(List<Vector2> list, params Color[] colors)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = Vector2.Zero;
                if (i == list.Count - 1) diff = list[0] - element;
                else diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                //Color color = Lighting.GetColor(element.ToTileCoordinates(), mainColor);
                Color color = ModUtils.ColorLerpCycle(i, list.Count, colors);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 0.5f;

            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3600;
        }

        public float t = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0]++ == 0)
            {
                DrawOffsetX = -(int)(Projectile.width * (1 - Projectile.scale));
                DrawOriginOffsetY = -(int)(Projectile.height * (1 - Projectile.scale));
                focus1 = player.Center;
                focus2 = player.Center + (Main.MouseWorld - player.Center) * 0.01f;
                t = Projectile.ai[1] * 2 * MathHelper.Pi / player.GetModPlayer<PolaritiesPlayer>().orbMinionSlots;
            }

            if (!player.channel || !player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                Projectile.timeLeft = 2;
            }

            focus1 = Vector2.Lerp(focus1, player.Center, 0.05f);
            focus2 = Vector2.Lerp(focus2, player.Center + (Main.MouseWorld - player.Center) * DistanceMultiplier, 0.15f);
            Projectile.position = GetEllipsePosition(focus1, focus2, Eccentricity, t) - new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            t += OrbitalSpeed;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (OrbitColors.Length > 0)
            {
                List<Vector2> orbitPoints = new List<Vector2>();
                for (float i = 0; i < MathHelper.Pi * 2; i += 0.01f)
                {
                    orbitPoints.Add(GetEllipsePosition(focus1, focus2, Eccentricity, i));
                }

                DrawLoop(orbitPoints, OrbitColors);
            }

            Texture2D glow;
            if (GlowOverride == "default") glow = Request<Texture2D>(Texture + "Aura").Value;
            else glow = Request<Texture2D>(GlowOverride).Value;

            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), Color.White, Projectile.rotation, new Vector2(glow.Width / 2, glow.Height / 2), Projectile.scale * 1.05f, SpriteEffects.None, 0f);

            //for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            //{
            //    Main.spriteBatch.Draw(glow, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), lightColor * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.oldRot[i], new Vector2(glow.Width / 2, glow.Height / 2), Projectile.scale * new Vector2(1, ((4 * Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 4)), SpriteEffects.None, 0f);
            //}

            return true;
        }
    }

    public class Mercury : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Mercury";
        public override float DistanceMultiplier => 0.25f;
        public override float Eccentricity => 0.8f;
        public override float OrbitalSpeed => 0.25f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 50;
            Projectile.height = 50;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60 * 4);
        }
    }
    public class Venus : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Venus";
        public override float DistanceMultiplier => 0.35f;
        public override float Eccentricity => 0.6f;
        public override float OrbitalSpeed => 0.04f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 66;
            Projectile.height = 66;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60 * 3);
        }
    }
    public class Earth : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Earth";
        public override float DistanceMultiplier => 0.4f;
        public override float Eccentricity => 0.5f;
        public override float OrbitalSpeed => 0.03f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 66;
            Projectile.height = 66;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60 * 2);
        }
    }
    public class Mars : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Mars";
        public override float DistanceMultiplier => 0.55f;
        public override float Eccentricity => 0.5f;
        public override float OrbitalSpeed => 0.02f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 66;
            Projectile.height = 66;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60 * 1);
        }
    }
    public class Jupiter : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Jupiter";
        public override float DistanceMultiplier => 0.6f;
        public override float Eccentricity => 0.3f;
        public override float OrbitalSpeed => 0.01f;
        public override Color[] OrbitColors => new Color[] { Color.Cyan, Color.DeepSkyBlue, Color.DodgerBlue };

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.scale = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 60 * 1);
        }
    }
    public class Saturn : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Saturn";
        public override float DistanceMultiplier => 0.7f;
        public override float Eccentricity => 0.25f;
        public override float OrbitalSpeed => 0.009f;
        public override Color[] OrbitColors => new Color[] { Color.Cyan, Color.DeepSkyBlue, Color.DodgerBlue };

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 110;
            Projectile.height = 50;
            Projectile.scale = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 60 * 2);
        }
    }
    public class Uranus : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Uranus";
        public override float DistanceMultiplier => 0.9f; // WHAT
        public override float Eccentricity => 0.25f;
        public override float OrbitalSpeed => 0.006f;
        public override Color[] OrbitColors => new Color[] { Color.Cyan, Color.DeepSkyBlue, Color.DodgerBlue };

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 66;
            Projectile.height = 86;
            Projectile.scale = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 60 * 3);
        }
    }
    public class Neptune : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Neptune";
        public override float DistanceMultiplier => 0.9f; // WHAT
        public override float Eccentricity => 0.2f;
        public override float OrbitalSpeed => 0.0075f;
        public override Color[] OrbitColors => new Color[] { Color.Cyan, Color.DeepSkyBlue, Color.DodgerBlue };

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.scale = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 60 * 4);
        }
    }
    public class Asteroid : EclipxieOrbPlanet
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/Asteroid";
        public override float DistanceMultiplier => 0.7f;
        public override float Eccentricity => 0.5f;
        public override float OrbitalSpeed => -0.04f;
        public override Color[] OrbitColors => new Color[] { };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.frame = Main.rand.Next(0, 3);
            Eccentricity += Main.rand.NextFloat(-0.1f, 0.1f);
            DistanceMultiplier += Main.rand.NextFloat(-0.1f, 0.1f);
            OrbitalSpeed += Main.rand.NextFloat(-0.02f, 0.02f);
            GlowOverride = "Polarities/Content/Items/Weapons/Summon/Flawless/EclipxiePlanets/AsteroidAura" + Projectile.frame;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        bool shmorp = true;
        public override void AI()
        {
            base.AI();
            if (shmorp)
            {
                t = Main.rand.NextFloat(0, MathHelper.TwoPi);
                shmorp = false;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 15;
            modifiers.FinalDamage /= 4;
        }
    }
}