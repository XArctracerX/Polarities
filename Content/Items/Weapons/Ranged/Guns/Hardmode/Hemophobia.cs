using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Projectiles;
using System;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Guns.Hardmode
{
	public class Hemophobia : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Hemophobia");
			//Tooltip.SetDefault("Creates blood explosions on striking enemies");
		}

		public override void SetDefaults()
		{
			Item.damage = 112;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 48;
            Item.height = 24;
            Item.useTime = 32;
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 10f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -8);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //int shot = Projectile.NewProjectile(source, position + new Vector2(Main.rand.NextFloat(2)).RotatedByRandom(MathHelper.TwoPi), velocity * Main.rand.NextFloat(0.9f, 1.1f), type, damage, knockback, player.whoAmI);
            //Main.Projectile[shot].GetGlobalProjectile<PolaritiesProjectile>().bloodsplosion = true;
            Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)].GetGlobalProjectile<PolaritiesProjectile>().bloodsplosion = true;
            return false;
        }
    }

    public class Bloodsplosion : ModProjectile
    {
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodsplosion");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.Center);

            for (int i = 0; i < 128; i++)
            {
                Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(96), 0).RotatedByRandom(MathHelper.TwoPi);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Blood, Velocity: (dustPos - Projectile.Center) / 24, Scale: 2f);
                dust.noGravity = true;
            }
        }
    }

    public class BloodsplosionProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/Hemorrphage/BloodSpray";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Spray");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 16;
            Projectile.height = 16;
            DrawOffsetX = 3;
            DrawOriginOffsetY = -2;

            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

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

            Projectile.velocity.Y += 0.15f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        private void HitEffect()
        {
            Projectile.friendly = false;
            if (Projectile.timeLeft > Projectile.oldPos.Length + 2)
            {
                Projectile.timeLeft = Projectile.oldPos.Length + 2;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HitEffect();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HitEffect();
            return false;
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