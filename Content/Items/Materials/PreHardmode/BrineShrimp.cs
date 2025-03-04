using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Materials.PreHardmode
{
    public class BrineShrimp : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (3);
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.value = 500;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CookedShrimp)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}

