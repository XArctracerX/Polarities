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
    public class Fractlatl : AtlatlBase
    {
        public override Vector2[] ShotDistances => new Vector2[] { new Vector2(41) };

        //DisplayName.SetDefault("Fractlatl");
        //Tooltip.SetDefault("Shoots additional projectiles more horizontally and vertically");

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(40, 6, 4);
            Item.DamageType = DamageClass.Ranged;

            Item.width = 52;
            Item.height = 52;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Dart;

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Pink;
        }

        public override bool RealShoot(Player player, EntitySource_ItemUse_WithAmmo source, int index, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, 0), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, new Vector2(0, velocity.Y), type, damage, knockback, player.whoAmI);
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
