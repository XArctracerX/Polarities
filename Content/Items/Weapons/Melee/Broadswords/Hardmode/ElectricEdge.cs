using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Melee.Broadswords.Hardmode
{
	public class ElectricEdge : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Electric Edge");
			//Tooltip.SetDefault("Rain lightning on your foes");
			Item.ResearchUnlockCount = 1;
			Item.staff[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 290;
			Item.DamageType = DamageClass.Melee;
			Item.width = 80;
			Item.height = 80;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 1;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<LightningSwordHead>();
			Item.shootSpeed = 24;
		}

		public override void MeleeEffects (Player player, Rectangle hitbox) {
			Lighting.AddLight(player.Center, Color.White.ToVector3() * Main.essScale);
		}

        public override System.Boolean Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, System.Int32 type, System.Int32 damage, System.Single knockback)
        {
			float initX = Main.MouseWorld.X + (float)Main.rand.Next(-300, 300);
			float initY = Main.MouseWorld.Y - 1000;

			velocity = new Vector2(24 * (Main.MouseWorld.X - initX) / (float)Math.Sqrt((Main.MouseWorld.X - initX) * (Main.MouseWorld.X - initX) + (Main.MouseWorld.Y - initY) * (Main.MouseWorld.Y - initY)), 24 * (Main.MouseWorld.Y - initY) / (float)Math.Sqrt((Main.MouseWorld.X - initX) * (Main.MouseWorld.X - initX) + (Main.MouseWorld.Y - initY) * (Main.MouseWorld.Y - initY)));

			Projectile.NewProjectile(source, new Vector2(initX, initY), velocity, type, damage, knockback, player.whoAmI, initX, initY);
			return false;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
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

	public class LightningSwordHead : ModProjectile
	{
		private int timer;

		private float lightningPointX
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float lightningPointY
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lightning Strike");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.alpha = 0;
			Projectile.timeLeft = 240;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{

			timer = (timer + 1) % 2;

			if (timer == 0)
			{
				//need to find angles such that the point of distance c from lightningPoint is within distance r of position: Cos(2theta)=(c^2+d^2-r^2)/2cd

				float segmentLength = 56;
				float variance = 64;
				float dist = (float)Math.Sqrt((lightningPointX - Projectile.position.X) * (lightningPointX - Projectile.position.X) + (lightningPointY - Projectile.position.Y) * (lightningPointY - Projectile.position.Y));

				float theta = 0;

				if (dist > variance + segmentLength || segmentLength > variance + dist)
				{
					theta = 0;
				}
				else if (variance > dist + segmentLength)
				{
					theta = (float)Math.PI;
				}
				else
				{
					theta = (float)Math.Acos((segmentLength * segmentLength + dist * dist - variance * variance) / (2 * segmentLength * dist));
				}

				float dirToProjectile = (float)Math.Atan((Projectile.position.Y - lightningPointY) / (Projectile.position.X - lightningPointX));

				if (Projectile.position.X - lightningPointX < 0)
				{
					dirToProjectile += (float)Math.PI;
				}

				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;

					float angle = dirToProjectile + 2 * theta * (float)Main.rand.NextDouble() - theta;

					int segment = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(lightningPointX, lightningPointY), Vector2.Zero, ProjectileType<LightningSwordArc>(), Projectile.damage, Projectile.knockBack, Projectile.owner, angle, Projectile.whoAmI);
					Main.projectile[segment].DamageType = DamageClass.Melee;
					Main.projectile[segment].minion = Projectile.minion;
					Main.projectile[segment].rotation = angle;

					lightningPointX += (float)Math.Cos(angle) * segmentLength;
					lightningPointY += (float)Math.Sin(angle) * segmentLength;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.timeLeft > 4)
			{
				Projectile.timeLeft = 4;
				SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
				for (int i = 0; i < 128; i++)
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position - new Vector2(32, 32), 64, 64, 226, 0f, 0f, 0, new Color(63, 63, 255), 2f)];
					dust.noGravity = true;
					dust.scale = 1.2f;
				}
			}
			return false;
		}
	}

	public class LightningSwordArc : ModProjectile
	{
		private float angle
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private int owner
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		private int length;

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 14;
			Projectile.height = 14;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = -25;
			Projectile.alpha = 0;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.light = 1f;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0)
			{
				Projectile.localAI[0] = 1;

				for (length = 0; length < 64; length += 8)
				{
					if (!Collision.CanHit(Projectile.Center, 1, 1, Projectile.Center + (new Vector2(length - Projectile.width / 2, 0)).RotatedBy(Projectile.rotation), 1, 1))
					{
						Main.projectile[owner].Kill();
						SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
						break;
					}
				}
			}
			if (length < 64)
			{
				int dust = Dust.NewDust(Projectile.Center + (new Vector2(length - Projectile.width / 2, 0)).RotatedBy(Projectile.rotation), 0, 0, DustID.Electric, 0f, 0f, 0, new Color(63, 63, 255), 1.2f);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			return Collision.CheckAABBvLineCollision(new Vector2(targetHitbox.X, targetHitbox.Y), targetHitbox.Size(), Projectile.Center, Projectile.Center + (new Vector2(length, 0)).RotatedBy(Projectile.rotation), 14, ref point);
		}

		public override bool PreDraw(ref Color lightColor)
		{

			Vector2 DrawOrigin = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 drawPos = Projectile.position - Main.screenPosition + DrawOrigin;
			Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, drawPos, new Rectangle(0, 0, length, 14), Color.White, Projectile.rotation, DrawOrigin, Projectile.scale, SpriteEffects.None, 0f);

			return false;
		}
	}
}