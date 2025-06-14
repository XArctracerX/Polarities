using Microsoft.Xna.Framework;
using Terraria;
using Polarities.Content.Items.Weapons.Melee;
using Polarities.Content.Items.Weapons.Magic;
using Polarities.Content.Items.Weapons.Ranged;
using Polarities.Content.Items.Weapons.Summon;
using Polarities.Content.Items.Accessories.Movement.PreHardmode;
using Polarities.Content.Items.Placeable.Blocks;
using Polarities.Content.Items.Weapons.Summon.Sentries.PreHardmode;
using Polarities.Content.Items.Weapons.Melee.Knives.PreHardmode;
using Polarities.Content.Items.Consumables.Potions.PreHardmode;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.Crates
{
	public class FractalCrate : CrateBase
	{
		public override int CrateIndex => 1;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			// Drop weapons
			IItemDropRule[] weapons = new IItemDropRule[] {
				ItemDropRule.Common(ItemType<Weapons.Melee.Boomerangs.Hardmode.Sawrang>(), 1),
				ItemDropRule.Common(ItemType<Weapons.Melee.Yoyos.Hardmode.Gosperian>(), 1),
				ItemDropRule.Common(ItemType<Weapons.Magic.Books.Hardmode.MindTwister>(), 1),
				ItemDropRule.Common(ItemType<Weapons.Magic.Staffs.Hardmode.OrthogonalStaff>(), 1),
				ItemDropRule.Common(ItemType<Weapons.Ranged.Atlatls.Hardmode.Fractlatl>(), 1),
				ItemDropRule.Common(ItemType<Accessories.Combat.Defense.Hardmode.FractalAntenna>(), 1),
				ItemDropRule.Common(ItemType<Accessories.Movement.Hardmode.AntoinesCharm>(), 1),
				ItemDropRule.Common(ItemType<Tools.Misc.Hardmode.TwistedMirror>(), 1),
			};
			itemLoot.Add(new OneFromRulesRule(1, weapons));

			// Drop conch
			itemLoot.Add(ItemDropRule.Common(ItemType<Items.Tools.Misc.Hardmode.FractalConch>(), 8));

			// Drop coins
			itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 5, 13));

			// Drop ores
			itemLoot.Add(ItemDropRule.Common(ItemType<Placeable.Blocks.Fractal.FractalOre>(), 7, 30, 50));

			// Drop bars 
			itemLoot.Add(ItemDropRule.Common(ItemType<Placeable.Bars.FractalBar>(), 4, 10, 21));

			// Drop potions
			itemLoot.Add(ItemDropRule.Common(ItemType<Potions.Hardmode.TwistedRecallPotion>(), 4, 2, 5));

			// Drop resource potion
			IItemDropRule[] resourcePotions = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.HealingPotion, 1, 5, 18),
				ItemDropRule.Common(ItemID.ManaPotion, 1, 5, 18),
			};
			itemLoot.Add(new OneFromRulesRule(2, resourcePotions));

			// Drop bait
			IItemDropRule[] highendBait = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.JourneymanBait, 1, 2, 7),
				ItemDropRule.Common(ItemID.MasterBait, 1, 2, 7),
			};
			itemLoot.Add(new OneFromRulesRule(2, highendBait));
		}
	}

	public class SelfsimilarCrate : CrateBase
	{
		public override int CrateIndex => 2;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			// Drop lockbox in lieu of weapons
			itemLoot.Add(ItemDropRule.Common(ItemType<FractalLockbox>(), 1));

			// Drop conch
			itemLoot.Add(ItemDropRule.Common(ItemType<Items.Tools.Misc.Hardmode.FractalConch>(), 8));

			// Drop coins
			itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 5, 13));

			// Drop ores
			itemLoot.Add(ItemDropRule.Common(ItemType<Placeable.Blocks.Fractal.SelfsimilarOre>(), 7, 30, 50));

			// Drop bars 
			itemLoot.Add(ItemDropRule.Common(ItemType<Placeable.Bars.SelfsimilarBar>(), 4, 10, 21));

			// Drop potions
			IItemDropRule[] potions = new IItemDropRule[] {
				ItemDropRule.Common(ItemType<Potions.Hardmode.TwistedRecallPotion>(), 1, 2, 5),
				ItemDropRule.Common(ItemType<Potions.Hardmode.FractalizationPotion>(), 1, 2, 5),
				ItemDropRule.Common(ItemType<Potions.Hardmode.FractalAntidote>(), 1, 2, 5),
			};
			itemLoot.Add(new OneFromRulesRule(4, potions));

			// Drop resource potion
			IItemDropRule[] resourcePotions = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 2, 7),
				ItemDropRule.Common(ItemID.GreaterManaPotion, 1, 2, 7),
			};
			itemLoot.Add(new OneFromRulesRule(2, resourcePotions));

			// Drop bait
			IItemDropRule[] highendBait = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.JourneymanBait, 1, 2, 7),
				ItemDropRule.Common(ItemID.MasterBait, 1, 2, 7),
			};
			itemLoot.Add(new OneFromRulesRule(2, highendBait));
		}
	}

	public class FractalLockbox : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
			//DisplayName.SetDefault("Fractal Lock Box");
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Yellow;
			Item.consumable = true;
		}

		public override bool CanRightClick()
		{
			return Main.LocalPlayer.HasItem(ItemType<Keys.Hardmode.FractalKey>());
		}

        public override void RightClick(Player player)
		{
			Main.LocalPlayer.ConsumeItem(ItemType<Keys.Hardmode.FractalKey>());

			int[] itemsToPlaceInLockedFractalChests = new int[] {
					ItemType<Content.Items.Accessories.Wings.FractalWings>(),
					ItemType<Content.Items.Weapons.Ranged.Bows.Hardmode.CBow>(),
					ItemType<Content.Items.Weapons.Magic.Staffs.Hardmode.EnergyLash>(),
					ItemType<Content.Items.Accessories.Combat.Offense.Hardmode.FractalEye>(),
					ItemType<Content.Items.Weapons.Magic.Guns.Hardmode.BinaryFlux>(),
					ItemType<Content.Items.Weapons.Melee.Broadswords.Hardmode.CaliperBlades>(),
					ItemType<Content.Items.Accessories.Combat.Defense.Hardmode.FractalAbsorber>(),
					ItemType<Content.Items.Accessories.Combat.Offense.Hardmode.ChaosFlower>()
				};

			player.QuickSpawnItem(player.GetSource_FromThis(), itemsToPlaceInLockedFractalChests[Main.rand.Next(itemsToPlaceInLockedFractalChests.Length)]);
		}
	}
}