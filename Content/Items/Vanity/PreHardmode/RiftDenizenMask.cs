using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Vanity.PreHardmode
{
	[AutoloadEquip(EquipType.Head)]
	public class RiftDenizenMask : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            //ArmorIDs.Head.Sets.DrawBackHair[equipSlotHead] = true;
            ArmorIDs.Head.Sets.DrawHatHair[equipSlotHead] = false;
            ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}