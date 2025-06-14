using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Polarities.Content.Items;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Movement.Hardmode // it is MOVING the items to you :)
{
	public class Electromagnet : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Electromagnet");
			//Tooltip.SetDefault("Significantly increases item pickup range");
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 7);
			Item.rare = ItemRarityID.Red;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            foreach (Item i in Main.item)
            {
				if (i.active && i.Distance(player.Center) > player.GetItemGrabRange(i) && i.Distance(player.Center) < player.GetItemGrabRange(i) + 1200)
                {
					i.velocity = 3 * i.DirectionTo(player.Center);
                }
            }
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CelestialMagnet)
				.AddIngredient(ItemType<Placeable.Bars.PolarizedBar>(), 6)
				.AddIngredient(ItemType<Materials.Hardmode.SmiteSoul>(), 6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
