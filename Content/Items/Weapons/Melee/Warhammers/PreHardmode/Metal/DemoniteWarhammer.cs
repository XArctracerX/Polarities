using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Weapons.Melee.Warhammers;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.PreHardmode.Metal
{
    public class DemoniteWarhammer : WarhammerBase
    {
        public override int HammerLength => 46;
        public override int HammerHeadSize => 12;
        public override int DefenseLoss => 12;
        public override int DebuffTime => 600;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(20, 13, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 58;
            Item.height = 58;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}