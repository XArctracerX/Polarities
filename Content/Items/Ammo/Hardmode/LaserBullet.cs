using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Polarities.Content.Items.Ammo.Hardmode
{
	public class LaserBullet : ModItem
	{
		//public override void SetStaticDefaults()
		//{
		//	DisplayName.SetDefault("Laser Bullet");
		//	Tooltip.SetDefault("Shoots a laser");
		//}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 6;
			Item.height = 18;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 0f;
			Item.value = Item.sellPrice(0, 0, 0, 2);
			Item.rare = 8;
			Item.shoot = ProjectileType<LaserBulletProjectile>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 0f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.MartianConduitPlating)
				.AddIngredient(ItemID.EmptyBullet, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

    public class LaserBulletProjectile : ModProjectile
    {
        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("Laser Bullet");
        //}

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 5120;
            Projectile.extraUpdates = 1024;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

		private Vector2 startingLocation;

        public override void AI()
		{
			if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;

				startingLocation = Projectile.Center;

				SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero)*4;
				Projectile.timeLeft = 4 * (1+Projectile.extraUpdates);
				Projectile.knockBack = 0;
			}

			if ((Projectile.Center + Projectile.velocity).X < 0 || (Projectile.Center + Projectile.velocity).X > Main.maxTilesX * 16 || (Projectile.Center + Projectile.velocity).Y < 0 || (Projectile.Center + Projectile.velocity).Y > Main.maxTilesY * 16)
			{
				Projectile.friendly = false;
				Projectile.velocity = Vector2.Zero;
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate++;
			Projectile.friendly = false;
			Projectile.velocity = Vector2.Zero;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.friendly = false;
			Projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 mainCenter = startingLocation;
			Vector2 center = Projectile.Center;
			Vector2 distToProj = mainCenter - center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();

			if (Projectile.localAI[0] > 0)
			{
				Color drawColor = Color.White;
				int numDraws = 2;
				drawColor.A = 127;
				for (int x = 0; x < numDraws; x++)
				{
					while (distance > 4f && !float.IsNaN(distance))
					{
						distToProj.Normalize();                 //get unit vector

						//this uses the normalized one
						Vector2 laserOffset = distToProj.RotatedBy(MathHelper.PiOver2) * (float)Math.Sin(MathHelper.TwoPi * x / numDraws + (1 - Projectile.timeLeft / (5f * Projectile.extraUpdates)));

						distToProj *= 4f;                      //speed = 4
						center += distToProj;                   //update draw position
						distToProj = mainCenter - center;    //update distance
						distance = distToProj.Length();


						//Draw chain
						Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, laserOffset + new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
							new Rectangle(0, 0, 4, 4), drawColor, projRotation,
							new Vector2(4 * 0.5f, 4 * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
	}
}
