using Polarities.Content.Items.Placeable.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Multi.PreHardmode
{
    public class SunplateHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(24, 10f, 0);
            Item.DamageType = DamageClass.Melee;

            Item.axe = 20;
            Item.hammer = 60;

            Item.width = 52;
            Item.height = 48;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = 4000;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SunplateBar>(), 10)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}