using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Buffs;
using Polarities.Content.Buffs.PreHardmode;
using Polarities.Content.Buffs.Hardmode;
using Polarities.Content.Items.Tools.Books;
using Polarities.Content.Items.Tools.Books.PreHardmode;
using Polarities.Content.Items.Tools.Books.Hardmode;
using Polarities.Content.Items.Armor.Summon.PreHardmode.Stormcloud;
using Polarities.Content.Items.Weapons.Ranged.Guns.Hardmode;
using Polarities.Content.Items.Accessories.Combat.Offense.PreHardmode;
using Polarities.Content.Items.Accessories.Combat.Offense.Hardmode;
using Polarities.Content.Items.Vanity.DevSets.BubbySet;
using Polarities.Content.Items.Vanity.DevSets.TuringSet;
using Polarities.Content.Items.Vanity.DevSets.ElectroManiacSet;
using Polarities.Content.NPCs.Bosses.Hardmode.ConvectiveWanderer;
using Polarities.Content.Items.Accessories.ExpertMode.Hardmode;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities
{
	public partial class PolaritiesPlayer : ModPlayer
	{
        public int warhammerDefenseBoost = 0;
        public int warhammerTimeBoost = 0;

        //public Dictionary<float, float> screenShakes = new Dictionary<float, float>(); //key is time at which the screenshake ends, value is magnitude
        //private int screenshakeRandomSeed;

        public int screenshakeTimer;
        public int screenshakeMagnitude;

        public float hookSpeedMult;
        public float manaStarMultiplier;
        public float orbMinionSlots;
        public int royalOrbHitCount;
        public Vector2 velocityMultiplier;
        public bool strangeObituary;
        public float usedBookSlots;
        public float maxBookSlots;
        public bool canJumpAgain_Sail_Extra;
        public bool jumpAgain_Sail_Extra;
        public bool hasGlide;
        public int skeletronBookCooldown;
        public int beeRingTimer;
        public bool stormcore;
        public bool stargelAmulet;
        public bool hopperCrystal;
        public bool limestoneShield;
        public int limestoneShieldCooldown;
        public bool limestoneSetBonus;
        public int limestoneSetBonusHitCooldown;
        public bool skeletronBook;
        public int moonLordLifestealCooldown;
        public int wingTimeBoost;
        public float critDamageBoostMultiplier;
        public int ignoreCritDefenseAmount;
        public bool snakescaleSetBonus;
        public int desiccation;
        public int incineration;
        public int incinerationResistanceTime;
        public bool coneVenom;
        public float runSpeedBoost;
        public float spawnRate;
        public bool solarEnergizer;
        public int wyvernsNestDamage;
        public bool wormScarf;
        public Vector3 light;
        public DamageClass convectiveSetBonusType;
        public int convectiveSetBonusCharge;
        public StatModifier dartDamage;
        public bool justHit;
        public float candyCaneAtlatlBoost;
        public StatModifier nonMagicDamage;
        public bool hydraHide;
        public float hydraHideTime = 0;
        public bool convectiveDash;
        public int convectiveDashCharge;
        public Vector2 convectiveDashVelocity = Vector2.Zero;
        public bool convectiveDashing;
        public int convectiveDashStartTime;
        public bool stormcloudArmor;
        public int stormcloudArmorCooldown;
        public int tolerancePotionDelayTime = 3600;

        //direction of dash
        public int dashDir;
        //index of dash in Dash.dashes
        public int dashIndex;
        //time left until next dash
        public int dashDelay;
        //time left in dash
        public int dashTimer;

        //dash directions
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public int itemHitCooldown = 0;


        private const int trailCacheLength = 20;
        public Vector2[] oldCenters = new Vector2[trailCacheLength];
        public Vector2[] oldVelocities = new Vector2[trailCacheLength];
        public int[] oldDirections = new int[trailCacheLength];

        public int bubbyWingFrameCounter;
        public int bubbyWingFrame;

        //public override void UpdateDead()
        //{
        //fractalization = 0;
        //}

        public override void ResetEffects()
		{
            //update old positions, velocities, etc.
            for (int i = trailCacheLength - 1; i > 0; i--)
            {
                oldCenters[i] = oldCenters[i - 1];
                oldVelocities[i] = oldVelocities[i - 1];
                oldDirections[i] = oldDirections[i - 1];
            }
            oldCenters[0] = Player.Center;
            oldVelocities[0] = Player.velocity;
            oldDirections[0] = Player.direction;

            //reset a bunch of values
            warhammerDefenseBoost = 0;
            warhammerTimeBoost = 0;
            hookSpeedMult = 1f;
            manaStarMultiplier = 1f;
            orbMinionSlots = 1f;
            strangeObituary = false;
            usedBookSlots = 0f;
            hasGlide = false;
            stormcore = false;
            stargelAmulet = false;
            hopperCrystal = false;
            limestoneShield = false;
            limestoneSetBonus = false;
            skeletronBook = false;
            wingTimeBoost = 0;
            critDamageBoostMultiplier = 1f;
            ignoreCritDefenseAmount = 0;
            snakescaleSetBonus = false;
            desiccation = 0;
            incineration = 0;
            incinerationResistanceTime = 0;
            coneVenom = false;
            runSpeedBoost = 1f;
            spawnRate = 1f;
            solarEnergizer = false;
            wormScarf = false;
            wyvernsNestDamage = 0;
            light = Vector3.Zero;
            convectiveSetBonusType = null;
            dartDamage = StatModifier.Default;
            justHit = false;
            nonMagicDamage = StatModifier.Default;
            hydraHide = false;
            convectiveDash = false;
            stormcloudArmor = false;

            if (skeletronBookCooldown > 0) skeletronBookCooldown--;
            if (beeRingTimer > 0) beeRingTimer--;
            if (limestoneShieldCooldown > 0) limestoneShieldCooldown--;
            if (limestoneSetBonusHitCooldown > 0) limestoneSetBonusHitCooldown--;
            if (moonLordLifestealCooldown > 0) moonLordLifestealCooldown--;
            if (candyCaneAtlatlBoost > 0) candyCaneAtlatlBoost--;
            if (stormcloudArmorCooldown > 0) stormcloudArmorCooldown--;
            if (hydraHideTime > 0)
            {
                hydraHideTime--;
                Player.lifeRegenTime = 120;
            }

            //screenshakeRandomSeed = Main.rand.Next();

            dashIndex = 0;
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                dashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                dashDir = DashLeft;
            }
            else
            {
                dashDir = -1;
            }

            if (itemHitCooldown > 0) itemHitCooldown--;

            Player.velocity /= velocityMultiplier;
            velocityMultiplier = Vector2.One;
        }

        //this is reset here to ensure books update properly
        public override void PostUpdateBuffs()
        {
            maxBookSlots = 1f;
        }


        public override void PostUpdateEquips()
        {
            if (Player.HasBuff<GolemBookBuff>() && Player.wingsLogic == 0)
            {
                Player.noFallDmg = true;
                Player.jumpSpeedBoost += 16;
                Player.statDefense += 6;
            }

            canJumpAgain_Sail_Extra = false;
            if (Player.HasBuff(BuffType<KingSlimeBookBuff>()))
            {
                if (Player.GetJumpState(ExtraJump.TsunamiInABottle).Enabled) { canJumpAgain_Sail_Extra = true; }
                Player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            }

            if (stargelAmulet)
            {
                float amountOfDay;
                if (Main.dayTime)
                {
                    amountOfDay = 1f - (float)Math.Abs(Main.time - Main.dayLength / 2) / (float)Main.dayLength;
                }
                else
                {
                    amountOfDay = (float)Math.Abs(Main.time - Main.nightLength / 2) / (float)Main.nightLength;
                }
                Player.GetDamage(DamageClass.Generic) += 0.12f * amountOfDay;
                Player.endurance *= 1 - 0.1f * (1 - amountOfDay);
            }

            //wing time boost
            Player.wingTimeMax += wingTimeBoost;

            //run speed boost
            Player.maxRunSpeed *= runSpeedBoost;
            Player.accRunSpeed *= runSpeedBoost;

            //custom slimes
            foreach (int i in PolaritiesNPC.customSlimes)
            {
                Player.npcTypeNoAggro[i] = Player.npcTypeNoAggro[NPCID.BlueSlime];
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!Player.mount.Active)
            {
                if (PlayerInput.Triggers.Current.Jump && hasGlide)
                {
                    Player.maxFallSpeed = 1f;
                }
            }

            //target via orbs
            if (PlayerInput.Triggers.JustPressed.MouseRight && (Player.HeldItem.DamageType == DamageClass.Summon || Player.HeldItem.DamageType.GetEffectInheritance(DamageClass.Summon)) && Player.channel)
            {
                Player.MinionNPCTargetAim(false);
            }

            if (stargelAmulet)
            {
                float amountOfDay;
                if (Main.dayTime)
                {
                    amountOfDay = 1f - (float)Math.Abs(Main.time - Main.dayLength / 2) / (float)Main.dayLength;
                }
                else
                {
                    amountOfDay = (float)Math.Abs(Main.time - Main.nightLength / 2) / (float)Main.nightLength;
                }
                Player.GetDamage(DamageClass.Generic) += 0.12f * amountOfDay;
                Player.endurance *= 1 - 0.1f * (1 - amountOfDay);
            }

            //convective dash
            if (convectiveDash)
            {
                if (Polarities.ConvectiveDashHotkey.JustPressed && convectiveDashCharge > 60 && !Player.mount.Active)
                {
                    //start dash
                    convectiveDashCharge -= convectiveDashCharge % 60; //return to last charge level

                    convectiveDashing = true;
                    convectiveDashVelocity = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 24;
                    convectiveDashStartTime = PolaritiesSystem.timer;

                    SoundEngine.PlaySound(SoundID.NPCDeath14, Player.Center);

                    for (int i = 0; i < 24; i++)
                    {
                        Vector2 particleSpawnPos = Player.Center + new Vector2(Main.rand.NextFloat(24), 0).RotatedByRandom(MathHelper.TwoPi);
                        Vector2 particleVelocity = new Vector2(Main.rand.NextFloat(6, 20), 0).RotatedByRandom(MathHelper.TwoPi) - convectiveDashVelocity;
                        ConvectiveWandererVortexParticle particle = Particle.NewParticle<ConvectiveWandererVortexParticle>(particleSpawnPos, particleVelocity, 0f, 0f, Scale: Main.rand.NextFloat(0.1f, 0.2f), Color: ModUtils.ConvectiveFlameColor(Main.rand.NextFloat(0.2f, 0.4f)));
                        ParticleLayer.AfterLiquidsAdditive.Add(particle);
                    }
                }
                else if (convectiveDashing && (!Polarities.ConvectiveDashHotkey.Current || Player.mount.Active))
                {
                    convectiveDashCharge = 0;
                    convectiveDashing = false;

                    Player.velocity *= 0.25f;
                }

                if (convectiveDashing)
                {
                    convectiveDashCharge -= 4;

                    if (convectiveDashCharge < 0)
                    {
                        convectiveDashCharge = 0;
                        convectiveDashing = false;

                        Player.velocity *= 0.25f;
                    }
                    else
                    {
                        Player.velocity = convectiveDashVelocity;
                        Player.ChangeDir(convectiveDashVelocity.X > 0 ? 1 : -1);

                        //prevent other movement things from interfering
                        Player.maxFallSpeed = convectiveDashVelocity.Length();
                        Player.controlDown = false;
                        Player.controlJump = false;
                        Player.controlLeft = false;
                        Player.controlRight = false;
                        Player.controlUp = false;

                        Vector2 particleSpawnPos = Player.Center + new Vector2(Main.rand.NextFloat(24), 0).RotatedByRandom(MathHelper.TwoPi);
                        Vector2 particleVelocity = -convectiveDashVelocity.RotatedByRandom(0.1f);
                        ConvectiveWandererVortexParticle particle = Particle.NewParticle<ConvectiveWandererVortexParticle>(particleSpawnPos, particleVelocity, 0f, 0f, Scale: Main.rand.NextFloat(0.1f, 0.2f), Color: ModUtils.ConvectiveFlameColor(Main.rand.NextFloat(0.2f, 0.4f)));
                        ParticleLayer.AfterLiquidsAdditive.Add(particle);

                        //ramming! (based on EoC)
                        Rectangle rectangle = new Rectangle((int)(Player.position.X + Player.velocity.X * 0.5 - 4.0), (int)(Player.position.Y + Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                        for (int i = 0; i < 200; i++)
                        {
                            NPC nPC = Main.npc[i];
                            if (!nPC.active || nPC.dontTakeDamage || nPC.friendly || nPC.aiStyle == 112 && !(nPC.ai[2] <= 1f) || !Player.CanNPCBeHitByPlayerOrPlayerProjectile(nPC))
                            {
                                continue;
                            }
                        }
                    }
                }
                else if (convectiveDashCharge < 240)
                {
                    if (convectiveDashCharge % 60 == 0)
                    {
                        float progress = convectiveDashCharge / 240f;
                        BuildingEruptionChargingParticle particle = Particle.NewParticle<BuildingEruptionChargingParticle>(Vector2.Zero, Vector2.Zero, 0f, 0f, Scale: 1f, Color: ModUtils.ConvectiveFlameColor(progress * progress * 0.5f));
                        particle.playerOwner = Player.whoAmI;
                        ParticleLayer.BeforePlayersAdditive.Add(particle);
                    }

                    convectiveDashCharge++;

                    if (convectiveDashCharge % 60 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item114.WithPitchOffset((convectiveDashCharge - 60) / 180f - 0.5f), Player.Center);
                    }
                }
            }
            else
            {
                if (convectiveDashing)
                {
                    Player.velocity *= 0.25f;
                    convectiveDashing = false;
                }

                if (!convectiveDash) convectiveDashCharge = 0;
            }
        }

        public override void PostUpdate()
		{
            if (hopperCrystal && Player.justJumped)
            {
                Player.velocity.X = (Player.velocity.X > 0 ? 1 : -1) * Math.Min(2 * Player.maxRunSpeed, Math.Abs(2 * Player.velocity.X));
            }

            if (stormcore && 0.2f + Player.slotsMinions <= Player.maxMinions && Main.rand.NextBool(60))
			{
				Main.projectile[Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X + 500 * (2 * (float)Main.rand.NextDouble() - 1), Player.Center.Y - 500, 0, 0, ProjectileType<StormcoreMinion>(), 1, Player.GetTotalKnockback(DamageClass.Summon).ApplyTo(0.5f), Player.whoAmI, 0, 0)].originalDamage = 1;
			}

            if (wyvernsNestDamage > 0)
            {
                //sentries don't despawn while using the wyvern's nest
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].sentry)
                    {
                        Main.projectile[i].timeLeft = Projectile.SentryLifeTime;
                    }
                }
                for (int i = Player.ownedProjectileCounts[ProjectileType<WyvernsNestMinion>()]; i < Player.maxTurrets; i++)
                {
                    Main.projectile[Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ProjectileType<WyvernsNestMinion>(), wyvernsNestDamage, Player.GetTotalKnockback(DamageClass.Summon).ApplyTo(2f), Player.whoAmI, 0, 0)].originalDamage = 20;
                }
            }

            if (stormcloudArmor && stormcloudArmorCooldown <= 0 && Player.ownedProjectileCounts[ProjectileType<StormcloudArmorRaincloud>()] < 8)
            {
                stormcloudArmorCooldown = 240;

                float distance = 750f;
                bool isTarget = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable)
                    {
                        Vector2 newMove = Main.npc[k].Center - Player.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            targetID = k;
                            distance = distanceTo;
                            isTarget = true;
                        }
                    }
                }

                if (isTarget)
                {
                    NPC target = Main.npc[targetID];
                    Projectile.NewProjectile(Player.GetSource_FromAI(), new Vector2(target.Center.X, target.position.Y - 128), Vector2.Zero, ProjectileType<StormcloudArmorRaincloud>(), (int)Player.GetTotalDamage(DamageClass.Summon).ApplyTo(8), 0, Player.whoAmI);
                }
            }

            if (canJumpAgain_Sail_Extra)
            {
                if (Player.justJumped || Player.controlHook || Player.velocity.Y == 0f)
                {
                    jumpAgain_Sail_Extra = true;
                }
                if (!Player.GetJumpState(ExtraJump.TsunamiInABottle).Available && jumpAgain_Sail_Extra)
                {
                    jumpAgain_Sail_Extra = false;
                    Player.GetJumpState(ExtraJump.TsunamiInABottle).Available = true;
                }
            }

            if (Player.HasBuff(BuffType<QueenBeeBookBuff>()) && Player.ownedProjectileCounts[ProjectileType<QueenBeeBookBee>()] + Player.ownedProjectileCounts[ProjectileType<QueenBeeBookBeeLarge>()] < 6 && beeRingTimer == 0)
            {
                int buffIndex = Player.FindBuffIndex(BuffType<QueenBeeBookBuff>());
                if (Player.strongBees && Main.rand.NextBool())
                {
                    Projectile.NewProjectile(Player.GetSource_Buff(buffIndex), Main.MouseWorld, Vector2.Zero, ProjectileType<QueenBeeBookBeeLarge>(), 8, 0.5f, Player.whoAmI, Main.rand.NextFloat(0, 2 * MathHelper.Pi));
                }
                else
                {
                    Projectile.NewProjectile(Player.GetSource_Buff(buffIndex), Main.MouseWorld, Vector2.Zero, ProjectileType<QueenBeeBookBee>(), 5, 0, Player.whoAmI, Main.rand.NextFloat(0, 2 * MathHelper.Pi));
                }
                beeRingTimer = 5;
            }

            //update bubby vanity wing frames
            if (Player.velocity.Y == 0)
            {
                bubbyWingFrame = 0;
                bubbyWingFrameCounter = 1;
            }
            else
            {
                bubbyWingFrameCounter++;
                if (bubbyWingFrameCounter == 2)
                {
                    bubbyWingFrameCounter = 0;
                    bubbyWingFrame++;
                    if (bubbyWingFrame >= 7 && Player.velocity.Y < 0)
                    {
                        bubbyWingFrame = 1;
                    }
                    if ((bubbyWingFrame < 7 || bubbyWingFrame >= 15) && Player.velocity.Y > 0)
                    {
                        bubbyWingFrame = 7;
                    }
                }
            }

            Lighting.AddLight(Player.Center, light);
        }

        //public void AddScreenShake(float magnitude, float timeLeft)
        //{
        //if (magnitude > 0 && timeLeft > 0)
        //{
        //float endTime = timeLeft + PolaritiesSystem.timer;
        //if (screenShakes.ContainsKey(endTime))
        //{
        //screenShakes[endTime] += magnitude / timeLeft;
        //}
        //else
        //{
        //screenShakes.Add(endTime, magnitude / timeLeft);
        //}
        //}
        //}

        public override void ModifyScreenPosition()
        {
            screenshakeTimer--;
            if (screenshakeTimer > 0)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(screenshakeMagnitude * -1, screenshakeMagnitude + 1), Main.rand.Next(screenshakeMagnitude * -1, screenshakeMagnitude + 1));
            }
        }

        //public override void ModifyScreenPosition()
        //{
        //if (screenShakes.Keys.Count > 0)
        //{
        //List<float> removeTimesLeft = new List<float>();

        //Polarities.preGeneratedRand.SetIndex(screenshakeRandomSeed);
        //foreach (float timeLeft in screenShakes.Keys)
        //{
        //if (timeLeft <= PolaritiesSystem.timer)
        //{
        //removeTimesLeft.Add(timeLeft);
        //}
        //else
        //{
        //Main.screenPosition += new Vector2(Polarities.preGeneratedRand.NextNormallyDistributedFloat(screenShakes[timeLeft] * (timeLeft - PolaritiesSystem.timer)), 0).RotatedBy(Polarities.preGeneratedRand.NextFloat(MathHelper.TwoPi));
        //}
        //}
        //foreach (float timeLeft in removeTimesLeft) screenShakes.Remove(timeLeft);
        //}

        //to prevent jittering of some things
        //Main.screenPosition.X = (int)Main.screenPosition.X;
        //Main.screenPosition.Y = (int)Main.screenPosition.Y;
        //}

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            //OnHitNPCWithAnything(target, damage, knockback, crit, item.DamageType);

            if (target.GetGlobalNPC<PolaritiesNPC>().usesProjectileHitCooldowns)
            {
                itemHitCooldown = target.GetGlobalNPC<PolaritiesNPC>().projectileHitCooldownTime;
            }

            if (item.DamageType == DamageClass.Summon || item.DamageType.GetEffectInheritance(DamageClass.Summon))
            {
                royalOrbHitCount++;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            //OnHitNPCWithAnything(target, damage, knockback, crit, proj.DamageType);

            if (proj.IsTypeSummon())
            {
                royalOrbHitCount++;
            }

            if ((proj.sentry || ProjectileID.Sets.SentryShot[proj.type]) && Player.HasBuff(BuffType<BetsyBookBuff>()))
            {
                target.AddBuff(BuffID.OnFire, 300);
                target.AddBuff(BuffID.OnFire3, 300);
            }
        }

        public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, DamageClass damageClass)
        {
        //if (snakescaleSetBonus && crit)
        //{
        //target.AddBuff(BuffID.Venom, 5 * 60);
        //}

            if (moonLordLifestealCooldown == 0 && Player.HasBuff(BuffType<MoonLordBookBuff>()) && !Player.moonLeech)
            {
                float baseLifestealAmount = (float)Math.Log(damage * Math.Pow(Main.rand.NextFloat(1f), 4));
                if (baseLifestealAmount >= 1)
                {
                    moonLordLifestealCooldown = 10;
                    Player.statLife += (int)baseLifestealAmount;
                    Player.HealEffect((int)baseLifestealAmount);
                }
            }

            if (damageClass != DamageClass.Magic && !damageClass.GetEffectInheritance(DamageClass.Magic) && solarEnergizer)
            {
                Player.statMana++;
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            //TODO: (MAYBE) Replace with source propagation system once supported/if it doesn't end up being trivially supported, also move terraprisma to be obtained on any flawless run if/when this system is added
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                    Main.npc[i].GetGlobalNPC<PolaritiesNPC>().flawless = false;
            }

            if (strangeObituary)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValueWith("Mods.Polarities.DeathMessage.StrangeObituary", new { PlayerName = Player.name })), 1.0, 0, false);
                return;
            }

            if (Player.HasBuff(BuffType<EyeOfCthulhuBookBuff>()))
            {
                Projectile.NewProjectile(Player.GetSource_Buff(Player.FindBuffIndex(BuffType<EyeOfCthulhuBookBuff>())), Player.Center, new Vector2(4, 0).RotatedByRandom(2 * Math.PI), ProjectileType<EyeOfCthulhuBookEye>(), 12, 3, Player.whoAmI);
            }

            if (skeletronBook && skeletronBookCooldown == 0)
            {
                skeletronBookCooldown = 3 * 60 * 60;
            }

            if (limestoneShield && limestoneShieldCooldown == 0)
            {
                limestoneShieldCooldown = 60 * 30;
            }

            if (limestoneSetBonus)
            {
                limestoneSetBonusHitCooldown = 300;
            }

            hydraHideTime = 120;

            justHit = true;
        }

        //public override void PreUpdateBuffs()
        //{
        //UpdateFractalizationTimer();
        //}

        public override void PreUpdateMovement()
        {
            Turbulence.Update(Player);

            //apply dash if it exists
            if (Dash.HasDash(dashIndex) && CanUseAnyDash())
            {
                Dash dash = Dash.GetDash(dashIndex);

                if (dashDir != -1 && dashDelay == 0 && dash.TryUse(Player))
                {
                    Vector2 newVelocity = Player.velocity;

                    switch (dashDir)
                    {
                        case DashLeft when Player.velocity.X > -dash.Speed:
                        case DashRight when Player.velocity.X < dash.Speed:
                            {
                                // X-velocity is set here
                                float dashDirection = dashDir == DashRight ? 1 : -1;
                                newVelocity.X = dashDirection * dash.Speed;
                                break;
                            }
                        default:
                            return; // not moving fast enough, so don't start our dash
                    }

                    // start our dash
                    dashDelay = dash.Cooldown;
                    dashTimer = dash.Duration;
                    Player.velocity = newVelocity;

                    dash.OnDash(Player);
                }
            }

            if (dashDelay > 0)
            {
                dashDelay--;
            }

            if (dashTimer > 0)
            {
                //dash is active
                if (Dash.HasDash(dashIndex) && CanUseAnyDash())
                {
                    Dash dash = Dash.GetDash(dashIndex);

                    dash.Update(Player, dashTimer);
                }

                // count down frames remaining
                dashTimer--;
            }

            Player.velocity *= velocityMultiplier;
        }

        private bool CanUseAnyDash()
        {
            return Player.dashType == 0 && !Player.setSolar && !Player.mount.Active;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            //ModifyHitNPCWithAnything(target, item.DamageType, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            //ModifyHitNPCWithAnything(target, proj.DamageType, ref damage, ref knockback, ref crit);
        }

        public void ModifyHitNPCWithAnything(NPC target, DamageClass damageType, ref int damage, ref float knockback, ref bool crit)
        {
            if (damageType != DamageClass.Magic && damageType.GetModifierInheritance(DamageClass.Magic).damageInheritance == 0f && damageType.GetModifierInheritance(DamageClass.Generic).Equals(StatInheritanceData.Full))
            {
                damage = (int)(damage * nonMagicDamage.Additive * nonMagicDamage.Multiplicative);
            }

            if (target.HasBuff(BuffType<Pinpointed>()) && Main.rand.NextBool()) crit = true;

            target.GetGlobalNPC<PolaritiesNPC>().ignoredDefenseFromCritAmount = 0;
            if (crit)
            {
                damage = Math.Max(damage, (int)(damage * critDamageBoostMultiplier));
                target.GetGlobalNPC<PolaritiesNPC>().ignoredDefenseFromCritAmount = ignoreCritDefenseAmount;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            //ModifyHitByAnything(ref damage, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            //ModifyHitByAnything(ref damage, ref crit);
        }

        public void ModifyHitByAnything(ref int damage, ref bool crit)
        {
            if (Player.HasBuff(BuffType<Pinpointed>()) && Main.rand.NextBool()) crit = true;
        }

        //public int GetFractalization()
        //{
        //return fractalization;
        //}

        public override void UpdateBadLifeRegen()
        {
            if (coneVenom)
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 70;
            }

            if (desiccation > 0)
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                if (desiccation > 60 * 10)
                {
                    Player.lifeRegen -= 60;
                }
            }

            if (incineration * 2 - incinerationResistanceTime > 0)
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= incineration * 2 - incinerationResistanceTime;
            }
        }

        //return true if we do vanilla life ticking, return false if we do our own
        public bool DoUpdateBadLifeRegen()
        {
            int incineratonLifeRegenValue = incineration * 2 - incinerationResistanceTime;
            if (incineratonLifeRegenValue > 0)
            {
                int lifeRegenStep = (int)Math.Ceiling(incineratonLifeRegenValue / 12f);
                while (Player.lifeRegenCount <= -lifeRegenStep * 120)
                {
                    Player.lifeRegenCount += lifeRegenStep * 120;
                    Player.statLife -= lifeRegenStep;
                    CombatText.NewText(Player.Hitbox, CombatText.LifeRegen, lifeRegenStep, dramatic: false, dot: true);
                    if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer)
                    {
                        Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValueWith("Mods.Polarities.DeathMessage.Incinerating", new { PlayerName = Player.name })), 10.0, 0);
                    }
                }
                return false;
            }

            if (coneVenom)
            {
                while (Player.lifeRegenCount <= -720)
                {
                    Player.lifeRegenCount += 720;
                    Player.statLife -= 6;
                    CombatText.NewText(Player.Hitbox, CombatText.LifeRegen, 6, dramatic: false, dot: true);
                    if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer)
                    {
                        Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValueWith("Mods.Polarities.DeathMessage.ConeVenom", new { PlayerName = Player.name })), 10.0, 0);
                    }
                }
                return false;
            }

            if (desiccation > 60 * 10)
            {
                while (Player.lifeRegenCount <= -600)
                {
                    Player.lifeRegenCount += 600;
                    Player.statLife -= 5;
                    CombatText.NewText(Player.Hitbox, CombatText.LifeRegen, 5, dramatic: false, dot: true);
                    if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer)
                    {
                        Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValueWith("Mods.Polarities.DeathMessage.Desiccating", new { PlayerName = Player.name })), 10.0, 0);
                    }
                }
                return false;
            }

            return true;
        }

        //modded dev items
        private void Player_TryGettingDevArmor(Terraria.On_Player.orig_TryGettingDevArmor orig, Player self, IEntitySource source)
        {
            orig(self, source);

            if (source != null && source is EntitySource_ItemOpen itemSource && GetModItem(itemSource.ItemType)?.Mod == Mod)
            {
                if (ItemID.Sets.BossBag[itemSource.ItemType] && (!ItemID.Sets.PreHardmodeLikeBossBag[itemSource.ItemType] || Main.tenthAnniversaryWorld))
                {
                    if (Main.rand.NextBool(Main.tenthAnniversaryWorld ? 10 : 20))
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                self.QuickSpawnItem(source, ItemType<TuringHead>());
                                self.QuickSpawnItem(source, ItemType<TuringBody>());
                                self.QuickSpawnItem(source, ItemType<TuringLegs>());
                                self.QuickSpawnItem(source, ItemType<TuringWings>());
                                break;
                            case 1:
                                self.QuickSpawnItem(source, ItemType<ElectroManiacHead>());
                                self.QuickSpawnItem(source, ItemType<ElectroManiacBody>());
                                self.QuickSpawnItem(source, ItemType<ElectroManiacLegs>());
                                break;
                            case 2:
                                self.QuickSpawnItem(source, ItemType<BubbyHead>());
                                self.QuickSpawnItem(source, ItemType<BubbyBody>());
                                self.QuickSpawnItem(source, ItemType<BubbyLegs>());
                                self.QuickSpawnItem(source, ItemType<BubbyWings>());
                                self.QuickSpawnItem(source, ItemType<VariableWispon>());
                                break;
                        }
                    }
                }
            }
        }
    }
}