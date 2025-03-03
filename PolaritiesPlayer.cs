using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Items.Accessories.PreHardmode;
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
        public Dictionary<float, float> screenShakes = new Dictionary<float, float>(); //key is time at which the screenshake ends, value is magnitude
        private int screenshakeRandomSeed;

        public float hookSpeedMult;
        public float manaStarMultiplier;
        public float orbMinionSlots;
        public int royalOrbHitCount;
        public Vector2 velocityMultiplier;

        public int desiccation;
        public int incineration;
        public int incinerationResistanceTime;
        public bool coneVenom;

        public bool stormcore;
		public bool wormScarf;

        public float candyCaneAtlatlBoost;

        public int itemHitCooldown = 0;

        //public override void UpdateDead()
        //{
        //fractalization = 0;
        //}

        public override void ResetEffects()
		{
            //reset a bunch of values
            hookSpeedMult = 1f;
            manaStarMultiplier = 1f;
            orbMinionSlots = 1f;

            desiccation = 0;
            incineration = 0;
            incinerationResistanceTime = 0;
            coneVenom = false;

            stormcore = false;
			wormScarf = false;

            if (candyCaneAtlatlBoost > 0) candyCaneAtlatlBoost--;

            screenshakeRandomSeed = Main.rand.Next();

            if (itemHitCooldown > 0) itemHitCooldown--;

            Player.velocity /= velocityMultiplier;
            velocityMultiplier = Vector2.One;
        }

		public override void PostUpdate()
		{
			if (stormcore && 0.2f + Player.slotsMinions <= Player.maxMinions && Main.rand.NextBool(60))
			{
				Main.projectile[Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X + 500 * (2 * (float)Main.rand.NextDouble() - 1), Player.Center.Y - 500, 0, 0, ProjectileType<StormcoreMinion>(), 1, Player.GetTotalKnockback(DamageClass.Summon).ApplyTo(0.5f), Player.whoAmI, 0, 0)].originalDamage = 1;
			}
		}

        public void AddScreenShake(float magnitude, float timeLeft)
        {
            if (magnitude > 0 && timeLeft > 0)
            {
                float endTime = timeLeft + PolaritiesSystem.timer;
                if (screenShakes.ContainsKey(endTime))
                {
                    screenShakes[endTime] += magnitude / timeLeft;
                }
                else
                {
                    screenShakes.Add(endTime, magnitude / timeLeft);
                }
            }
        }

        public override void ModifyScreenPosition()
        {
            if (screenShakes.Keys.Count > 0)
            {
                List<float> removeTimesLeft = new List<float>();

                Polarities.preGeneratedRand.SetIndex(screenshakeRandomSeed);
                foreach (float timeLeft in screenShakes.Keys)
                {
                    if (timeLeft <= PolaritiesSystem.timer)
                    {
                        removeTimesLeft.Add(timeLeft);
                    }
                    else
                    {
                        Main.screenPosition += new Vector2(Polarities.preGeneratedRand.NextNormallyDistributedFloat(screenShakes[timeLeft] * (timeLeft - PolaritiesSystem.timer)), 0).RotatedBy(Polarities.preGeneratedRand.NextFloat(MathHelper.TwoPi));
                    }
                }
                foreach (float timeLeft in removeTimesLeft) screenShakes.Remove(timeLeft);
            }

            //to prevent jittering of some things
            Main.screenPosition.X = (int)Main.screenPosition.X;
            Main.screenPosition.Y = (int)Main.screenPosition.Y;
        }

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

            //if ((proj.sentry || ProjectileID.Sets.SentryShot[proj.type]) && Player.HasBuff(BuffType<BetsyBookBuff>()))
            //{
                //target.AddBuff(BuffID.OnFire, 300);
                //target.AddBuff(BuffID.OnFire3, 300);
            //}
        }

        //public void OnHitNPCWithAnything(NPC target, int damage, float knockback, bool crit, DamageClass damageClass)
        //{
            //if (snakescaleSetBonus && crit)
            //{
                //target.AddBuff(BuffID.Venom, 5 * 60);
            //}

            //if (moonLordLifestealCooldown == 0 && Player.HasBuff(BuffType<MoonLordBookBuff>()) && !Player.moonLeech)
            //{
                //float baseLifestealAmount = (float)Math.Log(damage * Math.Pow(Main.rand.NextFloat(1f), 4));
                //if (baseLifestealAmount >= 1)
                //{
                    //moonLordLifestealCooldown = 10;
                    //Player.statLife += (int)baseLifestealAmount;
                    //Player.HealEffect((int)baseLifestealAmount);
                //}
            //}

            //if (damageClass != DamageClass.Magic && !damageClass.GetEffectInheritance(DamageClass.Magic) && solarEnergizer)
            //{
                //Player.statMana++;
            //}
        //}

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //target via orbs
            if (PlayerInput.Triggers.JustPressed.MouseRight && (Player.HeldItem.DamageType == DamageClass.Summon || Player.HeldItem.DamageType.GetEffectInheritance(DamageClass.Summon)) && Player.channel)
            {
                Player.MinionNPCTargetAim(false);
            }
        }
        //public override void PreUpdateBuffs()
        //{
        //UpdateFractalizationTimer();
        //}

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
    }
}