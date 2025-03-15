using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Hooks.PreHardmode
{
	internal class EndlessHook : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }
        
        public override void SetDefaults()
		{
            Item.noUseGraphic = true;
            Item.damage = 0;
            Item.knockBack = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileType<EndlessHookProjectile>();
            Item.width = 30;
            Item.height = 36;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 1);
		}
	}

	internal class EndlessHookProjectile : ModProjectile
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
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.aiStyle = 7;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
			Projectile.timeLeft *= 10;
		}

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;

            for (int l = 0; l < 1000; l++)
                if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type)
                    hooksOut++;

            return hooksOut <= 1;
        }

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange()
		{
			return 100000000f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 20f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 6;
		}

        public override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();

            center += distToProj.SafeNormalize(Vector2.Zero) * 3f;

            while (distance > 8f && !float.IsNaN(distance))
			{
				distToProj.Normalize();                 //get unit vector
				distToProj *= 8f;                      //speed = 24
				center += distToProj;                   //update draw position
				distToProj = playerCenter - center;    //update distance
				distance = distToProj.Length();
                Color drawColor = Lighting.GetColor(center.ToTileCoordinates());

                //Draw chain
                Main.EntitySpriteDraw(ChainTexture.Value, center - Main.screenPosition,
                    new Rectangle(0, 0, 2, 8), drawColor, projRotation,
					new Vector2(2 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
