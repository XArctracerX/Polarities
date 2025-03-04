using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Accessories.Movement.PreHardmode
{
    public class HopperCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.accessory = true;
            Item.value = 2500;
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpSpeedBoost += 1.2f;
            player.autoJump = true;
            player.GetModPlayer<PolaritiesPlayer>().hopperCrystal = true;
        }
    }
}