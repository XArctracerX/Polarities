using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using System;
using System.IO;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
	public class SturdyBloodTablet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Sturdy Blood Tablet");
			//Tooltip.SetDefault("Summons the Blood Moon"+"\nNot consumable");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SolarTablet);
			Item.consumable = false;
			Item.maxStack = 1;
		}

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !Main.bloodMoon && !FractalSubworld.Active;
		}

		public override bool? UseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			Main.NewText(Lang.misc[8].Value, 50, byte.MaxValue, 130);
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.bloodMoon = true;
			}
			else
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write("TriggerEvent");
				packet.Write((int)0);
				packet.Send();
			}

			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BloodMoonStarter)
				.AddIngredient(ItemType<Materials.Hardmode.HemorrhagicFluid>(), 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}