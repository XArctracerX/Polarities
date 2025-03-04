using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture;
using Polarities.Content.Items.Placeable.Blocks.Fractal;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Furniture.Fractal
{
    public class FractalToilet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenChair);
            Item.createTile = ModContent.TileType<FractalToiletTile>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalBrick>(), 6)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }

    public class FractalToiletTile : ToiletTileBase
    {
        // public override int MyDustType => ModContent.DustType<FractalMatterDust>();

        // public override int DropItem => ModContent.ItemType<FractalToilet>();
    }
}