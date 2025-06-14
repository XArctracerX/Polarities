using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
	public class SplashOfVenom : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Splash of Venom");
			//Tooltip.SetDefault("Attacks inflict venom and poison"+"\n5% increased damage");
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 36;
			Item.accessory = true;
			Item.value = 20000*5;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetModPlayer<PolaritiesPlayer>().splashOfVenom = true;
		}

        public override void AddRecipes() {
            CreateRecipe()
				.AddIngredient(ItemType<Materials.Hardmode.VenomGland>(),5)
				.AddIngredient(ItemType<Materials.PreHardmode.Rattle>())
				.AddTile(TileID.Anvils)
				.Register();
        }
	}
}
