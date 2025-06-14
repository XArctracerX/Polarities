using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Polarities.Content.Items.Weapons.Melee.Misc
{
    public class Heatsaw : ModItem
    {

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;

            Item.width = 36;
            Item.height = 11;

            Item.damage = 65;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 1;
            Item.useAnimation = 20;
            Item.reuseDelay = 40;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;


            Item.shoot = ModContent.ProjectileType<HeatsawPellet>();
            Item.shootSpeed = 23;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * Item.width * 0.5f;
            Vector2 normalOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)).RotatedBy(MathF.PI / 2f) * Item.height * 0.5f;

            position += muzzleOffset;
            if (player.altFunctionUse == 2) 
            position -= normalOffset;
        }

        int shots = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * Item.width * 0.5f;
            Vector2 normalOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)).RotatedBy(MathF.PI / 2f) * Item.height * 0.5f;

            if (player.altFunctionUse == 2)
            {
                SoundEngine.PlaySound(SoundID.Item38, position);

                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.reuseDelay = 40;
                Projectile.NewProjectileDirect(source, position + muzzleOffset, velocity * 0.25f, ModContent.ProjectileType<HeatsawLava>(), damage, knockback, player.whoAmI);
                for (int i = 0; i < 6; i++)
                {
                    Vector2 newVel = (velocity * 0.25f) + Main.rand.NextVector2Circular(3, 3);
                    Projectile.NewProjectileDirect(source, position + muzzleOffset, newVel, ModContent.ProjectileType<HeatsawLava>(), damage, knockback, player.whoAmI);
                }
            }
            else
            {
                if (shots++ % 20 == 1) SoundEngine.PlaySound(SoundID.Item22, position);

                Item.useTime = 1;
                Item.useAnimation = 20;
                Item.reuseDelay = 40;

                Projectile.NewProjectileDirect(source, position + muzzleOffset, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position + muzzleOffset + normalOffset, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}