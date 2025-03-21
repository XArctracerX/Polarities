using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Polarities.Content.Items.Weapons.Melee.Warhammers;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.Hardmode.Metal
{
    public class PythagoreanHammer : WarhammerBase
    {
        public override int HammerLength => 56;
        public override int HammerHeadSize => 26;
        public override int DefenseLoss => 24;
        public override int DebuffTime => 1200;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(100, 12f, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 82;
            Item.height = 82;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Pink;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<PythagoreanHammerProjectile>()] < 1;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                SoundEngine.PlaySound(SoundID.Item1, player.position);
                Item.shoot = ProjectileType<PythagoreanHammerProjectile>();
                Item.shootSpeed = 24f;
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = WarhammerUseStyle;
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0f;
                Item.noMelee = false;
                Item.noUseGraphic = false;
            }
            return null;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage /= 2;
            knockback /= 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.altFunctionUse == 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 16)
                .AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 3)
                .AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
                .Register();
        }
    }
    public class PythagoreanHammerProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Melee/Warhammers/Hardmode/Metal/PythagoreanHammer";
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.alpha = 0;
            Projectile.timeLeft = 3000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        Vector2 targetPos;
        float deltaR = 0;
        int dir = 1;
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                targetPos = Main.MouseWorld;
                Projectile.localAI[0] = 1;
                dir = Main.player[Projectile.owner].direction;
            }


            if (Projectile.localAI[0] == 1)
            {
                if (Vector2.Dot(Projectile.velocity, Projectile.DirectionTo(targetPos)) < 0) Projectile.localAI[0] = 2;
                if (Projectile.position.Distance(targetPos) < 5) Projectile.localAI[0] = 2;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetPos) * 20f, 0.1f);

                deltaR = MathHelper.Lerp(deltaR, 40 * dir, 0.1f);
            }

            if (Projectile.localAI[0] == 2)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.position) * 14f, 0.1f);
                if (Projectile.position.Distance(player.position) < 30) Projectile.Kill();
                deltaR = MathHelper.Lerp(deltaR, -30 * dir, 0.1f);
            }

            Projectile.rotation += MathHelper.ToRadians(deltaR);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = 2;
            Projectile.velocity = Projectile.DirectionTo(Main.player[Projectile.owner].position) * 14f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.localAI[0] = 2;
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.velocity = Projectile.DirectionTo(Main.player[Projectile.owner].position) * 14f;
            return false;
        }
    }
}