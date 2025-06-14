using Microsoft.Xna.Framework;
using System;
using Polarities.Global;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Knives.Hardmode
{
	public class ScabStabber : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Scab Stabber");
			//Tooltip.SetDefault("Stacks up to 5");
		}

		public override void SetDefaults()
		{
			Item.damage = 78;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<ScabStabberProjectile>();
			Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float angle = Main.rand.NextFloat(MathHelper.TwoPi);
			for (int i = 0; i < 5; i++)
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, angle + MathHelper.TwoPi * i / 5, 0);
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<TwistedTendril>())
				.AddIngredient(ItemType<Materials.Hardmode.HemorrhagicFluid>(), 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class ScabStabberProjectile : ModProjectile
	{
		public override string Texture => "Polarities/Content/Items/Weapons/Melee/Knives/Hardmode/ScabStabber";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Scab Stabber");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;

			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = true;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		private Vector2 startPosition;
		private Vector2 startVelocity;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player.dead || !player.active)
			{
				Projectile.active = false;
			}

			if (Projectile.timeLeft == 30)
			{
				startPosition = Projectile.Center;
				startVelocity = Projectile.velocity;
			}

			Vector2 goalPosition = startPosition + startVelocity.SafeNormalize(Vector2.Zero) * Projectile.ai[1] * Projectile.ai[1] * Projectile.ai[1] / 40f + startVelocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * 64 * (float)Math.Sin(Projectile.ai[0] + Projectile.ai[1] / 10f) * (float)Math.Sin(MathHelper.Pi * Projectile.ai[1] / 30f);

			Projectile.velocity = goalPosition - Projectile.Center;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

			Projectile.ai[1]++;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 2;
			height = 2;
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(16), 0).RotatedByRandom(MathHelper.TwoPi);
				Dust dust = Dust.NewDustPerfect(dustPos, DustID.Blood, Velocity: (dustPos - Projectile.Center) / 6, Scale: 1.4f);
				dust.noGravity = true;
			}
		}
	}
}