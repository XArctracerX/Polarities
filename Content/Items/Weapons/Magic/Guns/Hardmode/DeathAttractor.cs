using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Polarities.Projectiles;
using System;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Magic.Guns.Hardmode
{
    public class DeathAttractor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Death Attractor");
            //Tooltip.SetDefault("Attracts deadly metal shards");
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 54;
            Item.height = 22;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item93;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DeathAttractorShard>();
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + (Main.MouseWorld - player.Center).SafeNormalize(new Vector2(0, -1)) * 1200;
            position += Main.rand.NextVector2Circular(150, 150);
            velocity = position.DirectionTo(player.Center) * velocity.Length();
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes() {
            //    ModRecipe recipe = new ModRecipe(mod);
            //    recipe.AddIngredient(ItemType<Items.Placeable.PolarizedBar>(),16);
            //    recipe.AddIngredient(ItemType<SmiteSoul>(),6);
            //    recipe.AddTile(TileID.MythrilAnvil);
            //    recipe.SetResult(this);
            //    recipe.AddRecipe();
            CreateRecipe()
                    .AddIngredient(ItemType<Placeable.Bars.PolarizedBar>(), 16)
                    .AddIngredient(ItemType<Materials.Hardmode.SmiteSoul>(), 6)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
        }
    }

    public class DeathAttractorShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Metal Shard");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if ((player.Center - Projectile.Center).Length() < 40)
            {
                Projectile.Kill();
            }

            Vector2 playerVelocity = player.position - player.oldPosition;

            float v = Projectile.velocity.Length();
            float a = playerVelocity.Y;
            float b = playerVelocity.X;
            float c = (player.Center.X - Projectile.Center.X);
            float d = (player.Center.Y - Projectile.Center.Y);

            float internalVal = -a * a * c * c + 2 * a * b * c * d - b * b * d * d + v * v * (c * c + d * d);

            float theta = internalVal >= 0 ?
                2 * (float)Math.Atan2(c * v - Math.Sqrt(internalVal),
                a * c - d * (b + v))
                :
                playerVelocity.ToRotation();

            if (theta > MathHelper.Pi)
            {
                theta -= MathHelper.TwoPi;
            }
            else if (theta < -MathHelper.Pi)
            {
                theta += MathHelper.TwoPi;
            }

            float angleDiff = theta - Projectile.velocity.ToRotation();
            for (int i = 0; i < 2; i++)
            {
                if (angleDiff > MathHelper.Pi)
                {
                    angleDiff -= MathHelper.TwoPi;
                }
                else if (angleDiff < -MathHelper.Pi)
                {
                    angleDiff += MathHelper.TwoPi;
                }
            }

            Projectile.velocity = Projectile.velocity.RotatedBy(Math.Min(MathHelper.TwoPi / Projectile.velocity.Length(), Math.Max(-MathHelper.TwoPi / Projectile.velocity.Length(), angleDiff)));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}