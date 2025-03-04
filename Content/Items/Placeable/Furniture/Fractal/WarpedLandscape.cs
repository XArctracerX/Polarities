using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture;
using Polarities.Content.Items.Placeable.Blocks;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Furniture.Fractal
{
    public class WarpedLandscape : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Daylight);
            Item.createTile = TileType<WarpedLandscapeTile>();
            Item.placeStyle = 0;
        }
    }

    public class WarpedLandscapeTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
            DustType = DustType<FractalMatterDust>();
            AddMapEntry(new Color(33, 88, 106), Language.GetText("MapObject.Painting"));
        }
    }
}