using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Materials.PreHardmode
{
    public class BlueQuartz : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (25);
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
        }
    }
}