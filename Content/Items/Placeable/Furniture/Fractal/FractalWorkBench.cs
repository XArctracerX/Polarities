using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture;
using Polarities.Content.Items.Placeable.Blocks.Fractal;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Furniture.Fractal
{
    public class FractalWorkBench : WorkBenchBase
    {
        public override int PlaceTile => ModContent.TileType<FractalWorkBenchTile>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalMatter>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class FractalWorkBenchTile : WorkBenchTileBase
    {
        public override int MyDustType => ModContent.DustType<FractalMatterDust>();
        public override int DropItem => ModContent.ItemType<FractalWorkBench>();
    }
}
