using Polarities.Content.Projectiles;
using Polarities.Global;
using Polarities.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Flawless
{
	public class EntropicSeal : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = (1);
			PolaritiesItem.IsFlawless.Add(Type);
			//DisplayName.SetDefault("Entropic Seal");
			//Tooltip.SetDefault("Press the arrow keys to teleport"+"\nYou cannot move normally or use hooks or mounts"+"\nProvides immunity to chaos state"+"\nFlawless");
		}

		public override void SetDefaults() {
			Item.width = 50;
			Item.height = 50;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 10);

			Item.rare = RarityType<LunaticCultistFlawlessRarity>();
		}
	}
}
