using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Walls
{
    public class SaltBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (400);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall((ushort)WallType<SaltBrickWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemType<SaltBrick>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class SaltBrickWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
           
            AddMapEntry(new Color(204, 165, 205));

            DustType = DustType<SaltDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}