using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;
using Polarities.Content.Buffs.Hardmode;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace Polarities.Content.Items.Accessories.ExpertMode.Hardmode
{
	public class SolarCalibrator : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Solar Calibrator");
			// Tooltip.SetDefault("Gives the player a guarantee to crit enemies for a short time after dealing a certain amount of damage");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 27));
			//ItemID.Sets.AnimatesAsSoul[item.type] = true;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 38;

			Item.accessory = true;
			Item.value = 40000*5;
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PolaritiesPlayer>().solarCalibrator = true;
		}

		public override void UpdateInventory(Player player) {
			Item.rare = ItemRarityID.Expert;
		}
	}
}
