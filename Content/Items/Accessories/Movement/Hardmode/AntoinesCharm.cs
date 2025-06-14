using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;

namespace Polarities.Content.Items.Accessories.Movement.Hardmode
{
	public class AntoinesCharm : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Antoine's Charm");
		}

		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
		{
			try
			{
				string hotkey = "an unbound hotkey";
				if (Polarities.AntoinesCharmHotKey.GetAssignedKeys().ToArray().Length > 0)
				{
					hotkey = Polarities.AntoinesCharmHotKey.GetAssignedKeys()[0];
				}
				TooltipLine line = new TooltipLine(Mod, "Tooltip1", string.Format("Allows you to teleport a short distance by pressing {0}", hotkey));
				tooltips.Insert(2, line);
			}
			catch (Exception e)
            {

            }
		}

		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 20;
			Item.accessory = true;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<PolaritiesPlayer>().antoineCharm = true;
		}
	}
}
