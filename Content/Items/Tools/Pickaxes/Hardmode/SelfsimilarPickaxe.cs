using Polarities.Content.Items.Placeable.Bars;
using Polarities.Content.Items.Placeable.Furniture.Fractal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Pickaxes.Hardmode
{
	public class SelfsimilarPickaxe : ModItem
	{
        public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 5;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.pick = 220;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 16)
                .AddIngredient(ItemType<FractalPickaxe>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }

        public override void HoldItem(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().selfsimilarMining = true;
		}
    }
}