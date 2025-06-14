using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Accessories.Combat.Defense.Hardmode
{
    public class FractalAbsorber : ModItem
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

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 1);
        }

        int timer = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.Hardmode.Fractalizing>())
             && FractalSubworld.Active
             && timer % 3 == 0 || timer % 3 == 1) player.buffTime[player.FindBuffIndex(ModContent.BuffType<Buffs.Hardmode.Fractalizing>())]--;
            timer++;
        }
    }
}