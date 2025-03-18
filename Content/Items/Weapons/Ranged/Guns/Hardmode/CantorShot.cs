using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Ranged.Guns.Hardmode
{
	public class CantorShot : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Cantor Shot");
			//Tooltip.SetDefault("Shoots a fractal spray of bullets");
		}

		public override void SetDefaults() {
			Item.damage = 13;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 78;
			Item.height = 26;
			Item.useTime = 40;
			Item.useAnimation = 41;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            int[] shots = {0,2,6,8,18,20,24,26};
            foreach (int i in shots) {
                Projectile.NewProjectile(source, position, velocity*((i+27)/54f), type, damage, knockback, player.whoAmI, 0, 0);
            }
			return false;
		}

		public bool Override()
		{
			return false;
		}

		public void Render(Player drawPlayer)
		{
			Texture2D texture = ModContent.Request<Texture2D>("Items/Weapons/CantorShot_Mask").Value;

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));


			int num76 = 10;
			Vector2 vector12 = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
			Vector2 vector10 = (Vector2)HoldoutOffset();
			vector10.Y *= drawPlayer.gravDir;
			num76 = (int)vector10.X;
			vector12.Y += vector10.Y;
			Vector2 origin6 = new Vector2((float)(-num76), (float)(texture.Height / 2));
			if (drawPlayer.direction == -1)
			{
				origin6 = new Vector2((float)(texture.Width + num76), (float)(texture.Height / 2));
			}
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + vector12.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + vector12.Y)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, origin6, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			//Main.playerDrawData.Add(drawData);
		}

		public override Vector2? HoldoutOffset() {
			return Vector2.Zero;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemType<Content.Items.Materials.Hardmode.FractalResidue>(), 3)
				.AddIngredient(ItemType<Content.Items.Placeable.Bars.FractalBar>(), 14)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}
	}
}