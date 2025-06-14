using Polarities.Tiles;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Trophies
{
	public class PolaritiesTrophy : ModItem
	{
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 50000;
			Item.rare = 1;
			Item.createTile = TileType<PolaritiesTrophyTile>();
			Item.placeStyle = 0;
		}
	}
}