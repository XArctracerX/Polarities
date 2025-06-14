using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using System;
using System.IO;
using SubworldLibrary;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
	public class SturdySolarTablet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Sturdy Solar Tablet");
			//Tooltip.SetDefault("Summons the Eclipse" + "\nNot consumable");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SolarTablet);
			Item.consumable = false;
			Item.maxStack = 1;
		}

		public override bool CanUseItem(Player player)
		{
			return Main.dayTime && !Main.eclipse && !FractalSubworld.Active;
		}

		public override bool? UseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.eclipse = true;
				Main.NewText(Lang.misc[20].Value, 50, byte.MaxValue, 130);
			}
			else
			{
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, -6f);
			}

			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SolarTablet)
				.AddIngredient(ItemType<Materials.Hardmode.EclipxieDust>(), 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}