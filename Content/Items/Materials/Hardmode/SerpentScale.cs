using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Materials.Hardmode
{
    public class SerpentScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (25);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.value = 100;
            Item.rare = ItemRarityID.LightRed;
        }
    }
}