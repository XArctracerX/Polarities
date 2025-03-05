using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Accessories.Combat.Defense.Hardmode
{
    public class HydraHide : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.accessory = true;

            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PolaritiesPlayer>().hydraHide = true;
        }
    }
}