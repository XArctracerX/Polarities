using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Polarities.Global;
using Polarities.Core;
using Polarities.Content.Biomes;
using Polarities.Content.Events;
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
    public enum NPCCapSlotID
    {
        HallowInvasion,
        WorldEvilInvasion,
        WorldEvilInvasionWorm,
    }

    public class PolaritiesNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public Dictionary<int, int> hammerTimes;

        public bool flawless = true;

        public bool usesProjectileHitCooldowns = false;
        public int projectileHitCooldownTime = 0;
        public int ignoredDefenseFromCritAmount;
        public int whipTagDamage;

        public float defenseMultiplier;

        public int tentacleClubs;
        public int chlorophyteDarts;
        public int contagunPhages;
        public int boneShards;

        public int desiccation;
        public int incineration;
        public bool coneVenom;

        public bool spiritBite;
        public int spiritBiteLevel;

        public static Dictionary<int, bool> bestiaryCritter = new Dictionary<int, bool>();

        public static Dictionary<int, int> npcTypeCap = new Dictionary<int, int>();

        public static Dictionary<int, NPCCapSlotID> customNPCCapSlot = new Dictionary<int, NPCCapSlotID>();
        public static Dictionary<NPCCapSlotID, float> customNPCCapSlotCaps = new Dictionary<NPCCapSlotID, float>
        {
            [NPCCapSlotID.HallowInvasion] = 6f,
            [NPCCapSlotID.WorldEvilInvasion] = 2f,
            [NPCCapSlotID.WorldEvilInvasionWorm] = 2f,
        };

        public static HashSet<int> customSlimes = new HashSet<int>();
        public static HashSet<int> forceCountForRadar = new HashSet<int>();
        public static HashSet<int> canSpawnInLava = new HashSet<int>();

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

        public override void Unload()
        {
            bestiaryCritter = null;
            customNPCCapSlot = null;
            customNPCCapSlotCaps = null;
            customSlimes = null;
            forceCountForRadar = null;
            canSpawnInLava = null;

            //IL_ChooseSpawn -= PolaritiesNPC_IL_ChooseSpawn;
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

        private void Main_DrawInfoAccs(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld(typeof(Main).GetField("npc", BindingFlags.Public | BindingFlags.Static)),
                i => i.MatchLdloc(38),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(typeof(Entity).GetField("active", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchBrfalse(out _),
                i => i.MatchLdsfld(typeof(Main).GetField("npc", BindingFlags.Public | BindingFlags.Static)),
                i => i.MatchLdloc(38),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(typeof(NPC).GetField("friendly", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchBrtrue(out _),
                i => i.MatchLdsfld(typeof(Main).GetField("npc", BindingFlags.Public | BindingFlags.Static)),
                i => i.MatchLdloc(38),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(typeof(NPC).GetField("damage", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchLdcI4(0),
                i => i.MatchBle(out _),
                i => i.MatchLdsfld(typeof(Main).GetField("npc", BindingFlags.Public | BindingFlags.Static)),
                i => i.MatchLdloc(38),
                i => i.MatchLdelemRef(),
                i => i.MatchLdfld(typeof(NPC).GetField("lifeMax", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchLdcI4(5),
                i => i.MatchBle(out _)
                ))
            {
                GetInstance<Polarities>().Logger.Debug("Failed to find patch location");
                return;
            }

            ILLabel label = c.DefineLabel();
            label.Target = c.Next;

            c.Index -= 17;

            c.Emit(OpCodes.Ldloc, 38);
            c.EmitDelegate<Func<int, bool>>((index) =>
            {
                //return true to force counting
                return forceCountForRadar.Contains(Main.npc[index].type);
            });
            c.Emit(OpCodes.Brtrue, label);
        }

        public static bool lavaSpawnFlag;

        private void PolaritiesNPC_IL_ChooseSpawn(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdloc(0),
                i => i.MatchLdcI4(0),
                i => i.MatchLdcR4(1),
                i => i.MatchCallvirt(typeof(IDictionary<int, float>).GetProperty("Item", BindingFlags.Public | BindingFlags.Instance).GetSetMethod())
                ))
            {
                GetInstance<Polarities>().Logger.Debug("Failed to find patch location 1");
                return;
            }

            //remove vanilla spawns if conditions are met
            c.Emit(OpCodes.Ldloc, 0);
            c.Emit(OpCodes.Ldarg, 0);
            c.EmitDelegate<Action<IDictionary<int, float>, NPCSpawnInfo>>((pool, spawnInfo) =>
            {
                //don't remove vanilla spawns from pillars
                if (spawnInfo.Player.ZoneTowerSolar || spawnInfo.Player.ZoneTowerStardust || spawnInfo.Player.ZoneTowerNebula || spawnInfo.Player.ZoneTowerVortex) return;

                //remove vanilla spawns if in pestilence/rapture/fractal
                if (spawnInfo.Player.InModBiome(GetInstance<HallowInvasion>()) || spawnInfo.Player.InModBiome(GetInstance<HallowInvasion>())) //|| spawnInfo.Player.InModBiome<FractalBiome>())
                {
                    pool.Remove(0);
                }
            });

            ILLabel label = null;

            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchBleUn(out label),
                i => i.MatchLdloc(0),
                i => i.MatchLdloc(4),
                i => i.MatchCallvirt(typeof(ModNPC).GetProperty("NPC", BindingFlags.Public | BindingFlags.Instance).GetGetMethod()),
                i => i.MatchLdfld(typeof(NPC).GetField("type", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchLdloc(5),
                i => i.MatchCallvirt(typeof(IDictionary<int, float>).GetProperty("Item", BindingFlags.Public | BindingFlags.Instance).GetSetMethod())
                ))
            {
                GetInstance<Polarities>().Logger.Debug("Failed to find patch location 2");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Ldloc, 0);
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<IDictionary<int, float>, NPCSpawnInfo, ModNPC, bool>>((pool, spawnInfo, modNPC) =>
            {
                //return true to use normal spawn pool finding code, false to use custom code
                if (spawnInfo.Player.ZoneTowerSolar || spawnInfo.Player.ZoneTowerStardust || spawnInfo.Player.ZoneTowerNebula || spawnInfo.Player.ZoneTowerVortex)
                {
                    if (modNPC.Mod == Mod)
                    {
                        return false;
                    }
                }
                else
                {
                    if (spawnInfo.Player.InModBiome(GetInstance<HallowInvasion>()))
                    {
                        //purge invalid rapture spawns
                        if (!HallowInvasion.ValidNPC(modNPC.Type))
                        {
                            return false;
                        }
                    }
                    else if (spawnInfo.Player.InModBiome(GetInstance<WorldEvilInvasion>()))
                    {
                        //purge invalid world evil enemy spawns
                        if (!WorldEvilInvasion.ValidNPC(modNPC.Type))
                        {
                            return false;
                        }
                    }

                    if (lavaSpawnFlag)
                    {
                        //purge invalid lava spawns
                        if (!canSpawnInLava.Contains(modNPC.Type))
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            c.Emit(OpCodes.Brfalse, label);

            //replace vanilla spawns with null if in lava
            if (!c.TryGotoNext(MoveType.Before,
                i => i.MatchLdloc(12),
                i => i.MatchRet()
                ))
            {
                GetInstance<Polarities>().Logger.Debug("Failed to find patch location 3");
                return;
            }

            c.Index++;

            c.EmitDelegate<Func<int?, int?>>((type) =>
            {
                return type != 0 ? type :
                    lavaSpawnFlag ? null : 0;
            });
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

            contagunPhages = 0;
            tentacleClubs = 0;
            chlorophyteDarts = 0;
            boneShards = 0;

            desiccation = 0;
            incineration = 0;
            coneVenom = false;

            whipTagDamage = 0;

            if (!spiritBite)
            {
                spiritBiteLevel = 0;
            }
            spiritBite = false;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.IsTypeSummon())
            {
                //damage += spiritBiteLevel;

                //TODO: This is inconsistent with vanilla whip tag damage, there will apparently be a better hook for this
                //damage += whipTagDamage;
            }
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

