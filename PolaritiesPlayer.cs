using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
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

		public bool stormcore;
		public bool wormScarf;

        public float candyCaneAtlatlBoost;

        //public override void UpdateDead()
        //{
        //fractalization = 0;
        //}

        public override void ResetEffects()
		{
            //reset a bunch of values
            hookSpeedMult = 1f;
            manaStarMultiplier = 1f;

			stormcore = false;
			wormScarf = false;

            if (candyCaneAtlatlBoost > 0) candyCaneAtlatlBoost--;

            screenshakeRandomSeed = Main.rand.Next();
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

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

        }
            //public override void PreUpdateBuffs()
            //{
            //UpdateFractalizationTimer();
            //}

            //public int GetFractalization()
            //{
            //return fractalization;
            //}
    }
}