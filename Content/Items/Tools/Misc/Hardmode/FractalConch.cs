using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
	public class FractalConch : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Conch");
			//Tooltip.SetDefault("If you listen closely, you can hear the fractal ocean");
		}

		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 30;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 90;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item6;
		}

        public override bool CanUseItem(Player player)
        {
            return FractalSubworld.Active;
        }

		/*
        public override void HoldItem(Player player)
		{
			if (player.itemAnimation > 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.1f);
				}
				if (player.itemTime == 0)
				{
					player.itemTime = Player.Hooks.TotalUseTime(Item.useTime, player, Item);
				}
				else if (player.itemTime == Player.Hooks.TotalUseTime(Item.useTime, player, Item) / 2)
				{
					for (int num360 = 0; num360 < 70; num360++)
					{
						Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, default(Color), 1.5f);
					}
					player.grappling[0] = -1;
					player.grapCount = 0;
					for (int num359 = 0; num359 < 1000; num359++)
					{
						if (Main.projectile[num359].active && Main.projectile[num359].owner == player.whoAmI && Main.projectile[num359].aiStyle == 7)
						{
							Main.projectile[num359].Kill();
						}
					}

					Spawn(player);

					for (int num358 = 0; num358 < 70; num358++)
					{
						Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
					}
				}
			}
		}
		*/

		private void Spawn(Player player)
		{
			//Main.InitLifeBytes();
			if (player.whoAmI == Main.myPlayer)
			{
				if (Main.mapTime < 5)
				{
					Main.mapTime = 5;
				}
				Main.bgStyle = 10;
				player.FindSpawn();
				if (!Player.CheckSpawn(player.SpawnX, player.SpawnY))
				{
					player.SpawnX = -1;
					player.SpawnY = -1;
				}
				Main.maxQ = true;
			}
			if (Main.netMode == 1 && player.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(12, -1, -1, null, Main.myPlayer);
				Main.gameMenu = false;
			}
			player.headPosition = Vector2.Zero;
			player.bodyPosition = Vector2.Zero;
			player.legPosition = Vector2.Zero;
			player.headRotation = 0f;
			player.bodyRotation = 0f;
			player.legRotation = 0f;
			player.lavaTime = player.lavaMax;
			if (player.statLife <= 0)
			{
				int num = player.statLifeMax2 / 2;
				player.statLife = 100;
				if (num > player.statLife)
				{
					player.statLife = num;
				}
				player.breath = player.breathMax;
				if (player.spawnMax)
				{
					player.statLife = player.statLifeMax2;
					player.statMana = player.statManaMax2;
				}
			}
			player.immune = true;
			/*if (player.dead)
			{
				Player.Hooks.OnRespawn(player);
			}*/
			player.dead = false;
			player.immuneTime = 0;
			player.active = true;


			//go to ocean location
			int x = Main.offLimitBorderTiles + 2;
			int y = Main.offLimitBorderTiles + 4;
			if (player.position.X < Main.maxTilesX * 8)
            {
				x = Main.maxTilesX - Main.offLimitBorderTiles - 4;
			}
			bool go = true;
			while (y < Main.maxTilesY - Main.offScreenRange - 4 && go)
            {
				y++;

				if (Main.tile[x, y].LiquidAmount > 0 || Main.tile[x-1, y].LiquidAmount > 0 || Main.tile[x+1, y].LiquidAmount > 0)
                {
					go = false;
                }
            }
			go = true;
			while (y > Main.offLimitBorderTiles + 4 && go)
            {
				y--;

				if (
					(!Main.tile[x, y].HasTile || !Main.tileSolid[Main.tile[x, y].TileType]) &&
					(!Main.tile[x-1, y].HasTile || !Main.tileSolid[Main.tile[x-1, y].TileType]) &&
					(!Main.tile[x+1, y].HasTile || !Main.tileSolid[Main.tile[x+1, y].TileType]) &&
					(!Main.tile[x, y-1].HasTile || !Main.tileSolid[Main.tile[x, y-1].TileType]) &&
					(!Main.tile[x-1, y-1].HasTile || !Main.tileSolid[Main.tile[x-1, y-1].TileType]) &&
					(!Main.tile[x+1, y-1].HasTile || !Main.tileSolid[Main.tile[x+1, y-1].TileType]) &&
					(!Main.tile[x, y-2].HasTile || !Main.tileSolid[Main.tile[x, y-2].TileType]) &&
					(!Main.tile[x-1, y-2].HasTile || !Main.tileSolid[Main.tile[x-1, y-2].TileType]) &&
					(!Main.tile[x+1, y-2].HasTile || !Main.tileSolid[Main.tile[x+1, y-2].TileType])
					)
				{
					go = false;
				}
			}

			player.position.X = x * 16 + 8 - player.width / 2;
			player.position.Y = y * 16 - player.height;


			player.wet = false;
			player.wetCount = 0;
			player.lavaWet = false;
			player.fallStart = (int)(player.position.Y / 16f);
			player.fallStart2 = player.fallStart;
			player.velocity.X = 0f;
			player.velocity.Y = 0f;
			for (int j = 0; j < 3; j++)
			{
				player.UpdateSocialShadow();
			}
			player.oldPosition = player.position + player.BlehOldPositionFixer;
			if (player.talkNPC != -1) player.SetTalkNPC(player.talkNPC, false);
			if (player.whoAmI == Main.myPlayer)
			{
				Main.npcChatCornerItem = 0;
			}
			if (player.pvpDeath)
			{
				player.pvpDeath = false;
				player.immuneTime = 300;
				player.statLife = player.statLifeMax;
			}
			else
			{
				player.immuneTime = 60;
			}
			if (player.whoAmI == Main.myPlayer)
			{
				Main.BlackFadeIn = 255;
				Main.renderNow = true;
				if (Main.netMode == 1)
				{
					//Netplay.newRecent();
				}
				Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
				Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
			}
		}
	}
}
