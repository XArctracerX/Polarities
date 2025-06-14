using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;

namespace Polarities.Content.NPCs.Critters.Hardmode
{
	internal class SparkFly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sparkfly");
			Main.npcFrameCount[NPC.type] = 6;
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.TrailCacheLength[NPC.type] = 8;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCAIStyleID.Firefly);
			NPC.friendly = true;
			NPC.aiStyle = NPCID.LightningBug;
			NPC.scale = 1f;
			//animationType = NPCID.LightningBug;

			NPC.catchItem = (short)ItemType<SparkFlyItem>();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return true;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;

			NPC.frameCounter++;
			if (NPC.frameCounter == 3)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (frameHeight * 6);
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.localAI[2] <= 3f)
			{
				for (int i = 0; i < NPC.oldPos.Length; i++)
				{
					spriteBatch.Draw(ModContent.Request<Texture2D>("Polarities/Content/NPCs/Critters/Hardmode/SparkFlyTrail").Value, NPC.oldPos[i] + new Vector2(NPC.spriteDirection == -1 ? 6 : 6, 11) - Main.screenPosition, new Rectangle(0, (i / 2) * 10, 10, 10), Color.White * 0.5f, NPC.rotation, new Vector2(6, 7), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}

			return true;
		}

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.localAI[2] <= 3f)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>("Polarities/Content/NPCs/Critters/Hardmode/SparkFlyGlow").Value, NPC.Center + new Vector2(0, 3) - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, new Vector2(6, 7), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (FractalSubworld.Active)
			{
				return FractalSubworld.SpawnConditionFractalSky(spawnInfo);
			}
			return 0f;
		}
	}

	internal class SparkFlyItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Firefly);
			Item.bait = 35;
			Item.rare = ItemRarityID.Green;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.reuseDelay = 0;
			Item.channel = false;
			Item.makeNPC = (short)NPCType<SparkFly>();
		}
	}
}
