using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Materials.Hardmode
{
	public class EclipxieDust : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Eclipxie Dust");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 10));
		}

		public override void SetDefaults() {
			Item.width = 24;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = 5000;
            Item.rare = ItemRarityID.Yellow;
		}
	}
}