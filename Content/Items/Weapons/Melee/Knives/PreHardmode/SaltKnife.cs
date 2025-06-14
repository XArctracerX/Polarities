using Microsoft.Xna.Framework;
using System;
using Polarities.Global;
using Polarities.Content.Items.Placeable.Blocks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Knives.PreHardmode
{
    public class SaltKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (5);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(10, 1, 0);
            Item.DamageType = DamageClass.Melee;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.shoot = ProjectileType<SaltKnifeProjectile>();
            Item.shootSpeed = 1f;

            Item.width = 32;
            Item.height = 30;

            Item.useTime = 8;
            Item.useAnimation = 8;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 5)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, 0, 40);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SaltCrystals>(), 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SaltKnifeProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Melee/Knives/PreHardmode/SaltKnife";

        private float rotationOffset;
        private int timer;
        private int atkCooldown;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("{$Mods.Polarities.ItemName.SaltKnife}");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 40;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active)
            {
                Projectile.active = false;
            }

            timer++;
            atkCooldown--;
            Projectile.friendly = atkCooldown < 0;

            rotationOffset = -player.direction * 0.5f * (float)Math.Cos(Projectile.ai[0] + timer * 2 * Math.PI / Projectile.ai[1]);
            Projectile.rotation = rotationOffset + (Main.MouseWorld - player.MountedCenter).ToRotation() + MathHelper.Pi / 4;
            Projectile.position = -(new Vector2(Projectile.width / 2, Projectile.height / 2)) + player.MountedCenter + (new Vector2(1, -1)).RotatedBy(Projectile.rotation) * Projectile.width * 0.5f * (1 + 0.2f * (1 + (float)Math.Sin(Projectile.ai[0] + timer * 2 * Math.PI / Projectile.ai[1])));

            Projectile.velocity = Vector2.Zero;

            player.itemLocation = player.MountedCenter + (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero) * 10f;
            player.itemRotation = (float)Math.IEEERemainder((Main.MouseWorld - player.MountedCenter).ToRotation() + (player.direction == 1 ? 0 : MathHelper.Pi), MathHelper.TwoPi);
        }

        //public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        //{
            //hitDirection = Main.player[Projectile.owner].direction;
        //}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            atkCooldown = (int)Projectile.ai[1] / 2;
        }
    }
}