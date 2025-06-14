using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
	public class SnakeEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Rattler Emblem");
			//Tooltip.SetDefault("17% increased melee damage" + "\nAttacks inflict venom and poison");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.17f;
			player.GetModPlayer<PolaritiesPlayer>().splashOfVenom = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SummonerEmblem)
				.AddIngredient(ItemType<SplashOfVenom>())
				.AddIngredient(ItemID.HallowedBar, 6)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
