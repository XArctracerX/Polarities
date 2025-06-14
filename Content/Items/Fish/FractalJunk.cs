using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Polarities.Content.Items.Fish
{
	public class BranchedTwig : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 28;
			Item.maxStack = 9999;
			Item.value = 0;
			Item.rare = ItemRarityID.Gray;
		}
	}

	public class FractalWeeds : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 38;
			Item.maxStack = 9999;
			Item.value = 0;
			Item.rare = ItemRarityID.Gray;
		}
	}
}