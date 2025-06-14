using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using SubworldLibrary;

namespace Polarities.Content.Items.Consumables.Potions.Hardmode
{
    public class FractalizationPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Imbues you with as much fractalization as you can handle");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
        }

        public override bool CanUseItem(Player player)
        {
            return FractalSubworld.Active;
        }

        public override bool? UseItem(Player player)
        {
            if (player.buffTime[player.FindBuffIndex(BuffType<Buffs.Hardmode.Fractalizing>())] > player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks)
                player.buffTime[player.FindBuffIndex(BuffType<Buffs.Hardmode.Fractalizing>())] += 3600;
            else player.buffTime[player.FindBuffIndex(BuffType<Buffs.Hardmode.Fractalizing>())] = player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemType<Fish.SelfsimilarScabbardfish>())
                .AddIngredient(ItemType<Placeable.Blocks.Fractal.Fractus>())
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}