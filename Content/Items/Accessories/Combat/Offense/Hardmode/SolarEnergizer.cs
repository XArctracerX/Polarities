using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Polarities.Content.Items.Accessories.Combat.Offense.Hardmode
{
    public class SolarEnergizer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 28;
            Item.accessory = true;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PolaritiesPlayer>().solarEnergizer = true;
            player.manaCost -= 0.1f;
        }
    }
}