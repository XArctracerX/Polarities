using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Polarities.Global;
using Polarities.Core;
using Polarities.Content.Biomes;
using Polarities.Content.Events;
using Polarities.Content.Items.Consumables.Summons.Hardmode;
using Polarities.Content.Items.Weapons.Ranged.Atlatls.Hardmode;
using Polarities.Content.Items.Weapons.Ranged.Flawless;
using Polarities.Content.Items.Weapons.Melee.Flawless;
using Polarities.Content.Items.Weapons.Magic.Flawless;
using Polarities.Content.Items.Weapons.Summon.Flawless;
using Polarities.Content.Items.Armor.Flawless.MechaMayhemArmor;
using Polarities.Content.Items.Accessories.Movement.Hardmode;
using Polarities.Content.NPCs.TownNPCs.PreHardmode;
using Polarities.Content.Items.Accessories.Combat.Offense.Hardmode;
using Polarities.Content.Items.Weapons.Melee.Boomerangs.Hardmode;
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
        public bool phagocytes;

        public int desiccation;
        public int incineration;
        public bool coneVenom;

        public bool spiritBite;
        public int spiritBiteLevel;

        //vanilla NPC.takenDamageMultiplier doesn't have any effect when less than 1, so we do this instead
        public float neutralTakenDamageMultiplier = 1f;

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
            neutralTakenDamageMultiplier = 1f;

            List<int> removeKeys = new List<int>();
            foreach (int i in hammerTimes.Keys)
            {
                hammerTimes[i]--;
                if (hammerTimes[i] <= 0)
                {
                    removeKeys.Add(i);
                }
            }
            foreach (int i in removeKeys)
            {
                hammerTimes.Remove(i);
            }

            contagunPhages = 0;
            tentacleClubs = 0;
            chlorophyteDarts = 0;
            boneShards = 0;
            phagocytes = false;

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
                if (tentacleClubs > 0)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    int amountLoss = tentacleClubs * 10;
                    npc.lifeRegen -= amountLoss * 2;
                    if (damage < amountLoss)
                    {
                        damage = amountLoss;
                    }
                }
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
                if (contagunPhages > 0)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    int amountLoss = contagunPhages * 10;
                    npc.lifeRegen -= amountLoss * 2;
                    if (damage < amountLoss)
                    {
                        damage = amountLoss;
                    }
                }
                if (phagocytes)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    int phagocyteCount = 0;
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.type == ProjectileType<PhagocyteProjectile>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI)
                        {
                            phagocyteCount++;
                        }
                    }
                    npc.lifeRegen -= phagocyteCount * phagocyteCount * 2;
                    if (damage < phagocyteCount * phagocyteCount)
                    {
                        damage = phagocyteCount * phagocyteCount;
                    }
                }
                if (boneShards > 0)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    int amountLoss = boneShards * 2;
                    npc.lifeRegen -= amountLoss * 2;
                    if (damage < amountLoss)
                    {
                        damage = amountLoss;
                    }
                }
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
            if (incineration > 0)
            {
                npc.lifeRegen -= incineration * 2;
                if (damage < incineration / 6)
                {
                    damage = incineration / 6;
                }
            }

            if (contagunPhages > 10)
            {
                SoundEngine.PlaySound(SoundID.Item17, npc.Center);
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ProjectileType<ContagunVirusProjectile>() && Main.projectile[i].ai[0] == npc.whoAmI + 1)
                    {
                        Main.projectile[i].ai[0] = 0;
                        Main.projectile[i].velocity = new Vector2(10, 0).RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.5f, 2f);

                        Projectile.NewProjectile(Main.projectile[i].GetSource_FromAI(), Main.projectile[i].Center, new Vector2(Main.rand.NextFloat(5f)).RotatedByRandom(MathHelper.TwoPi), ProjectileType<ContagunProjectile>(), Main.projectile[i].damage, Main.projectile[i].knockBack, Main.projectile[i].owner, ai0: Main.rand.NextFloat(MathHelper.TwoPi), ai1: 240f);
                    }
                }
            }

            //UpdateCustomSoulDrain(npc);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Polarities.customNPCGlowMasks.ContainsKey(npc.type))
            {
                float num246 = Main.NPCAddHeight(npc);
                SpriteEffects spriteEffects = 0;
                if (npc.spriteDirection == 1)
                {
                    spriteEffects = (SpriteEffects)1;
                }
                Vector2 halfSize = new Vector2(Polarities.customNPCGlowMasks[npc.type].Width() / 2, Polarities.customNPCGlowMasks[npc.type].Height() / Main.npcFrameCount[npc.type] / 2);

                Color color = npc.GetAlpha(npc.GetNPCColorTintedByBuffs(Color.White));

                spriteBatch.Draw(Polarities.customNPCGlowMasks[npc.type].Value, npc.Bottom - screenPos + new Vector2(-Polarities.customNPCGlowMasks[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, -Polarities.customNPCGlowMasks[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num246 + npc.gfxOffY), (Rectangle?)npc.frame, color, npc.rotation, halfSize, npc.scale, spriteEffects, 0f);
            }
        }

        public override bool CanHitNPC(NPC npc, NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            if (target.type == NPCType<Ghostwriter>() && !(npc.type == NPCID.Wraith || npc.type == NPCID.Ghost || npc.type == NPCID.Reaper || npc.type == NPCID.Poltergeist || npc.type == NPCID.DungeonSpirit))
            {
                return false;
            }
            return true;
        }

        public override void BuffTownNPC(ref float damageMult, ref int defense)
        {
            if (PolaritiesSystem.downedStormCloudfish)
            {
                damageMult += 0.1f;
                defense += 3;
            }
            if (PolaritiesSystem.downedStarConstruct)
            {
                damageMult += 0.1f;
                defense += 3;
            }
            if (PolaritiesSystem.downedGigabat)
            {
                damageMult += 0.1f;
                defense += 3;
            }
            if (PolaritiesSystem.downedRiftDenizen)
            {
                damageMult += 0.1f;
                defense += 3;
            }

            if (PolaritiesSystem.downedSunPixie)
            {
                damageMult += 0.15f;
                defense += 6;
            }
            if (PolaritiesSystem.downedEsophage)
            {
                damageMult += 0.15f;
                defense += 6;
            }
            if (PolaritiesSystem.downedSelfsimilarSentinel)
            {
                damageMult += 0.15f;
                defense += 6;
            }
            if (PolaritiesSystem.downedEclipxie)
            {
                damageMult += 0.15f;
                defense += 6;
            }
            if (PolaritiesSystem.downedHemorrphage)
            {
                damageMult += 0.15f;
                defense += 6;
            }

            if (PolaritiesSystem.downedPolarities)
            {
                damageMult += 0.15f;
                defense += 10;
            }
            if (NPC.downedMoonlord)
            {
                damageMult += 0.15f;
                defense += 10;
            }
        }

        public override void OnKill(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsTail:
                    if (npc.boss)
                    {
                        if (!PolaritiesSystem.downedEaterOfWorlds)
                        {
                            PolaritiesSystem.downedEaterOfWorlds = true;
                        }
                    }
                    break;
                case NPCID.BrainofCthulhu:
                    if (!PolaritiesSystem.downedBrainOfCthulhu)
                    {
                        PolaritiesSystem.downedBrainOfCthulhu = true;
                    }
                    break;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (HallowInvasion.ValidNPC(npc.type))
            {
                npcLoot.Add(ItemDropRule.ByCondition(new SunPixieSummonItemDropCondition(), ItemType<SunPixieSummonItem>()));
            }
            if (WorldEvilInvasion.ValidNPC(npc.type))
            {
                npcLoot.Add(ItemDropRule.ByCondition(new EsophageSummonItemDropCondition(), ItemType<EsophageSummonItem>()));
            }

            if (customSlimes.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.SlimeStaff, 10000, 7000));
            }

            switch (npc.type)
            {
                case NPCID.GraniteFlyer:
                case NPCID.GraniteGolem:
                    //TODO: npcLoot.Add(ItemDropRule.Common(ItemType<BlueQuartz>(), 2, 1, 2));
                    break;

                //bosses (mostly flawless stuff)
                case NPCID.KingSlime:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<Gelthrower>()));
                    break;
                case NPCID.EyeofCthulhu:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<Eyeruption>()));
                    break;
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsTail:
                    {
                        LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.LegacyHack_IsABoss());
                        leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<ConsumptionCannon>()));
                        npcLoot.Add(leadingConditionRule);
                    }
                    break;
                case NPCID.BrainofCthulhu:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<NeuralBasher>()));
                    break;
                case NPCID.QueenBee:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<RoyalOrb>()));
                    break;
                case NPCID.SkeletronHead:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<BonyBackhand>()));
                    break;
                case NPCID.WallofFlesh:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<MawOfFlesh>()));
                    break;
                case NPCID.TheDestroyer:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<FlawlessMechTail>()));
                    break;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    {
                        LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
                        leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<FlawlessMechMask>()));
                        npcLoot.Add(leadingConditionRule);
                    }
                    break;
                case NPCID.SkeletronPrime:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<FlawlessMechChestplate>()));
                    break;
                case NPCID.Plantera:
                    {
                        npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<UnfoldingBlossom>()));

                        LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
                        leadingConditionRule.OnSuccess(ItemDropRule.Common(ItemType<JunglesRage>(), 4));
                    }
                    break;
                case NPCID.Everscream:
                    {
                        //adds the candy cane atlatl
                        List<IItemDropRule> everscreamLoot = npcLoot.Get(includeGlobalDrops: false);
                        for (int index = 0; index < everscreamLoot.Count; index++)
                        {
                            IItemDropRule rule = everscreamLoot[index];
                            if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.FrostMoonDropGatingChance)
                            {
                                foreach (IItemDropRuleChainAttempt tryAttempt in rule.ChainedRules)
                                {
                                    if (tryAttempt is TryIfSucceeded chain && chain.RuleToChain is CommonDrop rule2)
                                    {
                                        foreach (IItemDropRuleChainAttempt tryAttempt2 in rule2.ChainedRules)
                                        {
                                            if (tryAttempt2 is TryIfFailedRandomRoll chain2 && chain2.RuleToChain is OneFromOptionsDropRule rule3)
                                            {
                                                Array.Resize(ref rule3.dropIds, rule3.dropIds.Length + 1);
                                                rule3.dropIds[rule3.dropIds.Length - 1] = ItemType<CandyCaneAtlatl>();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case NPCID.DD2Betsy:
                    npcLoot.Add(ItemDropRule.ByCondition(new FlawlessDropCondition(), ItemType<WyvernsNest>()));
                    break;
            }

            //replace trophies and master pets
            List<IItemDropRule> originalLoot = npcLoot.Get(includeGlobalDrops: false);
            for (int index = 0; index < originalLoot.Count; index++)
            {
                IItemDropRule rule = originalLoot[index];

                //why can we not just insert things
                void RuleReplace(IItemDropRule newRule)
                {
                    npcLoot.Add(newRule);
                    npcLoot.Remove(rule);

                    List<IItemDropRule> currentLoot = npcLoot.Get(includeGlobalDrops: false);

                    for (int i = index; i < currentLoot.Count - 1; i++)
                    {
                        npcLoot.Remove(currentLoot[i]);
                        npcLoot.Add(currentLoot[i]);
                    }
                }

                void ChainedRuleReplace(IItemDropRule baseRule, IItemDropRuleChainAttempt oldTryAttempt, IItemDropRule newRule)
                {
                    baseRule.ChainedRules.Remove(oldTryAttempt);
                    baseRule.OnSuccess(newRule);

                    IItemDropRuleChainAttempt[] currentLoot = new IItemDropRuleChainAttempt[baseRule.ChainedRules.Count];
                    baseRule.ChainedRules.CopyTo(currentLoot);

                    for (int i = index; i < currentLoot.Length - 1; i++)
                    {
                        baseRule.ChainedRules.Remove(currentLoot[i]);
                        baseRule.ChainedRules.Add(currentLoot[i]);
                    }
                }

                //in vanilla, Conditions.LegacyHack_IsABoss is used only for trophies and for the eater of worlds
                if (rule is ItemDropWithConditionRule trophyDrop && trophyDrop.condition is Conditions.LegacyHack_IsABoss && trophyDrop.chanceDenominator == 10 && trophyDrop.chanceNumerator == 1 && trophyDrop.amountDroppedMinimum == 1 && trophyDrop.amountDroppedMaximum == 1)
                {
                    //replace with a better trophy rule
                    RuleReplace(new FlawlessOrRandomDropRule(trophyDrop.itemId, 10, 1, 1, 1, new Conditions.LegacyHack_IsABoss()));
                }
                //in vanilla, DropBasedOnMasterMode is only used for master mode pets
                //This does not work for eater of worlds, the twins, and the pumpkin/frost moons, as their master mode pet drops have conditions, so we handle those separately
                else if (rule is DropBasedOnMasterMode masterPetDrop && masterPetDrop.ruleForDefault is DropNothing && masterPetDrop.ruleForMasterMode is DropPerPlayerOnThePlayer perPlayerDrop && perPlayerDrop.chanceDenominator == 4 && perPlayerDrop.chanceNumerator == 1 && perPlayerDrop.amountDroppedMaximum == 1 && perPlayerDrop.amountDroppedMinimum == 1)
                {
                    //replace with a better master pet rule
                    RuleReplace(ModUtils.MasterModeDropOnAllPlayersOrFlawless(perPlayerDrop.itemId, 4, 1, 1, 1));
                }
                //EoW master mode pet
                else if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail)
                {
                    if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.LegacyHack_IsABoss)
                    {
                        List<IItemDropRuleChainAttempt> replaceRules = new List<IItemDropRuleChainAttempt>();
                        List<IItemDropRule> newRules = new List<IItemDropRule>();
                        foreach (IItemDropRuleChainAttempt tryAttempt in rule.ChainedRules)
                        {
                            if (tryAttempt is TryIfSucceeded rule2 && rule2.RuleToChain is DropBasedOnMasterMode masterPetDrop2 && masterPetDrop2.ruleForDefault is DropNothing && masterPetDrop2.ruleForMasterMode is DropPerPlayerOnThePlayer perPlayerDrop2 && perPlayerDrop2.chanceDenominator == 4 && perPlayerDrop2.chanceNumerator == 1 && perPlayerDrop2.amountDroppedMaximum == 1 && perPlayerDrop2.amountDroppedMinimum == 1)
                            {
                                //replace with a better master pet rule
                                newRules.Add(ModUtils.MasterModeDropOnAllPlayersOrFlawless(perPlayerDrop2.itemId, 4, 1, 1, 1));
                                replaceRules.Add(tryAttempt);
                            }
                        }
                        for (int i = 0; i < replaceRules.Count; i++)
                        {
                            ChainedRuleReplace(rule, replaceRules[i], newRules[i]);
                        }
                    }
                }
                //Twins master mode pet
                else if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
                {
                    if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.MissingTwin)
                    {
                        List<IItemDropRuleChainAttempt> replaceRules = new List<IItemDropRuleChainAttempt>();
                        List<IItemDropRule> newRules = new List<IItemDropRule>();
                        foreach (IItemDropRuleChainAttempt tryAttempt in rule.ChainedRules)
                        {
                            if (tryAttempt is TryIfSucceeded rule2 && rule2.RuleToChain is DropBasedOnMasterMode masterPetDrop2 && masterPetDrop2.ruleForDefault is DropNothing && masterPetDrop2.ruleForMasterMode is DropPerPlayerOnThePlayer perPlayerDrop2 && perPlayerDrop2.chanceDenominator == 4 && perPlayerDrop2.chanceNumerator == 1 && perPlayerDrop2.amountDroppedMaximum == 1 && perPlayerDrop2.amountDroppedMinimum == 1)
                            {
                                //replace with a better master pet rule
                                newRules.Add(ModUtils.MasterModeDropOnAllPlayersOrFlawless(perPlayerDrop2.itemId, 4, 1, 1, 1));
                                replaceRules.Add(tryAttempt);
                            }
                        }
                        for (int i = 0; i < replaceRules.Count; i++)
                        {
                            ChainedRuleReplace(rule, replaceRules[i], newRules[i]);
                        }
                    }
                }
                //Pumpkin moon master mode pet
                else if (npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking)
                {
                    if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.PumpkinMoonDropGatingChance)
                    {
                        List<IItemDropRuleChainAttempt> replaceRules = new List<IItemDropRuleChainAttempt>();
                        List<IItemDropRule> newRules = new List<IItemDropRule>();
                        foreach (IItemDropRuleChainAttempt tryAttempt in rule.ChainedRules)
                        {
                            if (tryAttempt is TryIfSucceeded rule2 && rule2.RuleToChain is DropBasedOnMasterMode masterPetDrop2 && masterPetDrop2.ruleForDefault is DropNothing && masterPetDrop2.ruleForMasterMode is DropPerPlayerOnThePlayer perPlayerDrop2 && perPlayerDrop2.chanceDenominator == 4 && perPlayerDrop2.chanceNumerator == 1 && perPlayerDrop2.amountDroppedMaximum == 1 && perPlayerDrop2.amountDroppedMinimum == 1)
                            {
                                //replace with a better master pet rule
                                newRules.Add(ModUtils.MasterModeDropOnAllPlayersOrFlawless(perPlayerDrop2.itemId, 4, 1, 1, 1));
                                replaceRules.Add(tryAttempt);
                            }
                        }
                        for (int i = 0; i < replaceRules.Count; i++)
                        {
                            ChainedRuleReplace(rule, replaceRules[i], newRules[i]);
                        }
                    }
                }
                //Frost moon master mode pet
                else if (npc.type == NPCID.Everscream || npc.type == NPCID.SantaNK1 || npc.type == NPCID.IceQueen)
                {
                    if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.FrostMoonDropGatingChance)
                    {
                        List<IItemDropRuleChainAttempt> replaceRules = new List<IItemDropRuleChainAttempt>();
                        List<IItemDropRule> newRules = new List<IItemDropRule>();
                        foreach (IItemDropRuleChainAttempt tryAttempt in rule.ChainedRules)
                        {
                            if (tryAttempt is TryIfSucceeded rule2 && rule2.RuleToChain is DropBasedOnMasterMode masterPetDrop2 && masterPetDrop2.ruleForDefault is DropNothing && masterPetDrop2.ruleForMasterMode is DropPerPlayerOnThePlayer perPlayerDrop2 && perPlayerDrop2.chanceDenominator == 4 && perPlayerDrop2.chanceNumerator == 1 && perPlayerDrop2.amountDroppedMaximum == 1 && perPlayerDrop2.amountDroppedMinimum == 1)
                            {
                                //replace with a better master pet rule
                                newRules.Add(ModUtils.MasterModeDropOnAllPlayersOrFlawless(perPlayerDrop2.itemId, 4, 1, 1, 1));
                                replaceRules.Add(tryAttempt);
                            }
                        }
                        for (int i = 0; i < replaceRules.Count; i++)
                        {
                            ChainedRuleReplace(rule, replaceRules[i], newRules[i]);
                        }
                    }
                }
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            //they updated something about damage so this never works now
            //damage *= neutralTakenDamageMultiplier;
            //return true;
        }
    }
}

