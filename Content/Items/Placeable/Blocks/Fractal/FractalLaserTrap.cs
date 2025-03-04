using Terraria.ModLoader;

namespace Polarities.Content.Items.Placeable.Blocks.Fractal
{
    public class FractalLaserTrap : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<FractalTrap>(), 1);
        }
    }
}
