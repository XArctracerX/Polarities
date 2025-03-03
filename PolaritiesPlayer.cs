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
		public float manaStarMultiplier;
		public bool stormcore;
		public bool wormScarf;

		//public override void UpdateDead()
		//{
			//fractalization = 0;
		//}

		public override void ResetEffects()
		{
			//reset a bunch of values
			manaStarMultiplier = 1f;
			stormcore = false;
			wormScarf = false;
		}

		public override void PostUpdate()
		{
			if (stormcore && 0.2f + Player.slotsMinions <= Player.maxMinions && Main.rand.NextBool(60))
			{
				Main.projectile[Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center.X + 500 * (2 * (float)Main.rand.NextDouble() - 1), Player.Center.Y - 500, 0, 0, ProjectileType<StormcoreMinion>(), 1, Player.GetTotalKnockback(DamageClass.Summon).ApplyTo(0.5f), Player.whoAmI, 0, 0)].originalDamage = 1;
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
	}
}