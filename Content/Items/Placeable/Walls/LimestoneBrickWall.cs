using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Walls
{
    public class LimestoneBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (400);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall((ushort)WallType<LimestoneBrickWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemType<LimestoneBrick>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class LimestoneBrickWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            
            AddMapEntry(new Color(34, 50, 30));

            DustType = DustType<LimestoneDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}