using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Consumables.Keys.Hardmode
{
    public class FractalKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
        }
    }
}