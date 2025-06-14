using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SubworldLibrary;

namespace Polarities.Content.Items.Fish
{
	public class Manyfin : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//DisplayName.SetDefault("Manyfin");
			//Tooltip.SetDefault("Minor improvements to all stats" + "\nIncreases fractalization by 15 seconds when in the fractal dimension");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 42;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.White;
			Item.consumable = true;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.useStyle = 2;
			Item.UseSound = SoundID.Item2;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 18000;
		}

        public override bool ConsumeItem(Player player)
        {
			if (FractalSubworld.Active)
			{
				player.AddBuff(BuffType<Buffs.Hardmode.Fractalizing>(), 15 * 60);
				player.GetModPlayer<PolaritiesPlayer>().suddenFractalizationChange = true;
			}

			return true;
        }
	}

	public class Trilobiter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//DisplayName.SetDefault("Trilobiter");
			//Tooltip.SetDefault("Minor improvements to all stats" + "\nIncreases fractalization by 30 seconds when in the fractal dimension");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 15);
			Item.rare = ItemRarityID.White;
			Item.consumable = true;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.useStyle = 2;
			Item.UseSound = SoundID.Item2;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 28800;
		}

		public override bool ConsumeItem(Player player)
		{
			if (FractalSubworld.Active)
			{
				player.AddBuff(BuffType<Buffs.Hardmode.Fractalizing>(), 30 * 60);
				player.GetModPlayer<PolaritiesPlayer>().suddenFractalizationChange = true;
			}

			return true;
		}
	}

	public class Mirrorfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//DisplayName.SetDefault("Mirrorfish");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
		}
	}

	public class Regularfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//DisplayName.SetDefault("Regular Fish");
			//Tooltip.SetDefault("'An ordinary fish'");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
		}
	}

	public class SelfsimilarScabbardfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
			//DisplayName.SetDefault("Selfsimilar Scabbardfish");
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 52;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
		}
	}
}