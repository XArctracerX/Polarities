using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Global;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Flawless
{
    public class ColdShoulder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.noUseGraphic = true;
            Item.damage = 0;
            Item.knockBack = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 13f;
            Item.shoot = ProjectileType<ColdShoulderProjectile>();
            Item.width = 30;
            Item.height = 26;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.value = 5000;
        }
    }

    public class ColdShoulderProjectile : ModProjectile
    {
        //texture cacheing
        public static Asset<Texture2D> ChainTexture;

        public override void Load()
        {
            ChainTexture = Request<Texture2D>(Texture + "_Chain");
        }

        public override void Unload()
        {
            ChainTexture = null;

        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.aiStyle = 7;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 10;
        }

        public override void AI()
        {
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active) continue;
                if (npc.Distance(Projectile.position) > 160) continue;

                float freezeScale = (160 - npc.Distance(Projectile.position)) / 160f;
                npc.position -= npc.velocity * freezeScale * npc.knockBackResist;
            }
        }

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange()
        {
            return 500f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 2;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 18f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 20;
        }

        public override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 16f && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= 16f;
                center += distToProj;
                distToProj = playerCenter - center;
                distance = distToProj.Length();

                //Draw chain
                Main.spriteBatch.Draw(ChainTexture.Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 10, 16), Color.White, projRotation,
                    new Vector2(10 * 0.5f, 16 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
