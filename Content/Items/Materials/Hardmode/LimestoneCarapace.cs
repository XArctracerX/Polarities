using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Materials.Hardmode
{
    public class LimestoneCarapace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (25);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Orange;
        }
    }
}