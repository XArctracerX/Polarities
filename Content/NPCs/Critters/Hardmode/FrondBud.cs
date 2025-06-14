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
    public class FrondBud : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frond Bud");
            Main.npcFrameCount[NPC.type] = 4;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 22;
            NPC.height = 22;
            DrawOffsetY = -2;

            NPC.friendly = true;
            NPC.lifeMax = 5;

            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.catchItem = (short)ItemType<FrondBudItem>();
        }

        public override void AI()
        {
            if (NPC.ai[0] == 240)
            {
                if (Main.netMode != 1)
                {
                    NPC.ai[0] = -Main.rand.Next(120);
                }
                //netupdates later

                bool attemptSuccessful = false;

                Teleport(ref attemptSuccessful);

                if (!attemptSuccessful)
                {
                    return;
                }
            }
            NPC.ai[0]++;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode != 1)
            {
                NPC.ai[0] = -Main.rand.Next(120);
            }
            //netupdates later

            bool attemptSuccessful = false;
            Teleport(ref attemptSuccessful);
            if (attemptSuccessful)
            {
                NPC.ai[0]++;
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (Main.netMode != 1)
            {
                NPC.ai[0] = -Main.rand.Next(120);
            }
            //netupdates later

            bool attemptSuccessful = false;
            Teleport(ref attemptSuccessful);
            if (attemptSuccessful)
            {
                NPC.ai[0]++;
            }
        }

        private void Teleport(ref bool attemptSuccessful)
        {
            NPC.ai[1] = 0;

            //try up to 40 times
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 40; i++)
                {
                    Vector2 tryGoalPoint = NPC.Center + new Vector2(-NPC.width / 2 + Main.rand.NextFloat(100f) * (Main.rand.Next(2) * 2 - 1), Main.rand.NextFloat(-100f, 100f));
                    tryGoalPoint.Y = 16 * (int)(tryGoalPoint.Y / 16);
                    tryGoalPoint -= new Vector2(0, NPC.height);

                    bool viable = true;

                    for (int x = (int)((tryGoalPoint.X) / 16); x <= (int)((tryGoalPoint.X + NPC.width) / 16); x++)
                    {
                        for (int y = (int)((tryGoalPoint.Y) / 16); y <= (int)((tryGoalPoint.Y + NPC.height) / 16); y++)
                        {
                            if (Main.tile[x, y].HasTile)
                            {
                                viable = false;
                                break;
                            }
                        }
                        if (!viable)
                        {
                            break;
                        }
                    }

                    if (viable)
                    {
                        for (int y = (int)((tryGoalPoint.Y + NPC.height) / 16); y < (int)((NPC.position.Y + NPC.height + 100) / 16); y++)
                        {
                            int x = (int)((tryGoalPoint.X + NPC.width / 2) / 16);
                            if (Main.tile[x, y].HasTile && (Main.tileSolid[Main.tile[x, y].TileType] || Main.tileSolidTop[Main.tile[x, y].TileType]))
                            {
                                NPC.ai[1] = tryGoalPoint.X;
                                NPC.ai[2] = y * 16 - NPC.height;
                                NPC.ai[3] = 1;
                                break;
                            }
                        }

                        if (NPC.ai[1] != 0) {
                            break;
                        }
                    }
                }
            }
            NPC.netUpdate = true;

            if (NPC.ai[1] != 0)
            {
                for (int a = 0; a < 3; a++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, newColor: Color.LightBlue, Scale: 1f);
                }

                NPC.position = new Vector2(NPC.ai[1], NPC.ai[2]);

                for (int a = 0; a < 3; a++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, newColor: Color.LightBlue, Scale: 1f);
                }

                attemptSuccessful = true;
            }

            NPC.spriteDirection = Main.rand.Next(2) * 2 - 1;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (4 * frameHeight);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (FractalSubworld.Active)
            {
                return ((FractalSubworld.SpawnConditionFractalNormal(spawnInfo) + FractalSubworld.SpawnConditionFractalCoasts(spawnInfo)) * FractalSubworld.SpawnConditionFractalOverworld(spawnInfo));
            }
            return 0f;
        }

        public override bool CheckDead()
        {
            for (int a = 0; a < 3; a++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, newColor: Color.LightBlue, Scale: 1f);
            }
            return true;
        }
    }

    internal class FrondBudItem : ModItem
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

            Item.width = 18;
            Item.height = 24;

            Item.makeNPC = (short)NPCType<FrondBud>();
        }
    }
}