using Polarities.Content.Items.Placeable.Bars;
using Polarities.Content.Items.Placeable.Furniture.Fractal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Pickaxes.Hardmode
{
	public class FractalPickaxe : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 8;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.pick = 190;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold:2,silver:25);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.tileBoost = 1;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 16)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
		}
	}
}