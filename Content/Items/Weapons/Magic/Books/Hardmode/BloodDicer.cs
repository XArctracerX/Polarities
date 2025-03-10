using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Polarities.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Books.Hardmode
{
	public class BloodDicer : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Blood Dicer");
			//Tooltip.SetDefault("Shoots sets of four homing bloodsaws");
		}

		public override void SetDefaults()
		{
			Item.damage = 144;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;

            Item.width = 32;
            Item.height = 28;

            Item.useTime = 10;
            Item.useAnimation = 40;

            Item.reuseDelay = 30;
            Item.useStyle = 5;

            Item.noMelee = true;
            Item.knockBack = 5;

            Item.value = Item.sellPrice(gold:10);
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item8;

            Item.autoReuse = true;
            Item.shoot = ProjectileType<FriendlyBloodsaw>();
            Item.shootSpeed = 6f;
		}
    }

    public class FriendlyBloodsaw : ModProjectile
    {
        public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/Hemorrphage/HomingClot";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodsaw");
            //ProjectileID.Sets.Homing[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.light = 1f;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
            }

            Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(26), 0).RotatedByRandom(MathHelper.TwoPi);
            Dust dust = Dust.NewDustPerfect(dustPos, DustID.Blood, Velocity: Projectile.velocity, Scale: 1.75f);
            dust.noGravity = true;

            if (Projectile.timeLeft > 1080)
            {
                Projectile.velocity.Y += 0.15f;
            }
            else
            {
                int targetID = -1;
                Projectile.Minion_FindTargetInRange(750, ref targetID, true);
                NPC target = null;
                if (targetID != -1)
                {
                    target = Main.npc[targetID];

                    Vector2 goalVelocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;
                    Projectile.velocity += (goalVelocity - Projectile.velocity) / 90;
                }
                else
                {
                    Projectile.velocity.Y += 0.15f;
                }
            }

            Projectile.rotation += 0.25f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            fallThrough = true;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
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

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(26), 0).RotatedByRandom(MathHelper.TwoPi);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Blood, Velocity: Projectile.velocity, Scale: 1.75f);
                dust.noGravity = true;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}