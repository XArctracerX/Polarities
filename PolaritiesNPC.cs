using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Polarities.Global;
using Polarities.Core;
//using Polarities.Biomes;
//using Polarities.Biomes.Fractal;
//using Polarities.Buffs;
//using Polarities.Items;
//using Polarities.Items.Accessories;
//using Polarities.Items.Armor.MechaMayhemArmor;
//using Polarities.Items.Books;
//using Polarities.Items.Weapons.Magic;
//using Polarities.Items.Weapons.Melee;
//using Polarities.Items.Weapons.Melee.Warhammers;
//using Polarities.Items.Weapons.Ranged;
//using Polarities.Items.Weapons.Ranged.Atlatls;
//using Polarities.Items.Weapons.Summon.Orbs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.ModLoader.ModContent;

namespace Polarities
{
    //public enum NPCCapSlotID
    //{
        //HallowInvasion,
        //WorldEvilInvasion,
        //WorldEvilInvasionWorm,
    //}

    public class PolaritiesNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public Dictionary<int, int> hammerTimes;

        public bool flawless = true;

        public bool usesProjectileHitCooldowns = false;
        public int projectileHitCooldownTime = 0;
        public int ignoredDefenseFromCritAmount;

        public float defenseMultiplier;

        public int tentacleClubs;
        public int chlorophyteDarts;
        public int contagunPhages;
        public int boneShards;

        public int desiccation;
        public int incineration;
        public bool coneVenom;

        public static Dictionary<int, bool> bestiaryCritter = new Dictionary<int, bool>();

        public override void Load()
        {
            //Terraria.On_NPC.GetNPCColorTintedByBuffs += NPC_GetNPCColorTintedByBuffs;

            //Terraria.IL_NPC.StrikeNPC += NPC_StrikeNPC;

            //counts weird critters
            //Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.AddEmptyEntries_CrittersAndEnemies_Automated += BestiaryDatabaseNPCsPopulator_AddEmptyEntries_CrittersAndEnemies_Automated;
            //Terraria.GameContent.Bestiary.IL_NPCWasNearPlayerTracker.ScanWorldForFinds += NPCWasNearPlayerTracker_ScanWorldForFinds;
            //Terraria.On_NPC.HittableForOnHitRewards += NPC_HittableForOnHitRewards;

            //avoid bad spawns
            //IL_ChooseSpawn += PolaritiesNPC_IL_ChooseSpawn;

            //flawless continuity for EoW
            //Terraria.On_NPC.Transform += NPC_Transform;

            //force counts things for the radar
            //Terraria.IL_Main.DrawInfoAccs += Main_DrawInfoAccs;

            //allows npcs to spawn in lava
            //moves prismatic lacewings to post-sun-pixie
            //Terraria.IL_NPC.SpawnNPC += NPC_SpawnNPC;
        }

        public override void SetDefaults(NPC npc)
        {
            hammerTimes = new Dictionary<int, int>();

            //switch (npc.type)
            //{
            //case NPCID.DungeonGuardian:
            //npc.buffImmune[BuffType<Incinerating>()] = true;
            //break;
            //}
        }

        private static bool? IsBestiaryCritter(int npcType)
        {
            return bestiaryCritter.ContainsKey(npcType) ? bestiaryCritter[npcType] : null;
        }

        //public override void NPC_Transform(Terraria.On_NPC.orig_Transform orig, NPC self, int newType)
        //{
            //bool flawless = self.GetGlobalNPC<PolaritiesNPC>().flawless;
            //Dictionary<int, int> hammerTimes = self.GetGlobalNPC<PolaritiesNPC>().hammerTimes;

            //orig(self, newType);

            //self.GetGlobalNPC<PolaritiesNPC>().flawless = flawless;
            //self.GetGlobalNPC<PolaritiesNPC>().hammerTimes = hammerTimes;
        //}

