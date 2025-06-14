using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Broadswords.Hardmode
{
	public class FractalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			// DisplayName.SetDefault("Fractal Sword");
			// Tooltip.SetDefault("Inflicts chaos state on striking enemies"+"\nDeals double damage to enemies with chaos state");
		}

		public override void SetDefaults()
		{
			Item.damage = 98;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.height = 32;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<FractalSwordShot>();
			Item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			Vector2 position = target.position + Main.rand.NextVector2CircularEdge(200, 200);
			Vector2 velocity = position.DirectionTo(target.position) * Item.shootSpeed;
			Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
			p.ai[0] = player.direction;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 3)
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 15)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}

	public class SelfsimilarSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			// DisplayName.SetDefault("Selfsimilar Sword");
			// Tooltip.SetDefault("Inflicts chaos state on melee hits" + "\nDeals double damage to enemies with chaos state");
		}

		public override void SetDefaults()
		{
			Item.damage = 210;
			Item.DamageType = DamageClass.Melee;
			Item.width = 38;
			Item.height = 57;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.shoot = ProjectileType<SelfsimilarSwordShot>();
			Item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			Vector2 position = target.position + Main.rand.NextVector2CircularEdge(400, 400);
			Vector2 velocity = position.DirectionTo(target.position) * Item.shootSpeed;
			Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
			p.ai[0] = player.direction;
			p.ai[1] = target.whoAmI;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<FractalSword>())
				.AddIngredient(ItemType<Placeable.Bars.SelfsimilarBar>(), 10)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}

	public class FractalSwordShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Selfsimilar Sword");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 25;
			Projectile.height = 50;
			DrawOffsetX = -44;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 22;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
			Projectile.alpha = 255;

			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
		}

		float timer = 0;
		private const int FADE_TICKS = 8;

		public override void AI()
		{
			Projectile.alpha -= 255 / FADE_TICKS;
			if (Projectile.alpha < 0) Projectile.alpha = 0;

			Vector2 dustPos = Projectile.Top + 3 * (Projectile.velocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero));
			dustPos = dustPos.RotatedBy(timer, Projectile.Center);
			Dust.NewDustPerfect(dustPos, DustID.Electric, Velocity: Vector2.Zero, newColor: Color.LightBlue, Scale: 1f).noGravity = true;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + timer;
			timer += (float)Math.PI / 15f * Projectile.ai[0];
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor = Color.White;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				color.MultiplyRGBA(new Color(1f, 1f, 1f, (255f - Projectile.alpha) / 255));
				float scale = Projectile.scale;

				float rotation = Projectile.rotation;

				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class SelfsimilarSwordShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Selfsimilar Sword");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 9;
			Projectile.height = 24;
			DrawOffsetX = -44;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 22;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
			Projectile.alpha = 255;

			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
		}

		float timer = 0;
		private const int FADE_TICKS = 16;

		public override void AI()
		{
			Projectile.alpha -= 255 / FADE_TICKS;
			if (Projectile.alpha < 0) Projectile.alpha = 0;

			Vector2 dustPos = Projectile.Top + 3 * (Projectile.velocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero));
			dustPos = dustPos.RotatedBy(timer, Projectile.Center);
			Dust.NewDustPerfect(dustPos, DustID.Electric, Velocity: Vector2.Zero, newColor: Color.LightBlue, Scale: 1f).noGravity = true;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + timer;
			timer += (float)Math.PI / 15f * Projectile.ai[0];
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.ai[1] != target.whoAmI && Projectile.ai[1] != -1)
			{
				Vector2 position = target.position + Main.rand.NextVector2CircularEdge(800, 800);
				Vector2 velocity = position.DirectionTo(target.position) * Projectile.velocity.Length();
				Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				p.ai[0] = Projectile.ai[0];
				if (p.ai[2] == 1) p.ai[1] = target.whoAmI;
				else p.ai[1] = -1;
				p.ai[2] = 1;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor = Color.White;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				color.MultiplyRGBA(new Color(1f, 1f, 1f, (255f - Projectile.alpha) / 255));
				float scale = Projectile.scale;

				float rotation = Projectile.rotation;

				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + Projectile.oldPos[k] - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}