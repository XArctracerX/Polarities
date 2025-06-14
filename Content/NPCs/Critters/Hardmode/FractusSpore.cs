using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SubworldLibrary;

namespace Polarities.Content.NPCs.Critters.Hardmode
{
    public class FractusSpore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fractus Spore");
            Main.npcFrameCount[NPC.type] = 6;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 24;
            NPC.height = 22;
            DrawOffsetY = -2;

            NPC.friendly = true;
            NPC.lifeMax = 5;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.catchItem = ItemType<FractusSporeItem>();
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.direction = 1;
            }

            Lighting.AddLight(NPC.Center, 192f / 255, 64f / 255, 128f / 255);

            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X = 0;

                NPC.ai[0]++;
                if (NPC.ai[0] >= 60)
                {
                    NPC.velocity.Y = -8;
                    NPC.velocity.X = NPC.direction * Main.rand.NextFloat(1.75f, 2.25f);
                    NPC.ai[0] = 0;
                }
            }
            if (NPC.collideX && Main.rand.NextBool())
            {
                NPC.direction *= -1;
            }
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
            if ((NPC.ai[0] >= 50 && NPC.ai[0] < 55) || (NPC.ai[0] < 5 && NPC.velocity.Y == 0))
            {
                NPC.frame.Y = frameHeight;
            }
            else if (NPC.ai[0] >= 55)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.velocity.Y == 0)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.velocity.Y < -1)
            {
                NPC.frame.Y = frameHeight * 3;
            }
            else if (NPC.velocity.Y < 1)
            {
                NPC.frame.Y = frameHeight * 4;
            }
            else
            {
                NPC.frame.Y = frameHeight * 5;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (FractalSubworld.Active)
            {
                return FractalSubworld.SpawnConditionFractalWastes(spawnInfo) * (1 - FractalSubworld.SpawnConditionFractalSky(spawnInfo));
            }
            return 0f;
        }

        public override bool CheckDead()
        {
            for (int a = 0; a < 3; a++)
            {
                Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 134, newColor: Color.Pink, Scale: 1f).noGravity = true;
            }
            return true;
        }
    }

    internal class FractusSporeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Firefly);
            Item.bait = 0;
            Item.rare = 1;

            Item.width = 26;
            Item.height = 22;

            Item.makeNPC = NPCType<FractusSpore>();
        }
    }
}