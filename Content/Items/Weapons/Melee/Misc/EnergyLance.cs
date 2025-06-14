using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Polarities.Projectiles;
using Polarities.Core;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Melee.Misc
{
	public class EnergyLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Energy Lance");
			Item.staff[Item.type] = true;
		}

		/*public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "Tooltip1", "Attack to lunge towards the cursor"+"\nStriking enemies grants a substantial period of invulnerability"+"\nCannot lunge multiple times in the air");
			tooltips.Insert(Math.Min(5, tooltips.Count), line);
		}*/

		public override void SetDefaults()
		{
			Item.damage = 210;
			Item.DamageType = DamageClass.Melee;
			Item.width = 80;
			Item.height = 80;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = false;
			Item.noUseGraphic = false;
			Item.knockBack = 8f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.crit = 46;

			Item.shoot = ProjectileType<EnergyLanceHelper>();
			Item.shootSpeed = 1;
		}

		private bool itemRedirectable;
		bool lungable = true;

		public override void UpdateInventory(Player player)
        {
			if (ModUtils.IsOnGroundPrecise(player)) lungable = true;
        }

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			/*if (!target.immortal)
			{
				player.immune = true;
				player.immuneTime = 60;
			}
			player.itemTime = Math.Max(player.itemTime, 60);*/
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (lungable)
            {
				player.GetModPlayer<PolaritiesPlayer>().energyLanceLungeTime = 30;
				player.GetModPlayer<PolaritiesPlayer>().energyLanceLungeDir = velocity * 18;
				lungable = false;
            }
			return true;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 30)
				.AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 6)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			if (itemRedirectable)
            {
				itemRedirectable = false;
				player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;

				if (player.direction == 1)
				{
					player.itemRotation = (Main.MouseWorld - player.Center).ToRotation();
				}
				else
				{
					player.itemRotation = (Main.MouseWorld - player.Center).ToRotation() + MathHelper.Pi;
				}
			}

			noHitbox = true;
			double rotation = player.itemRotation;
			if (player.direction == -1)
			{
				rotation += MathHelper.Pi;
			}
			Vector2 unitVector = new Vector2(1,0).RotatedBy(player.itemRotation);
			if (player.direction == -1) { unitVector = new Vector2(-(float)Math.Cos(rotation - Math.PI), -(float)Math.Sin(rotation - Math.PI)); }
			else { unitVector = new Vector2(-(float)Math.Cos(-rotation - Math.PI), (float)Math.Sin(-rotation - Math.PI)); }
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC target = Main.npc[i];
				if (target.active)
				{
					float point = 0f;
					if (player.whoAmI == Main.myPlayer && Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), player.itemLocation, player.itemLocation + unitVector * Item.width * Item.scale * (float)Math.Sqrt(2), 7, ref point))
					{
						if (!target.dontTakeDamage && target.immune[player.whoAmI] <= 0)
						{
							if (noHitbox || (target.Center - player.itemLocation).Length() < ((new Vector2(hitbox.Center.X, hitbox.Center.Y)) - player.itemLocation).Length())
							{
								hitbox = target.Hitbox;
								noHitbox = false;
							}
						}
					}
				}
			}
		}

		/*
		public bool Override()
		{
			return false;
		}

		public void Render(Player drawPlayer)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/EnergyLance_Mask");

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			float num79 = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
			int num78 = 0;
			int num77 = 0;
			Vector2 origin5 = new Vector2(0f, texture.Height);
			if (drawPlayer.gravDir == -1f)
			{
				if (drawPlayer.direction == -1)
				{
					num79 += 1.57f;
					origin5 = new Vector2(texture.Width, 0f);
					num78 -= texture.Width;
				}
				else
				{
					num79 -= 1.57f;
					origin5 = Vector2.Zero;
				}
			}
			else if (drawPlayer.direction == -1)
			{
				origin5 = new Vector2(texture.Width, texture.Height);
				num78 -= texture.Width;
			}
			Vector2 holdoutOrigin = Vector2.Zero;
			ItemLoader.HoldoutOrigin(drawPlayer, ref holdoutOrigin);
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + origin5.X + (float)num78), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + (float)num77)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, num79, origin5 + holdoutOrigin, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			Main.playerDrawData.Add(drawData);
		}
		*/
	}

	public class EnergyLanceHelper : ModProjectile
    {
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

		public override void SetDefaults()
		{
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
		}
	}
}