using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace Polarities.Content.Items.Weapons.Ranged.Bows.Hardmode
{
	public class Sunsliver : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Sunsliver");
			// Tooltip.SetDefault("Fires streams of arrows at varying speeds");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 56;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 80000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 0f;
			Item.useAmmo = AmmoID.Arrow;
		}

        public override Vector2? HoldoutOffset()
        {
			return new Vector2(-4, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity * 2, type, damage, knockBack, player.whoAmI).noDropItem = true;
			Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity * (float)Math.Pow(2, 2f / 3f), type, damage, knockBack, player.whoAmI).noDropItem = true;
			Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity * (float)Math.Pow(2, 1f / 3f), type, damage, knockBack, player.whoAmI).noDropItem = true;
			Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, type, damage, knockBack, player.whoAmI).noDropItem = true;

			/*Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX * 2, speedY * 2, type, damage, knockBack, player.whoAmI, 0, 0)].noDropItem = true;
			Main.projectile[Projectile.NewProjectile(position.X,position.Y,speedX*(float)Math.Pow(2,2f/3f),speedY*(float)Math.Pow(2,2f/3f),type,(int)(damage*Math.Pow(2,1f/3f)),knockBack,player.whoAmI,0,0)].noDropItem = true;
			Main.projectile[Projectile.NewProjectile(position.X,position.Y,speedX*(float)Math.Pow(2,1f/3f),speedY*(float)Math.Pow(2,1f/3f),type,(int)(damage*Math.Pow(2,2f/3f)),knockBack,player.whoAmI,0,0)].noDropItem = true;
			Main.projectile[Projectile.NewProjectile(position.X,position.Y,speedX,speedY,type,damage*2,knockBack,player.whoAmI,0,0)].noDropItem = true;*/
		}
	}
}