using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Boomerangs.Hardmode
{
	public class CosmicDiscs : ModItem
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Lunarang");
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults() {
			Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
			Item.width = 88;
			Item.height = 88;
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 80000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<Lunarang>();
			Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
			{
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.active)
                    {
                        if ((projectile.type == ProjectileType<Lunarang>() || projectile.type == ProjectileType<Solarang>()) && projectile.owner == player.whoAmI)
                        {
                            projectile.ai[0] = 2;
                        }
                    }
                }
            }
			else
			{
				if (player.ownedProjectileCounts[ProjectileType<Lunarang>()] <= 0)
				{
					Projectile.NewProjectile(source, position, velocity.RotatedBy((float)Math.PI * 0.125f), ProjectileType<Lunarang>(), damage, knockback, player.whoAmI);
				}
                if (player.ownedProjectileCounts[ProjectileType<Solarang>()] <= 0)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedBy((float)Math.PI * -0.125f), ProjectileType<Solarang>(), damage, knockback, player.whoAmI);
                }
            }
			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				return player.ownedProjectileCounts[ProjectileType<Lunarang>()] > 0 || player.ownedProjectileCounts[ProjectileType<Solarang>()] > 0;
			}
			else
			{
				return player.ownedProjectileCounts[ProjectileType<Lunarang>()] <= 0 || player.ownedProjectileCounts[ProjectileType<Solarang>()] <= 0;
			}
		}

		public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemType<SunDisc>())
				.AddIngredient(ItemType<MoonDisc>())
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
	}
}