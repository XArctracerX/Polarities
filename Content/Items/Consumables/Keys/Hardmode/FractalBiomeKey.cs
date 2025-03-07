using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Consumables.Keys.Hardmode
{
    public class FractalBiomeKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.UsesCursedByPlanteraTooltip[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.HallowedKey);
        }
    }
}
