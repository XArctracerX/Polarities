﻿using Polarities.Content.Items.Placeable.Banners.Items;
using Polarities.Content.Items.Weapons.Melee.Misc;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Buffs.Hardmode;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.NPCs.Enemies.Ocean.Hardmode
{
    public class ConeShell : ModNPC
    {
        private int attackCooldown;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.GlowingSnail];
            Main.npcCatchable[NPC.type] = true;

            PolaritiesNPC.forceCountForRadar.Add(Type);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                //spawn conditions
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				//flavor text
				this.TranslatedBestiaryEntry()
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 14;
            NPC.height = 14;
            NPC.aiStyle = 67;
            NPC.damage = 1;
            NPC.defense = 5;
            NPC.lifeMax = 100;
            NPC.knockBackResist = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.npcSlots = 0.5f;
            NPC.noGravity = true;
            NPC.value = 1000;

            AIType = NPCID.GlowingSnail;
            AnimationType = NPCID.GlowingSnail;

            Banner = NPCType<ConeShell>();
            BannerItem = ItemType<ConeShellBanner>();
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.damage = 1;
            NPC.defense = 5;
            NPC.lifeMax = 200;
        }

        public override bool PreAI()
        {
            if (attackCooldown == 0)
            {
                NPC.damage = 1;
            }
            else
            {
                attackCooldown--;
                NPC.damage = 0;
            }

            if (NPC.life < NPC.lifeMax / 2 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.catchItem = (short)ItemType<HandheldConeShell>();
                NPC.netUpdate = true;
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<ConeVenom>(), 3 * 60);
            attackCooldown = Main.expertMode ? 7 * 60 : 4 * 60;
        }

        public override bool CheckDead()
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ConeShellGore").Type);
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Ocean.Chance * 0.5f;
            }
            else
            {
                return 0f;
            }
        }
    }
}
