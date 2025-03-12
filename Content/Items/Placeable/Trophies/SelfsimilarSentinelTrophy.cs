using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Trophies
{
	public class SelfsimilarSentinelTrophy : ModItem
	{
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SelfsimilarSentinelTrophyTile>());

            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Blue;
            Item.value = 50000;
        }
    }
}