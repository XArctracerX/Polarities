using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using System.IO;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
    public class FractalTimer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fractal Timer");
            //Tooltip.SetDefault("Accelerates the fractalization process");
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = 10000;
            Item.rare = ItemRarityID.Lime;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.channel = true;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item29;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                //accelerate time
                player.itemTime = 1;
                player.itemAnimation = 1;
                player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffRate *= 10;
                if (FractalSubworld.Active && player.HasBuff(ModContent.BuffType<Buffs.Hardmode.Fractalizing>()))
                {
                    player.buffTime[player.FindBuffIndex(ModContent.BuffType<Buffs.Hardmode.Fractalizing>())] += player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffRate * 2;
                }
            }
            player.itemLocation = player.Center - new Microsoft.Xna.Framework.Vector2(player.direction*10,-10);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<Placeable.Blocks.Fractal.DendriticEnergy>(), 24)
                .AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 12)
                .AddIngredient(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
                .Register();
        }
    }
}