using Microsoft.Xna.Framework;
using Polarities.Content.Items.Weapons.Ranged.Atlatls;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Atlatls.PreHardmode
{
    public class Ratlatl : AtlatlBase
    {
        public override Vector2[] ShotDistances => new Vector2[] { new Vector2(30) };

        public override void SetDefaults()
        {
            Item.SetWeaponValues(16, 3, 0);
            Item.DamageType = DamageClass.Ranged;

            Item.width = 30;
            Item.height = 38;

            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = Sounds.Rattle;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Dart;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileID.PoisonDartBlowgun;

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                //.AddRecipeGroup("WoodenAtlatl")
                .AddIngredient(ItemType<Rattle>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
