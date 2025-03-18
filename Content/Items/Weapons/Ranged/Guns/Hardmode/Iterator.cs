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

namespace Polarities.Content.Items.Weapons.Ranged.Guns.Hardmode
{
	public class Iterator : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Iterator");
			//Tooltip.SetDefault("Shoots a fractal spread of bullets, which cause the weapon to shoot more bullets on hitting enemies");
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 70;
			Item.height = 28;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 1f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int[] shots = { 0, 2, 6, 8, 18, 20, 24, 26 };
			foreach (int i in shots)
			{
				Projectile shot = Main.projectile[Projectile.NewProjectile(source, position, velocity * ((i + 27) / 54f), type, damage, knockback, player.whoAmI, 0, 0)];

				shot.GetGlobalProjectile<Content.Projectiles.PolaritiesProjectile>().recurShotItem = Item;
			}
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10,0);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<CantorShot>())
				.AddIngredient(ItemType<Placeable.Bars.SelfsimilarBar>(), 10)
				.AddIngredient(ItemType<Placeable.Blocks.Fractal.DendriticEnergy>(), 32)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}
}