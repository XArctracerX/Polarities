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
    public class FractalChandelier : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CopperChandelier);
            Item.createTile = ModContent.TileType<FractalChandelierTile>();
            Item.placeStyle = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalBrick>(), 4)
                .AddIngredient(ItemID.Torch, 4)
                .AddIngredient(ItemID.Chain)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class FractalChandelierTile : ChandelierTileBase
    {
        public override int MyDustType => ModContent.DustType<FractalMatterDust>();
        public override int DropItem => ModContent.ItemType<FractalChandelier>();
        public override Color LightColor => new(0.75f, 0.75f, 1f);
        public override bool DieInWater => false;
    }
}