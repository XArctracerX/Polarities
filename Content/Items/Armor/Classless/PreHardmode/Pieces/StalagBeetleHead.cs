using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Armor.Classless.PreHardmode.Pieces
{
    [AutoloadEquip(EquipType.Head)]
    public class StalagBeetleHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 10;
            Item.value = 2000;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Darkness, 2);
        }
    }
}

