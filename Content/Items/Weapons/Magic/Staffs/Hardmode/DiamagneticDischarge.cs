using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Magic.Staffs.Hardmode
{
	public class DiamagneticDischarge : ModItem
	{
        public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Diamagnetic Discharge");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 175;
			Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 10;
			Item.useAnimation = 10;
            Item.autoReuse = true;
            Item.noMelee = true;
			Item.useStyle = 5;
            Item.UseSound = SoundID.Item20;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<ElectricDischarge>();
            Item.shootSpeed = 12f;
		}


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source,position,(1.5f * velocity).RotatedByRandom(MathHelper.PiOver4 / 2),ProjectileType<MagneticDischarge>(),3*damage/4,knockback,player.whoAmI);
            return true;
        }

        public override Vector2? HoldoutOrigin() {
            return new Vector2(10,10);
        }
	}
    
    public class ElectricDischarge : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Electric Discharge");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
            Projectile.extraUpdates = 127;
        }

        float lastAngle = 0;
        bool reachedCursor = false;
        public override void AI() {
            if (Projectile.ai[0] > 5)
            {
                Dust d = Dust.NewDustPerfect(Projectile.position, DustID.Electric, newColor: Color.LightBlue, Scale: 1f);
                d.noGravity = true;
                d.velocity /= 10;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }

            if (Projectile.ai[0]++ % 10 == 0)
            {
                //int targetID = -1;
                //Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: true);
                //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-lastAngle));
                //lastAngle = 40 * Main.rand.NextFloat(-1f, 1f) * Main.rand.NextFloat(-1f, 1f);
                //if (targetID != -1)
                //{
                //    lastAngle = Projectile.Center.DirectionTo(Main.npc[targetID].Center).ToRotation();
                //    Projectile.velocity = Projectile.velocity.Length() * lastAngle.ToRotationVector2();
                //}
                //else
                //{
                //    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(lastAngle));
                //}

                Vector2 newVelocityToCursor =  Projectile.velocity.Length() * Projectile.position.DirectionTo(Main.MouseWorld);
                newVelocityToCursor = newVelocityToCursor.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20, 20f)));
                if (Vector2.Dot(Projectile.velocity, newVelocityToCursor) < 0) reachedCursor = true; // projectile would turn around

                if (reachedCursor)
                {
                    lastAngle = Main.rand.NextFloat(-40f, 40f);
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(lastAngle));
                }
                else Projectile.velocity = newVelocityToCursor;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.Kill();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
		{
			for (int i=0; i<6; i++) {
				Main.dust[Dust.NewDust(Projectile.position-new Vector2(10,10),Projectile.width+20,Projectile.height+20, DustID.Electric ,newColor:Color.LightBlue, Scale:2f)].noGravity = true;
			}
		}
    }
    
    public class MagneticDischarge : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magnetic Discharge");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            int targetID = -1;
            Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
            NPC target = null;
            if (targetID != -1)
            {
                target = Main.npc[targetID];
            }

            if (target != null) {
                Vector2 targetVelocity = target.position - target.oldPosition;

                float v = Projectile.velocity.Length();
                float a = targetVelocity.Y;
                float b = targetVelocity.X;
                float c = (target.Center.X-Projectile.Center.X);
                float d = (target.Center.Y-Projectile.Center.Y);

                float internalVal = -a*a*c*c+2*a*b*c*d-b*b*d*d+v*v*(c*c+d*d);

                float theta = internalVal >= 0 ? 
                    2*(float)Math.Atan2(c*v - Math.Sqrt(internalVal),
                    a*c-d*(b+v))
                    :
                    targetVelocity.ToRotation();

                if (theta > MathHelper.Pi) {
                    theta -= MathHelper.TwoPi;
                } else if (theta < -MathHelper.Pi) {
                    theta += MathHelper.TwoPi;
                }

                float angleDiff = theta - Projectile.velocity.ToRotation();
                for(int i = 0; i<2; i++) {
                    if (angleDiff > MathHelper.Pi) {
                        angleDiff -= MathHelper.TwoPi;
                    } else if (angleDiff < -MathHelper.Pi) {
                        angleDiff += MathHelper.TwoPi;
                    }
                }

                Projectile.velocity = Projectile.velocity.RotatedBy(Math.Min(1f/Projectile.velocity.Length(),Math.Max(-1f/Projectile.velocity.Length(),angleDiff)));
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Main.dust[Dust.NewDust(Projectile.position,Projectile.width,Projectile.height, 235 ,newColor:Color.Red,Scale:2f)].noGravity = true;

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
		{
			for (int i=0; i<6; i++) {
				Main.dust[Dust.NewDust(Projectile.position-new Vector2(10,10),Projectile.width+20,Projectile.height+20, 235 ,newColor:Color.Red, Scale:2f)].noGravity = true;
			}
		}
    }
}