        public override void ResetEffects(NPC npc)
        {
            defenseMultiplier = 1f;
            //neutralTakenDamageMultiplier = 1f;

            //List<int> removeKeys = new List<int>();
            //foreach (int i in hammerTimes.Keys)
            //{
            //hammerTimes[i]--;
            //if (hammerTimes[i] <= 0)
            //{
            //removeKeys.Add(i);
            //}
            //}
            //foreach (int i in removeKeys)
            //{
            //hammerTimes.Remove(i);
            //}

            //contagunPhages = 0;
            //tentacleClubs = 0;
            chlorophyteDarts = 0;
            //boneShards = 0;

            desiccation = 0;
            incineration = 0;
            coneVenom = false;

            //whipTagDamage = 0;

            //if (!spiritBite)
            //{
                //spiritBiteLevel = 0;
            //}
            //spiritBite = false;
        }

        public void ModifyDefense(NPC npc, ref int defense)
        {
            defense = (int)(defense * defenseMultiplier);

            int hammerDefenseLoss = 0;
            foreach (int i in hammerTimes.Keys)
            {
                if (i > hammerDefenseLoss && hammerTimes[i] > 0) hammerDefenseLoss = i;
            }

            defense -= hammerDefenseLoss;

            defense -= ignoredDefenseFromCritAmount;

            defense -= boneShards;
        }


        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (!npc.buffImmune[BuffID.BoneJavelin])
            {
                //if (tentacleClubs > 0)
                //{
                    //if (npc.lifeRegen > 0)
                    //{
                        //npc.lifeRegen = 0;
                    //}
                    //int amountLoss = tentacleClubs * 10;
                    //npc.lifeRegen -= amountLoss * 2;
                    //if (damage < amountLoss)
                    //{
                        //damage = amountLoss;
                    //}
                //}
                if (chlorophyteDarts > 0)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    int amountLoss = chlorophyteDarts * 24;
                    npc.lifeRegen -= amountLoss * 2;
                    if (damage < amountLoss)
                    {
                        damage = amountLoss;
                    }
                }
                //if (contagunPhages > 0)
                //{
                    //if (npc.lifeRegen > 0)
                    //{
                        //npc.lifeRegen = 0;
                    //}
                    //int amountLoss = contagunPhages * 10;
                    //npc.lifeRegen -= amountLoss * 2;
                    //if (damage < amountLoss)
                    //{
                        //damage = amountLoss;
                    //}
                //}
                //if (boneShards > 0)
                //{
                    //if (npc.lifeRegen > 0)
                    //{
                        //npc.lifeRegen = 0;
                    //}
                    //int amountLoss = boneShards * 2;
                    //npc.lifeRegen -= amountLoss * 2;
                    //if (damage < amountLoss)
                    //{
                        //damage = amountLoss;
                    //}
                //}
            }
            if (desiccation > 60 * 10)
            {
                npc.lifeRegen -= 60;
                if (damage < 5)
                {
                    damage = 5;
                }
            }
            if (coneVenom)
            {
                npc.lifeRegen -= 140;
                if (damage < 12)
                {
                    damage = 12;
                }
            }
            //if (incineration > 0)
            //{
                //npc.lifeRegen -= incineration * 2;
                //if (damage < incineration / 6)
                //{
                    //damage = incineration / 6;
                //}
            //}

            //if (contagunPhages > 10)
            //{
                //SoundEngine.PlaySound(SoundID.Item17, npc.Center);
                //for (int i = 0; i < Main.maxProjectiles; i++)
                //{
                    //if (Main.projectile[i].active && Main.projectile[i].type == ProjectileType<ContagunVirusProjectile>() && Main.projectile[i].ai[0] == npc.whoAmI + 1)
                    //{
                        //Main.projectile[i].ai[0] = 0;
                        //Main.projectile[i].velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.5f, 2f);

                        //Projectile.NewProjectile(Main.projectile[i].GetSource_FromAI(), Main.projectile[i].Center, new Vector2(Main.rand.NextFloat(5f)).RotatedByRandom(MathHelper.TwoPi), ProjectileType<ContagunProjectile>(), Main.projectile[i].damage, Main.projectile[i].knockBack, Main.projectile[i].owner, ai0: Main.rand.NextFloat(MathHelper.TwoPi), ai1: 240f);
                    //}
                //}
            //}

            //UpdateCustomSoulDrain(npc);
        }
    }
}

