using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Walls
{
    public class LimestoneWall : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall((ushort)WallType<LimestoneWallPlaced>());

            Item.ResearchUnlockCount = (400);
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemType<Limestone>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class LimestoneWallPlaced : ModWall
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

    public class LimestoneWallNatural : ModWall
    {
        public override string Texture => "Polarities/Content/Items/Placeable/Walls/LimestoneWallPlaced";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(34, 50, 30));

            DustType = DustType<LimestoneDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}