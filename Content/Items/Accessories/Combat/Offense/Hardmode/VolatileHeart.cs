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

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
	public class VolatileHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Volatile Heart");
			//Tooltip.SetDefault("Hitting enemies has a chance to create damaging bloodsplosions");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
            Item.height = 40;
            Item.accessory = true;
            Item.value = 150000;
            Item.rare = ItemRarityID.Lime;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PolaritiesPlayer>().volatileHeart = true;
		}
	}

    public class VolatileHeartProjectile : ModProjectile
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

    public class VolatileHeartBloodsplosion : ModProjectile
    {
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
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
}
