using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Guns.Hardmode
{
	public class FractalBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Fractal Blaster");
			//Tooltip.SetDefault("Shoots a piercing laser that splits upon hitting enemies and bounces off tiles");
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 48;
			Item.height = 28;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<FractalBlasterShot>();
			Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).GetGlobalProjectile<Projectiles.PolaritiesProjectile>().cBow = 1;
			return false;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 12)
				.AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 3)
				.AddIngredient(ItemID.SpaceGun)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
		/*
		public bool Override()
		{
			return false;
		}

		public void Render(Player drawPlayer)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/FractalBlaster_Mask");

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			int num76 = 10;
			Vector2 vector12 = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
			Vector2 vector10 = (Vector2)HoldoutOffset();
			vector10.Y *= drawPlayer.gravDir;
			num76 = (int)vector10.X;
			vector12.Y += vector10.Y;
			Vector2 origin6 = new Vector2((float)(-num76), (float)(texture.Height / 2));
			if (drawPlayer.direction == -1)
			{
				origin6 = new Vector2((float)(texture.Width + num76), (float)(texture.Height / 2));
			}
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + vector12.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + vector12.Y)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, origin6, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			Main.playerDrawData.Add(drawData);
		}
		*/

        public override Vector2? HoldoutOffset()
        {
			return new Vector2(0, 0);
        }
    }

	public class FractalBlasterShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Blast");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;

			Projectile.width = 12;
			Projectile.height = 12;
			DrawOffsetX = -6;
			DrawOriginOffsetX = 3;

			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;

			Projectile.timeLeft = 600;
		}

		public override void AI()
		{
			if (Projectile.ai[0] > 0)
            {
				Projectile.ai[0]--;
            }

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.velocity = -oldVelocity;
			return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			for (int i=0; i<10; i++)
            {
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center-Projectile.velocity*(i/10f)*4 - Main.screenPosition, new Rectangle(0, 0, 18, 12), Color.White*(1-i/10f), Projectile.rotation, new Vector2(12, 6), Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}