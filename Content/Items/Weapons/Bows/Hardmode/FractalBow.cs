using Polarities.Global;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Polarities.Content.Items.Weapons.Ranged.Bows.Hardmode
{
	public class FractalBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Fractal Bow");
			//Tooltip.SetDefault("Shoots a mini fractal bow which fires an arrow at the cursor");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 54;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver:50);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile shot = Main.projectile[Projectile.NewProjectile(source, position, velocity/2f, ProjectileType<FractalBowMini>(), damage, knockback, player.whoAmI, type, Item.shootSpeed)];
			shot.timeLeft = Item.useTime/2 + 120;
			return true;
		}



		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Content.Items.Materials.Hardmode.FractalResidue>(), 3)
				.AddIngredient(ItemType<Content.Items.Placeable.Bars.FractalBar>(), 16)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
    }


	public class FractalBowMini : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Fractal Bow");
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			DrawOriginOffsetY = -6;

			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft > 120)
			{
				Projectile.velocity *= 0.95f;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();
				}
			} else if (Projectile.timeLeft == 120)
			{
				SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 displacement = Main.MouseWorld - Projectile.Center;
					Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, displacement.SafeNormalize(Vector2.Zero) * Projectile.ai[1], (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, Projectile.owner)].noDropItem = true;
				}
			} else
            {
				Projectile.tileCollide = false;
				Projectile.velocity.Y += 0.3f;
				Projectile.rotation += Projectile.velocity.X / 3;
            }
			Projectile.netUpdate = true;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.velocity.X != oldVelocity.X)
            {
				Projectile.velocity.X = oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = oldVelocity.Y;
			}
			return false;
        }
    }

	public class SelfsimilarBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Selfsimilar Bow");
			//Tooltip.SetDefault("Shoots four mini selfsimilar bows, which fire arrows at the cursor");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 34;
			Item.height = 62;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 4; i++)
			{
				Projectile shot = Main.projectile[Projectile.NewProjectile(source, position, new Vector2(Item.shootSpeed, 0).RotatedBy(i*MathHelper.TwoPi/4) / 2f + (velocity / 3), ProjectileType<SelfsimilarBowMini>(), damage, knockback, player.whoAmI, type, Item.shootSpeed)];
				shot.timeLeft = Item.useTime / 2 + 120;
			}
			damage *= 3;
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<FractalBow>())
				.AddIngredient(ItemType<Placeable.Bars.SelfsimilarBar>(), 10)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}


	public class SelfsimilarBowMini : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Selfsimilar Bow");
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			DrawOriginOffsetY = -6;

			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft > 120)
			{
				Projectile.velocity *= 0.95f;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();
				}
			}
			else if (Projectile.timeLeft == 120)
			{
				SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 displacement = Main.MouseWorld - Projectile.Center;
					Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, displacement.SafeNormalize(Vector2.Zero) * Projectile.ai[1], (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, Projectile.owner)].noDropItem = true;
				}
			}
			else
			{
				Projectile.tileCollide = false;
				Projectile.velocity.Y += 0.3f;
				Projectile.rotation += Projectile.velocity.X / 3;
			}
			Projectile.netUpdate = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = oldVelocity.Y;
			}
			return false;
		}
	}
}