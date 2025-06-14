using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Combat.Defense.Hardmode
{
	public class SolarPendant : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Solar Pendant");
			//Tooltip.SetDefault("You will take 50% less damage from a single hit"+"\nThis effect has a 10s cooldown, during which damage dealt and recieved is doubled");
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 42;
			Item.accessory = true;
			Item.value = 150000;
			Item.rare = ItemRarityID.Lime;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<PolaritiesPlayer>().solarPendant = true;
		}
	}
}
