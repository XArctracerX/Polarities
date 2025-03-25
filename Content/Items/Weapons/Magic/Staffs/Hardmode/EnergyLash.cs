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
	public class EnergyLash : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Energy Lash");
            //Tooltip.SetDefault("Shoots a branching beam of fractal energy");
            Item.staff[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 150;
			Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
            Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 100000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<EnergyLashProjectile>();
			Item.shootSpeed = 0.001f;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Projectile.NewProjectile(source, position,new Vector2(Main.MouseWorld.X-player.Center.X,Main.MouseWorld.Y-player.Center.Y).SafeNormalize(new Vector2(1,0)),type,damage,knockback,player.whoAmI, ai0: 31, ai1: MathHelper.PiOver2-(float)Math.Atan((Main.MouseWorld-player.Center).Length()/240));
            return false;
		}
	}

    public class EnergyLashProjectile : ModProjectile
	{
        public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Energy Lash");
            Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.light = 0.75f;
            Projectile.timeLeft = 40;
			Projectile.alpha = 255;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI() {
			if (Projectile.localAI[0] == 0)
            {
				Projectile.localAI[0] = 1;

				Projectile.alpha = 3;
			}

            Projectile.rotation = Projectile.velocity.ToRotation()-MathHelper.PiOver2;

            Projectile.alpha += 6;

            if (Projectile.ai[0] != 0) {
                Projectile.frame = 1;
            }

            if (Projectile.timeLeft == 39) {
                if (Projectile.ai[0] == 1 || Projectile.ai[0] == 2 || Projectile.ai[0] == 4 || Projectile.ai[0] == 8 || Projectile.ai[0] == 16) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center+Projectile.velocity*28, Projectile.velocity.RotatedBy(Projectile.ai[1]) ,Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, ai0: Projectile.ai[0]-1, ai1: Projectile.ai[1]);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center+Projectile.velocity*28, Projectile.velocity.RotatedBy(-Projectile.ai[1]) ,Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, ai0: Projectile.ai[0]-1, ai1: Projectile.ai[1]);
                } else if (Projectile.ai[0] != 0) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center+Projectile.velocity*31, Projectile.velocity ,Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, ai0: Projectile.ai[0]-1, ai1: Projectile.ai[1]);
                }
            }
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(new Vector2(targetHitbox.X,targetHitbox.Y),targetHitbox.Size(),Projectile.Center,Projectile.Center+(new Vector2(32,0)).RotatedBy(Projectile.rotation+MathHelper.PiOver2), 16 , ref point);
		}

        public override bool? CanCutTiles()
        {
			return false;
        }

        public override bool ShouldUpdatePosition() => false;
	}
}