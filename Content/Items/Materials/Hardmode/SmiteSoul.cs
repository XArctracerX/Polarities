using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Materials.Hardmode
{
	public class SmiteSoul : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Soul of Smite");
			//Tooltip.SetDefault("'The essence of electricity'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;

			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.SoulofSight);
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Red;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.75f * Main.essScale);
		}
	}
}