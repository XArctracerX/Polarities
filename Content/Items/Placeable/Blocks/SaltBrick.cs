﻿using Microsoft.Xna.Framework;
using Polarities.Assets.Dusts;
using Polarities.Content.Items.Placeable.Furniture.Salt;
using Polarities.Content.Items.Placeable.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Blocks
{
    public class SaltBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[Item.type] = ItemID.Sets.SortingPriorityMaterials[ItemID.MarbleBlock];

            Item.ResearchUnlockCount = (100);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<SaltBrickTile>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemType<RockSalt>())
                .AddIngredient(ItemID.StoneBlock)
                .AddTile(TileID.Furnaces)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemType<SaltPlatform>(), 2)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemType<SaltBrickWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class SaltBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            AddMapEntry(new Color(226, 205, 227));

            DustType = DustType<SaltDust>();
            
            HitSound = SoundID.Tink;

            MinPick = 0;
        }
    }
}