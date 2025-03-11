using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Polarities.Content.Projectiles;
using Polarities.Content.Items.Placeable;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.ExpertMode.PreHardmode
{
	[AutoloadEquip(EquipType.Back, EquipType.Front)]
	public class CloakofPockets : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cloak of Pockets");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			try
			{
				string hotkey = "an unbound hotkey";
				if (Polarities.RiftDodgeHotKey.GetAssignedKeys().ToArray().Length > 0)
				{
					hotkey = Polarities.RiftDodgeHotKey.GetAssignedKeys()[0];

				}
				TooltipLine line = new TooltipLine(Mod, "Tooltip1", string.Format("Press {0} to hide in your cloak, or to emerge if already hidden" + "\nYou cannot use weapons and your minions will not target enemies while hidden" + "\nYou can only remain hidden for 10 seconds at a time, and the effect has a 1 minute cooldown", hotkey));
				tooltips.Insert(tooltips.ToArray().Length - 1, line);
			}
			catch (Exception e)
            {

            }
		}

        public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 40;

			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
			Item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PolaritiesPlayer>().hasRiftDodge = true;
		}

		public static void RenderBack(PlayerDrawSet drawInfo)
        {
			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead)
			{
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("Polarities");

			// The texture we want to display on our player
			Texture2D texture = ModContent.Request<Texture2D>("Polarities/Content/Items/Accessories/ExpertMode/PreHardmode/CloakofPocketsFolding_Front").Value;

			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2 - 20f;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 - 1f + 5f * drawPlayer.gravDir; ;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

			int frameIndex = Math.Min(Math.Min(10, (PolaritiesPlayer.RIFT_DODGE_MAX_LENGTH - drawPlayer.GetModPlayer<PolaritiesPlayer>().riftDodgeTimer) / 3), (drawPlayer.GetModPlayer<PolaritiesPlayer>().riftDodgeTimer - 1) / 3);

			float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Color.White * drawPlayer.stealth;
			Rectangle frame = new Rectangle(0, frameIndex * 56, 80, 56);
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;

			DrawData drawData = new DrawData(texture, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.cFront;
			//Main.playerDrawData.Add(drawData);
		}

		public static void RenderFront(PlayerDrawSet drawInfo)
        {
			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead)
			{
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("Polarities");

            // The texture we want to display on our player
            Texture2D texture = ModContent.Request<Texture2D>("Polarities/Content/Items/Accessories/ExpertMode/CloakofPocketsFolding_Front").Value;

			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2 - 20f;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 - 1f + 5f * drawPlayer.gravDir; ;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

			int frameIndex = Math.Min(Math.Min(10, (PolaritiesPlayer.RIFT_DODGE_MAX_LENGTH - drawPlayer.GetModPlayer<PolaritiesPlayer>().riftDodgeTimer) / 3), (drawPlayer.GetModPlayer<PolaritiesPlayer>().riftDodgeTimer - 1) / 3);

			float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Color.White * drawPlayer.stealth;
			Rectangle frame = new Rectangle(0, frameIndex * 56, 80, 56);
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;

			DrawData drawData = new DrawData(texture, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.cFront;
			//Main.playerDrawData.Add(drawData);
		}
	}
}
