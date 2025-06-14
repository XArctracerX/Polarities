using Microsoft.Xna.Framework;
using Polarities.Content.Buffs.Hardmode;
using Polarities.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.Potions.PreHardmode
{
    public class PiercingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (20);

            ItemID.Sets.DrinkParticleColors[Type] = new Color[] { Color.Lime, Color.LimeGreen };
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.buffType = BuffType<Fractalizing>();
            Item.buffTime = 28800;
        }

        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemType<AlkalineFluid>())
                .AddIngredient(ItemID.Waterleaf)
                .AddIngredient(ItemID.Blinkroot)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }

    public class PiercingPotionBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 6;
        }
    }
}

