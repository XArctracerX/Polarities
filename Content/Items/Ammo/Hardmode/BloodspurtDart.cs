using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace Polarities.Content.Items.Ammo.Hardmode
{
	public class BloodspurtDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//Tooltip.SetDefault("Pushes itself along by spewing a damaging trail of blood");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 24;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(copper: 2);
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ProjectileType<BloodspurtDartProjectile>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 0f;                  //The speed of the projectile
			Item.ammo = AmmoID.Dart;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(100)
				.AddIngredient(ItemType<Materials.Hardmode.HemorrhagicFluid>())
				.AddIngredient(ItemType<Ammo.PreHardmode.WoodenDart>(), 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class BloodspurtDartProjectile : ModProjectile
	{
		public override string Texture => "Polarities/Content/Items/Ammo/Hardmode/BloodspurtDart";

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.alpha = 0;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
		}

		public override void AI()
		{
			Projectile.ai[0]++;

			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 0.3f);

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity.Y += 0.3f;

			if (Projectile.velocity.Length () > 0)
			{
				Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(8), 0).RotatedByRandom(MathHelper.TwoPi);
				Dust dust = Dust.NewDustPerfect(dustPos, DustID.Blood, Velocity: Projectile.velocity * ((Projectile.velocity.Length() - 10) / Projectile.velocity.Length()), Scale: 1.5f);

				if (Projectile.ai[0] % 5 == 0 && Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * ((Projectile.velocity.Length() - 10) / Projectile.velocity.Length()), ProjectileType<BloodSpurt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
	}

	public class BloodSpurt : ModProjectile
	{
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.alpha = 0;
			Projectile.timeLeft = 15;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;

			Projectile.hide = true;
		}

		public override void AI()
		{

		}
	}
}
