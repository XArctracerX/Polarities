using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Placeable.Blocks;
using Polarities.Content.Items.Materials.PreHardmode;

namespace Polarities.Content.Items.Placeable.Furniture.Limestone
{
    public class LimestoneDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<LimestoneDoorClosed>(), 0);
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LimestoneBrick>(), 6)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}