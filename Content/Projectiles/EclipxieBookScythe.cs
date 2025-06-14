using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.Audio;

namespace Polarities.Content.Projectiles
{
    public class EclipxieBookScythe : ModProjectile
    {
		public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/Eclipxie/SolarScythe";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Scythe");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f+Projectile.velocity.Length()/4;
            Projectile.velocity *= 1.05f;
            if (Projectile.velocity.Length() > 32) {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 32;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            Main.dust[Dust.NewDust(Projectile.position,Projectile.width,Projectile.height,DustID.Torch,Projectile.velocity.X/2,Projectile.velocity.Y/2,0,Color.Orange,2)].noGravity = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			target.AddBuff(BuffID.OnFire, 450, true);
		}
    }
}