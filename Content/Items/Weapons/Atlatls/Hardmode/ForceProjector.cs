using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Items.Weapons.Ranged.Atlatls;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Atlatls.Hardmode
{
    public class ForceProjector : AtlatlBase
    {
        private Projectile shot;
        public override Vector2[] ShotDistances => new Vector2[] { new Vector2(47) };
        public override float BaseShotDistance => 47;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Force Projector");
            //Tooltip.SetDefault("Shoots projectiles encased in forcefields");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 20;
            Item.useAnimation = 21;
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.shoot = 10;
            Item.shootSpeed = 14.5f;
            Item.useAmmo = AmmoID.Dart;
        }

        public override bool RealShoot(Player player, EntitySource_ItemUse_WithAmmo source, int index, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Projectile dart = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile forcefield = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<ForceProjectile>(), damage, knockback, player.whoAmI, ai0: dart.whoAmI);
            SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, position);

            return false;
        }

        public override void AddRecipes() {
            //    ModRecipe recipe = new ModRecipe(mod);
            //    recipe.AddIngredient(ItemType<Items.Placeable.PolarizedBar>(),6);
            //    recipe.AddIngredient(ItemType<SmiteSoul>(),16);
            //    recipe.AddTile(TileID.MythrilAnvil);
            //    recipe.SetResult(this);
            //    recipe.AddRecipe();
            CreateRecipe()
                    .AddIngredient(ItemType<Placeable.Bars.PolarizedBar>(), 6)
                    .AddIngredient(ItemType<Materials.Hardmode.SmiteSoul>(), 16)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
        }
    }

    public class ForceProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_465";
        public Projectile shot;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forcefield");
            Main.projFrames[Projectile.type] = 4;
        }

        private int[] hitCooldowns = new int[Main.npc.Length];

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.width = 96;
            Projectile.height = 96;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            DrawOriginOffsetX = 0;

            Projectile.alpha = 128;
            Projectile.light = 0.75f;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                shot = Main.projectile[(int)Projectile.ai[0]];

                Projectile.localAI[0] = 1;
            }

            if (!shot.active)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.position = shot.Center - Projectile.Hitbox.Size() / 2;
                Projectile.velocity = Vector2.Zero;
                Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 1.5f)].noGravity = true;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
        }

        public override void DrawBehind(System.Int32 index, System.Collections.Generic.List<System.Int32> behindNPCsAndTiles, System.Collections.Generic.List<System.Int32> behindNPCs, System.Collections.Generic.List<System.Int32> behindProjectiles, System.Collections.Generic.List<System.Int32> overPlayers, System.Collections.Generic.List<System.Int32> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}