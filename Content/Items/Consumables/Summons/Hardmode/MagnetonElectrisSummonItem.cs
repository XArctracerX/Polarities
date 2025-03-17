using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.NPCs.Bosses.Hardmode.MagnetonElectris;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.Summons.Hardmode
{
    public class MagnetonElectrisSummonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.rare = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 4;
            Item.consumable = false;
        }

        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }

        public override bool? UseItem(Player player)
        {
            int type1 = ModContent.NPCType<Magneton>();
            int type2 = ModContent.NPCType<Electris>();
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				// If the player is not in multiplayer, spawn directly
				NPC.SpawnOnPlayer(player.whoAmI, type1);
				NPC.SpawnOnPlayer(player.whoAmI, type2);
			}
            return true;
        }
		
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Content.Items.Placeable.Bars.SelfsimilarBar>(), 5)
				.AddIngredient(ItemType<Content.Items.Placeable.Bars.MantellarBar>(), 5)
				.AddIngredient(ItemID.Wire, 10)
				.AddIngredient(ItemID.SoulofLight, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}