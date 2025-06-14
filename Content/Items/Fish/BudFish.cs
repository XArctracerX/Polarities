using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Fish
{
	public class BudFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bud Fish");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 42;
			Item.height = 42;
			Item.uniqueStack = true;
			Item.rare = ItemRarityID.Quest;
		}

		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
			return PolaritiesSystem.downedRiftDenizen;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = "I was poking that rift that formed in the sky, and get this: I heard the sound of something swimming around on the other side! I don't know what kind of fish can survive in there, but whatever it is I want it!";
			catchLocation = "Caught in the Fractal Dimension.";
		}
	}
}
