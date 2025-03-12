using Microsoft.Xna.Framework;
using Polarities.Global;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Vanity.Hardmode
{
	[AutoloadEquip(EquipType.Head)]
	public class SelfsimilarSentinelMask : ModItem
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