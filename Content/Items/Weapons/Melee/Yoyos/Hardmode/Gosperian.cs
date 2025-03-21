using Polarities.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace Polarities.Content.Items.Weapons.Melee.Yoyos.Hardmode
{
	public class Gosperian : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Gosperian");
			//Tooltip.SetDefault("Releases waves of spikes");

			// These are all related to gamepad controls and don't seem to affect anything else
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.useStyle = 5;
			Item.width = 38;
			Item.height = 32;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 16f;
			Item.knockBack = 3.5f;
			Item.damage = 32;
			Item.rare = ItemRarityID.Pink;

			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.UseSound = SoundID.Item1;
			Item.value = 50000;
			Item.shoot = ProjectileType<GosperianProjectile>();
		}
	}

    public class GosperianProjectile : ModProjectile
	{
        private int timer;

		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Gosperian");
            
			// The following sets are only applicable to yoyo that use aiStyle 99.
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 15f;
			// YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
			// Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 280f;
			// YoyosTopSpeed is top speed of the yoyo Projectile. 
			// Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15f;
		}

		public override void SetDefaults() {
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			// aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}
		// notes for aiStyle 99: 
		// localAI[0] is used for timing up to YoyosLifeTimeMultiplier
		// localAI[1] can be used freely by specific types
		// ai[0] and ai[1] usually point towards the x and y world coordinate hover point
		// ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
		// ai[0] being negative makes the yoyo move back towards the player
		// Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.

		/*public override void PostAI() {
			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
				dust.noGravity = true;
				dust.scale = 0.75f;
			}
		}*/

        public override void AI() {
            timer++;
            if (timer == 20) {
                timer = 0;

                //spawn projectiles
                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
                for (int i=0; i<6; i++) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center,new Vector2(8,0).RotatedBy(Projectile.rotation + i*MathHelper.Pi/3),ProjectileType<GosperianSpike>(),(2*Projectile.damage)/3,Projectile.knockBack,Projectile.owner);
                }
            }
        }
	}

    public class GosperianSpike : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.alpha = 0;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
		}

		public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI/2;
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.95f;
		}
	}
}
