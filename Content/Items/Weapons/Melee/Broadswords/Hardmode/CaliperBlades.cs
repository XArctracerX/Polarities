using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Projectiles;
using System;

namespace Polarities.Content.Items.Weapons.Melee.Broadswords.Hardmode
{
	public class CaliperBlades : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Caliper Blades");
			//Tooltip.SetDefault("Summons blades to dash at the cursor upon striking enemies"+"\n'Measure once, cut twice'");
		}

		public override void SetDefaults()
		{
			Item.damage = 240;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (!target.immortal)
			{
				if (player.ownedProjectileCounts[ProjectileType<CaliperBlade1>()] < 4)
				{
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<CaliperBlade1>(), Item.damage, Item.knockBack, player.whoAmI);
				}
				if (player.ownedProjectileCounts[ProjectileType<CaliperBlade2>()] < 4)
				{
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<CaliperBlade2>(), Item.damage, Item.knockBack, player.whoAmI);
				}
			}
		}

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurt)
		{
			if (player.ownedProjectileCounts[ProjectileType<CaliperBlade1>()] < 4)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<CaliperBlade1>(), Item.damage, Item.knockBack, player.whoAmI);
			}
			if (player.ownedProjectileCounts[ProjectileType<CaliperBlade2>()] < 4)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<CaliperBlade2>(), Item.damage, Item.knockBack, player.whoAmI);
			}
		}
    }

    public class CaliperBlade1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Caliper Blade");
		}

		public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
			DrawOffsetX = -10;
			DrawOriginOffsetX = 5;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
            Projectile.timeLeft = 600;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
			if (Projectile.timeLeft % 60 > 45)
            {
				Projectile.velocity = Vector2.Zero;
				if (Projectile.timeLeft % 60 == 59)
                {
					Projectile.ai[0] = 0;
                }
				Projectile.ai[0] += 0.1f;
				Projectile.rotation += Projectile.ai[0];

				Projectile.friendly = false;
			}
			else
            {
				if (Projectile.timeLeft % 60 == 45)
				{
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;
					Projectile.netUpdate = true;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() - 0.436f;
				Projectile.velocity *= 0.98f;

				Projectile.friendly = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override bool? CanCutTiles()
        {
            return false;
		}
	}

	public class CaliperBlade2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Caliper Blade");
		}
		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			DrawOffsetX = -10;
			DrawOriginOffsetX = 5;

			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.timeLeft = 600;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.timeLeft % 100 > 75)
			{
				Projectile.velocity = Vector2.Zero;
				if (Projectile.timeLeft % 100 == 99)
				{
					Projectile.ai[0] = 0;
				}
				Projectile.ai[0] += 0.1f;
				Projectile.rotation += Projectile.ai[0];

				Projectile.friendly = false;
			}
			else
			{
				if (Projectile.timeLeft % 100 == 75)
				{
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;
					Projectile.netUpdate = true;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + 0.436f;
				Projectile.velocity *= 0.99f;

				Projectile.friendly = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}