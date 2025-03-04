using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture;
using Polarities.Content.Items.Placeable.Blocks.Fractal;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Furniture.Fractal
{
    public class FractalCandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Candle);
            Item.createTile = ModContent.TileType<FractalCandleTile>();
            Item.placeStyle = 1;
            Item.flame = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalBrick>(), 5)
                .AddIngredient(ItemID.Torch)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class FractalCandleTile : CandleTileBase
    {
        public override int MyDustType => ModContent.DustType<FractalMatterDust>();
        public override int DropItem => ModContent.ItemType<FractalCandle>();
        public override Color LightColor => new(0.75f, 0.75f, 1f);
    }
}
