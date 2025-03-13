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
	public class FractalScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Scythe");
			// Tooltip.SetDefault("Shoots a wave of fractal energy that splits on hitting enemies");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 90;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 30;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.shootSpeed = 6f;
			Item.shoot = ProjectileType<FractalScytheSlash>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Content.Items.Materials.Hardmode.FractalResidue>(), 4)
				.AddIngredient(ItemType<Content.Items.Placeable.Bars.FractalBar>(), 13)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}

	public class FractalScytheSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Slash");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 0.75f;

			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.light = 1f;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;

			Projectile.timeLeft = 80;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft <= Projectile.oldPos.Length)
            {
				Projectile.alpha = (int)(255 - 255 * Projectile.timeLeft / (float)Projectile.oldPos.Length);
            }
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			Vector2 start = Projectile.Center + new Vector2(0, 48 * Projectile.scale).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center - new Vector2(0, 48 * Projectile.scale).RotatedBy(Projectile.rotation);
			return Core.ModUtils.CheckAABBvSegment(targetHitbox, start, end);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Split();
        }

		private void Split()
		{
			if (Projectile.scale > 0.5f)
			{
				int shot = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, 24 * Projectile.scale).RotatedBy(Projectile.rotation), Projectile.velocity.RotatedBy(0.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[shot].scale = Projectile.scale / 2;
				Main.projectile[shot].timeLeft = Projectile.timeLeft - 1;
				shot = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 24 * Projectile.scale).RotatedBy(Projectile.rotation), Projectile.velocity.RotatedBy(-0.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[shot].scale = Projectile.scale / 2;
				Main.projectile[shot].timeLeft = Projectile.timeLeft - 1;
			}
			if (Projectile.timeLeft > Projectile.oldPos.Length)
			{
				Projectile.timeLeft = Projectile.oldPos.Length;
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = frame.Center();

			Color mainColor = new Color(128, 255, 255);

			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Projectile.position + Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(mainColor) * ((1 - i / (float)Projectile.oldPos.Length) * 0.75f), Projectile.rotation, origin, Projectile.scale * (1.5f - i / (float)Projectile.oldPos.Length) * 0.6f, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(mainColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class SelfsimilarSlasher : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Selfsimilar Slasher");
			//Tooltip.SetDefault("Shoots a splitting wave of selfsimilar energy");

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 30;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.shootSpeed = 6f;
			Item.shoot = ProjectileType<SelfsimilarScytheSlash>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<FractalScythe>())
				.AddIngredient(ItemType<Placeable.Bars.SelfsimilarBar>(), 10)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();

			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<Items.Placeable.SelfsimilarBar>(), 10);
			recipe.AddIngredient(ItemType<FractalScythe>());
			recipe.AddTile(TileType<Tiles.Furniture.FractalAssemblerTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
			 */
		}
	}

	public class SelfsimilarScytheSlash : ModProjectile
	{
        public override string Texture => "Polarities/Content/Items/Weapons/Melee/Broadswords/Hardmode/FractalScytheSlash";

        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Selfsimilar Slash");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1f;

			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.light = 1f;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;

			Projectile.timeLeft = 100;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft <= Projectile.oldPos.Length)
			{
				Projectile.alpha = (int)(255 - 255 * Projectile.timeLeft / (float)Projectile.oldPos.Length);
			}
			else if (Projectile.timeLeft == (int)(Projectile.oldPos.Length + Projectile.scale * 40))
            {
				Split();
            }
		}

		private void Split()
		{
			if (Projectile.scale > 0.1f)
			{
				int shot = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, 24 * Projectile.scale).RotatedBy(Projectile.rotation), Projectile.velocity.RotatedBy(0.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[shot].scale = Projectile.scale / 2;
				Main.projectile[shot].timeLeft = Projectile.timeLeft - 1;
				shot = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 24 * Projectile.scale).RotatedBy(Projectile.rotation), Projectile.velocity.RotatedBy(-0.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[shot].scale = Projectile.scale / 2;
				Main.projectile[shot].timeLeft = Projectile.timeLeft - 1;
			}
			if (Projectile.timeLeft > Projectile.oldPos.Length)
			{
				Projectile.timeLeft = Projectile.oldPos.Length;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Projectile.Center + new Vector2(0, 48 * Projectile.scale).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center - new Vector2(0, 48 * Projectile.scale).RotatedBy(Projectile.rotation);
			return Core.ModUtils.CheckAABBvSegment(targetHitbox, start, end);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Split();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = frame.Center();

			Color mainColor = new Color(247, 173, 255);
			Color secondaryColor = new Color(174, 116, 220);

			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Color useColor = Color.Lerp(mainColor, secondaryColor, (float)Math.Sin(Projectile.timeLeft - i) * 0.5f + 0.5f);
				Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Projectile.position + Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(useColor) * ((1 - i / (float)Projectile.oldPos.Length) * 0.75f), Projectile.rotation, origin, Projectile.scale * (1.5f - i / (float)Projectile.oldPos.Length) * 0.6f, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(mainColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}