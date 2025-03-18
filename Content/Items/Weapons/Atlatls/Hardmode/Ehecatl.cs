using Microsoft.Xna.Framework;
using Polarities.Content.Items.Weapons.Ranged.Atlatls;
using Polarities.Content.Items.Ammo.PreHardmode;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Atlatls.Hardmode
{
    public class Ehecatl : AtlatlBase
    {
        public override Vector2[] ShotDistances => new Vector2[] { new Vector2(36), new Vector2(42), new Vector2(48) };

        public override void SetDefaults()
        {
            Item.SetWeaponValues(20, 3, 4);
            Item.DamageType = DamageClass.Ranged;

            Item.width = 56;
            Item.height = 56;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = 10;
            Item.shootSpeed = 17f;
            Item.useAmmo = AmmoID.Dart;

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Pink;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            base.Shoot(player, source, position, velocity, type, damage, knockback);

            for (int i = 0; i < mostRecentShotTypes.Length; i++)
            {
                mostRecentShotTypes[2] = ProjectileType<WoodenDartProjectile>();
            }

            return false;
        }

        public override bool RealShoot(Player player, EntitySource_ItemUse_WithAmmo source, int index, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (index == 1)
            {
                velocity = velocity.RotatedByRandom(0.1f);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
