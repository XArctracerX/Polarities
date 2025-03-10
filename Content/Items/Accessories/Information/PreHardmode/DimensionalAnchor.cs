using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Information.PreHardmode
{
	public class DimensionalAnchor : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Dimensional Anchor");
			//Tooltip.SetDefault("Causes you to respawn in the fractal dimension if you die there"+"\nWorks when in the inventory");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
            Item.height = 34;

            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalDimensionRespawn = true;
		}
	}
}
