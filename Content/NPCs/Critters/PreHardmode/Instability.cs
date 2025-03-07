using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.NPCs.Bosses.PreHardmode.RiftDenizen;
using Polarities.Content.Items.Consumables.Summons.PreHardmode;

namespace Polarities.Content.NPCs.Critters.PreHardmode
{
    internal class Instability : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Instability");
            Main.npcFrameCount[NPC.type] = 1;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.width = 18;
            NPC.height = 22;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCHit36;
            NPC.catchItem = (short)ItemType<RiftDenizenSummon>();
            NPC.knockBackResist = 0f;
            NPC.rarity = 1;
            NPC.dontCountMe = true;
        }

        public override void AI()
        {
            Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, Scale: 0.25f)].noGravity = true;

            NPC.ai[0]++;
            if (NPC.ai[0] == 240)
            {
                if (Main.netMode != 1)
                {
                    NPC.ai[0] = Main.rand.Next(60);
                    NPC.ai[1] = NPC.ai[0];

                    NPC.position += new Vector2(Main.rand.NextFloat(60, 240)).RotatedByRandom(MathHelper.TwoPi);
                }
                NPC.netUpdate = true;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Sky)
            {
                return 0.6f;
            }
            return 0f;
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < 16; i++)
            {
                Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, Scale: 0.25f)].noGravity = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame = texture.Frame();

            float drawScaleX = Math.Min(1, Math.Min((NPC.ai[0] - NPC.ai[1]) / 8f, (240 - NPC.ai[0]) / 8f));

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, frame, Color.White, NPC.rotation, frame.Size() / 2, new Vector2(drawScaleX, 1) * NPC.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}