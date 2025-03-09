using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Vanity.Hardmode
{
    [AutoloadEquip(EquipType.Head)]
    public class HemorrphageMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}