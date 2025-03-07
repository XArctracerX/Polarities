using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Placeable.Blocks;
using Polarities.Content.Items.Materials.PreHardmode;

namespace Polarities.Content.Items.Placeable.Furniture.Salt
{
    public class SaltDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SaltDoorClosed>(), 0);
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 150;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SaltBrick>(), 6)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}