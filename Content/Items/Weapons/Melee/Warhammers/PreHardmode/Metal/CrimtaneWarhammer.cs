using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Weapons.Melee.Warhammers;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.PreHardmode.Metal
{
    public class CrimtaneWarhammer : WarhammerBase
    {
        public override int HammerLength => 52;
        public override int HammerHeadSize => 14;
        public override int DefenseLoss => 12;
        public override int DebuffTime => 720;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(28, 13, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 66;
            Item.height = 66;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}