using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Multi.Hardmode
{
	public class PolarHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = (1);
		}
		public override void SetDefaults() {
			Item.damage = 60;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 50;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
            Item.useTurn = true;
			Item.axe = 30;
            Item.hammer = 100;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(gold: 7);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.tileBoost = 2;
		}

		public override void UpdateInventory(Player player)
        {
			if (player.HeldItem == Item)
            {
				foreach (Item i in Main.item)
				{
					if (i.DistanceSQ(player.position) <= 625 * 64) Dust.NewDustDirect(i.position, i.width, i.height, DustID.Electric).noGravity = true;
					float pullStrength = (10000f / i.DistanceSQ(player.position));
					if (pullStrength > 100f) pullStrength = 100f;
					i.velocity += i.DirectionTo(player.position) * pullStrength;
				}
			}
        }

		public override void AddRecipes()
		{
			//    ModRecipe recipe = new ModRecipe(mod);
			//    recipe.AddIngredient(ItemType<Items.Placeable.PolarizedBar>(),6);
			//    recipe.AddIngredient(ItemType<SmiteSoul>(),16);
			//    recipe.AddTile(TileID.MythrilAnvil);
			//    recipe.SetResult(this);
			//    recipe.AddRecipe();
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Bars.PolarizedBar>(), 15)
				.AddIngredient(ItemType<Materials.Hardmode.SmiteSoul>(), 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}