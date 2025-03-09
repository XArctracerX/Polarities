using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Trophies
{
    public class HemorrphageTrophy : ModItem
    {
        public override void SetDefaults()
        {         
            Item.DefaultToPlaceableTile(ModContent.TileType<HemorrphageTrophyTile>());

            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Blue;
            Item.value = 50000;
        }
    }
}