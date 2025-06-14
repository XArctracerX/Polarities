using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Polarities.Global;

namespace Polarities.Content.Items.Accessories.Wings
{
	[AutoloadEquip(EquipType.Wings)]
	public class EclipxieWings : ModItem, IDrawArmor
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//Tooltip.SetDefault("'Soar like the sun'");
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 2.5f);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PolaritiesPlayer>().hasGlide = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.3f;
			constantAscend = 0.135f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FairyWings)
				.AddIngredient(ItemType<Materials.Hardmode.EclipxieDust>(), 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public bool Override()
		{
			return false;
		}

		public void DrawArmor(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!drawPlayer.invis)
			{
				Texture2D texture = Request<Texture2D>("Polarities/Content/Items/Accessories/Wings/EclipxieWings_Wings").Value;

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2 - 5f - drawPlayer.direction * 13f;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 3f - 11f * drawPlayer.gravDir;
				Vector2 origin = new Vector2(20, 29);
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

				Color color = drawInfo.colorArmorBody;
				Rectangle frame = new Rectangle(0, drawPlayer.GetModPlayer<PolaritiesPlayer>().bubbyWingFrame * 58, 50, 58);
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new DrawData(texture, position, frame, color, rotation, origin, 1f, spriteEffects, 0)
				{
					shader = drawInfo.cWings
				};
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	[AutoloadEquip(EquipType.Wings)]
	public class FractalWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Fractal Dragon Wings");
			//Tooltip.SetDefault("Allows you to dive by pressing down");
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 2.5f);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 48;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = 8;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 180;
			player.GetModPlayer<PolaritiesPlayer>().hasDive = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.3f;
			constantAscend = 0.135f;
		}
	}

	[AutoloadEquip(EquipType.Wings)]
	public class ElectricWings : ModItem, IDrawArmor
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Electric Wings");
			//Tooltip.SetDefault("Allows you to decelerate instantly");
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8.5f, 3f);
			ArmorMasks.wingIndexToArmorDraw.TryAdd(EquipLoader.GetEquipSlot(Mod, Name, EquipType.Wings), this);
		}
		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 40;
			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 180;
			player.GetModPlayer<PolaritiesPlayer>().hasInstantDeceleration = true;
			if (player.velocity.Length() > 6)
			{
				Dust dust = Main.dust[Dust.NewDust(player.position + new Vector2(-player.direction * 20, 0), player.width, 2 * player.height / 3, DustID.Electric, newColor: Color.LightBlue, Scale: 0.4f)];
				dust.velocity = player.velocity / 2 + new Vector2(3, 0).RotatedByRandom(MathHelper.Pi);
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
			}
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.5f;
			constantAscend = 0.135f;
		}

		public bool Override()
		{
			return false;
		}

		public void DrawArmor(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (false) //if (!drawPlayer.invis)
			{
				Texture2D texture = Request<Texture2D>("Polarities/Content/Items/Accessories/Wings/ElectricWings_Wings_Mask").Value;

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2 - 5f - drawPlayer.direction * 13f;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 3f - 11f * drawPlayer.gravDir;
				Vector2 origin = new Vector2(20, 29);
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

				Color color = drawInfo.colorArmorBody;
				Rectangle frame = new Rectangle(0, drawPlayer.GetModPlayer<PolaritiesPlayer>().bubbyWingFrame * 58, 50, 58);
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new DrawData(texture, position, frame, color, rotation, origin, 1f, spriteEffects, 0)
				{
					shader = drawInfo.cWings
				};
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}