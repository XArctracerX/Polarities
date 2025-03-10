using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Polarities.Content.Items.Weapons.Melee.Warhammers;
using Polarities.Content.Buffs.PreHardmode;
using Polarities.Content.Items.Placeable.Blocks;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.PreHardmode.Other
{
    public class RiftOnAStick : WarhammerBase
    {
        public override int HammerLength => 48;
        public override int HammerHeadSize => 16;
        public override int DefenseLoss => 20;
        public override int DebuffTime => 600;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(34, 14, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 48;
            Item.height = 48;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Blue;
        }
    }
}

