using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
    public class ChaosFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToAccessory(24, 24);
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Yellow;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PolaritiesPlayer>().orbMinionSlots += 1;
        }
    }
}
