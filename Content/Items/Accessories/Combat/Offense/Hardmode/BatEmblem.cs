using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
	public class BatEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = (1);
			//DisplayName.SetDefault("Bat Emblem");
			//Tooltip.SetDefault("15% increased minion damage"+"\nGrants more minion slots the lower your health, up to a maximum of 3 extra minion slots");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold:2);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.15f;
			//player.minionDamage += 0.15f;
			player.maxMinions += (int)(4 - 4 * (float)player.statLife / (float)player.statLifeMax2);
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.SummonerEmblem)
				.AddIngredient(ItemType<PreHardmode.BatSigil>())
				.AddIngredient(ItemID.HallowedBar, 6)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
        }
    }
}
