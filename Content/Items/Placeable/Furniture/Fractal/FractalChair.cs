using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture;
using Polarities.Content.Items.Placeable.Blocks.Fractal;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Furniture.Fractal
{
    public class FractalChair : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenChair);
            Item.createTile = ModContent.TileType<FractalChairTile>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalBrick>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class FractalChairTile : ChairTileBase
    {
        // public override int MyDustType => ModContent.DustType<FractalMatterDust>();
        // public override int DropItem => ModContent.ItemType<FractalChair>();
    }
}