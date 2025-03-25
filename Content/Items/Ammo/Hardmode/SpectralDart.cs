using Microsoft.Xna.Framework;
//using MultiHitboxNPCLibrary;
//using Polarities.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Ammo.Hardmode
{
    public class SpectralDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Pierces through enemies and walls until its damage is depleted");

            Item.ResearchUnlockCount = (99);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(20, 0, 0);
            Item.DamageType = DamageClass.Ranged;

            Item.width = 7;
            Item.height = 14;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.value = 20;
            Item.rare = ItemRarityID.Lime;

            Item.shoot = ProjectileType<SpectralDartProjectile>();
            Item.shootSpeed = 3.5f;
            Item.ammo = AmmoID.Dart;
        }

        public override void AddRecipes()
        {
            CreateRecipe(70)
                .AddIngredient(ItemID.Ectoplasm)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class SpectralDartProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Ammo/Hardmode/SpectralDart";

        float pierceScore;
        int timeSpentInWall;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.alpha = 0;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            //Projectile.GetGlobalProjectile<MultiHitboxNPCLibraryProjectile>().badCollision = true;
            //Projectile.GetGlobalProjectile<MultiHitboxNPCLibraryProjectile>().javelinSticking = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            while (Projectile.velocity.X >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y >= 16f || Projectile.velocity.Y < -16f)
            {
                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y *= 0.99f;
            }
            pierceScore = Projectile.damage;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            pierceScore -= 0.5f;
            if (timeSpentInWall % 10 == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Roar_1"), Projectile.position);
            }
            timeSpentInWall++;

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage -= damageDone / 2;
            pierceScore -= damageDone / 2;
        }
        public override void AI()
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 180).noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.075f;

            if (Projectile.velocity.Y > 16) Projectile.velocity.Y = 16;

            if (pierceScore <= 0) Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }
}