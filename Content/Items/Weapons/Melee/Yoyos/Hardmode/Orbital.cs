using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace Polarities.Content.Items.Weapons.Melee.Yoyos.Hardmode
{
	public class Orbital : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
            //DisplayName.SetDefault("Orbital");
			//Tooltip.SetDefault("Creates an electron cloud");

			// These are all related to gamepad controls and don't seem to affect anything else
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.useStyle = 5;
			Item.width = 30;
			Item.height = 24;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 16f;
			Item.knockBack = 2.5f;
			Item.damage = 130;
			Item.rare = ItemRarityID.Red;

			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(gold:10);
			Item.shoot = ProjectileType<OrbitalProjectile>();
		}

        public override void AddRecipes() {
			//         ModRecipe recipe = new ModRecipe(mod);
			//         recipe.AddIngredient(ItemType<Placeable.PolarizedBar>(),6);
			//         recipe.AddIngredient(ItemType<SmiteSoul>(),16);
			//		   recipe.AddTile(TileID.MythrilAnvil);
			//         recipe.SetResult(this);
			//         recipe.AddRecipe();
			CreateRecipe()
						 .AddIngredient(ItemType<Placeable.Bars.PolarizedBar>(), 6)
						 .AddIngredient(ItemType<Materials.Hardmode.SmiteSoul>(), 16)
						 .AddTile(TileID.MythrilAnvil)
						 .Register();
		}
	}

	public class OrbitalProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Orbital");

			// The following sets are only applicable to yoyo that use aiStyle 99.
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
			// YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
			// Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 390f;
			// YoyosTopSpeed is top speed of the yoyo Projectile. 
			// Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 17f;
		}

		public override void SetDefaults()
		{
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			// aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		// notes for aiStyle 99: 
		// localAI[0] is used for timing up to YoyosLifeTimeMultiplier
		// localAI[1] can be used freely by specific types
		// ai[0] and ai[1] usually point towards the x and y world coordinate hover point
		// ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
		// ai[0] being negative makes the yoyo move back towards the player
		// Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.

		public override void PostAI()
		{
			if (Projectile.localAI[1] == 0)
			{
				Projectile.localAI[1] = 1;
				for (int i = 0; i < 4; i++)
				{
					Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ProjectileType<OrbitalOrbital>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI)].rotation = i * MathHelper.PiOver2;
				}
			}

			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
				dust.noGravity = true;
				dust.scale = 0.75f;
			}
		}
	}

	public class OrbitalOrbital : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Orbital");
		}

		public override void SetDefaults()
		{
			Projectile.extraUpdates = 0;
			Projectile.alpha = 128;
			Projectile.width = 40;
			Projectile.height = 40;
			DrawOriginOffsetX = -9;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (!Main.projectile[(int)Projectile.ai[0]].active)
			{
				Projectile.active = false;
				return;
			}
			else
			{
				Projectile.timeLeft = 2;
			}

			Projectile.rotation -= 0.2f;
			Projectile.position = Main.projectile[(int)Projectile.ai[0]].Center - new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f) + (new Vector2(-40, 0)).RotatedBy(Projectile.rotation) - Main.projectile[(int)Projectile.ai[0]].velocity;
			Projectile.velocity = Main.projectile[(int)Projectile.ai[0]].velocity;
		}
	}
}
