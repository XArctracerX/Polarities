using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Polarities.Content.Buffs.Hardmode;
using Microsoft.Xna.Framework;

namespace Polarities.Content.Items.Consumables.Potions.Hardmode
{
    public class FractalAntidote : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            //Tooltip.SetDefault("Temporarily halts the progression of fractalization"+"\nCan be rendered ineffective by sufficiently powerful sources of fractalization");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            
            Item.buffType = BuffType<FractalAntidoteBuff>();
            Item.buffTime = 7200;
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemType<Fish.Regularfish>())
                .AddIngredient(ItemType<Placeable.Blocks.Fractal.DendriticEnergy>())
                .AddTile(TileID.Bottles)
                .Register();
        }
    }

    public class FractalAntidoteBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            if (FractalSubworld.Active && player.HasBuff(ModContent.BuffType<Fractalizing>()))
            {
                player.buffTime[player.FindBuffIndex(ModContent.BuffType<Fractalizing>())] -= player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffRate - 1;
            }
        }
    }
}