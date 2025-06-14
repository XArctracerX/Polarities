using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Polarities.Projectiles;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Ranged.Guns.Hardmode
{
	public class ChaosShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			//DisplayName.SetDefault("Chaos Shot");
			//Tooltip.SetDefault("Shoots a chaotic spray of armor-piercing bullets"+"\n50% chance not to consume ammo");
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 72;
			Item.height = 46;
			Item.useTime = 1;
			Item.useAnimation = 1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver:40);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item11;
			//Item.autoReuse = true;
			Item.shoot = 10;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 16f;

			Item.channel = true;
		}

		private int channelTime;
		private float angleOffset;

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
				if (channelTime % Item.useTime == 0)
                {
					float multiplier = 2.3f+channelTime/(3/(3-2.3f)*3600f);
					angleOffset = multiplier * ((float)Math.Pow(angleOffset, 3) - angleOffset);

					player.direction = (Main.MouseWorld.X > player.position.X) ? 1 : -1;
					player.itemRotation = (Main.MouseWorld - player.Center).ToRotation() + angleOffset/8;
					if (player.direction == -1) player.itemRotation += (float)MathHelper.Pi;

					bool shot = false;
					for (int i=0; i<player.inventory.Length; i++)
                    {
						Item itemShot = player.inventory[(i+50) % player.inventory.Length];
						if (itemShot != null && !itemShot.IsAir && itemShot.ammo == AmmoID.Bullet)
						{
							SoundEngine.PlaySound(Item.UseSound, player.position);
							float fireRotation = player.itemRotation;
							if (player.direction == -1) fireRotation -= (float)MathHelper.Pi;

							Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(Item.width/2,0).RotatedBy((Main.MouseWorld - player.Center).ToRotation()), new Vector2(Item.shootSpeed, 0).RotatedBy(fireRotation), itemShot.shoot, Item.damage + itemShot.damage, Item.knockBack + itemShot.knockBack, player.whoAmI)].ArmorPenetration = 999;

							shot = true;
							break;
                        }
					}
					if (!shot || multiplier >= 3) player.channel = false;
				}
				if (Math.Abs(angleOffset) < 0.01f)
				{
					angleOffset = 0.01f * (angleOffset > 0 ? -1 : 1);
				}
				player.itemTime = Item.useTime;
				player.itemAnimation = Item.useAnimation;
				channelTime++;
			}
			else
            {
				channelTime = 0;
				angleOffset = 0;
            }
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
			return Main.rand.NextBool(2);
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Blocks.Fractal.DendriticEnergy>(), 40)
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 12)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return false;
        }

		public bool Override()
		{
			return false;
		}

		/*public void Render(Player drawPlayer)
		{
			Color currentColor = Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16.0));

			Texture2D texture = Request<Texture2D>("Polarities/Content/Items/Weapons/Ranged/Guns/HardmodeChaosShotAnimation").Value;

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			int textureFrames = 4;
			int frame = (PolaritiesWorld.timer / 5) % 4;


			int num76 = 10;
			Vector2 vector12 = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2 / textureFrames));
			Vector2 vector10 = (Vector2)HoldoutOffset();
			vector10.Y *= drawPlayer.gravDir;
			num76 = (int)vector10.X;
			vector12.Y += vector10.Y;
			Vector2 origin6 = new Vector2((float)(-num76), (float)(texture.Height / 2 / textureFrames));
			if (drawPlayer.direction == -1)
			{
				origin6 = new Vector2((float)(texture.Width + num76), (float)(texture.Height / 2 / textureFrames));
			}
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + vector12.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + vector12.Y)), (Rectangle?)new Rectangle(0, frame * texture.Height / textureFrames, texture.Width, texture.Height / textureFrames), drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(currentColor), drawPlayer.itemRotation, origin6, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			Main.playerDrawData.Add(drawData);


			texture = mod.GetTexture("Items/Weapons/ChaosShotAnimation_Mask");

			drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + vector12.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + vector12.Y)), (Rectangle?)new Rectangle(0, frame * texture.Height / textureFrames, texture.Width, texture.Height / textureFrames), Color.White, drawPlayer.itemRotation, origin6, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			Main.playerDrawData.Add(drawData);
		}*/

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 0);
		}
	}
}