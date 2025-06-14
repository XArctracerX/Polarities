using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Polarities.Content.Projectiles
{
	public class EclipxieBookTrail : ModProjectile
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Eclipse Trail");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults() {
			Projectile.aiStyle = -1;
			Projectile.width = 76;
			Projectile.height = 76;
			Projectile.alpha = 0;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.light = 1f;
		}

		public override void AI() {
            Projectile.alpha++;
			if (Projectile.alpha >= 255) {
				Projectile.Kill();
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            if (Main.rand.NextBool(128)) {
                float distance = 1000f;
                bool isTarget = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++) {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable) {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance) {
                            targetID = k;
                            distance = distanceTo;
                            isTarget = true;
                        }
                    }
                }

                if (isTarget) {
                    NPC target = Main.npc[targetID];

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,(target.Center-Projectile.Center).SafeNormalize(Vector2.Zero)*0.1f,ProjectileType<EclipxieBookScythe>(),80,1,Projectile.owner);
                } else {
                    return;
                }
            }
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.OnFire, 450, true);
			target.AddBuff(BuffID.Frostburn, 450, true);
			Projectile.penetrate -= 1;
			if (Projectile.penetrate == 0) { Projectile.Kill();}
		}

        public override bool? CanCutTiles() {
            return false;
        }
	}
}