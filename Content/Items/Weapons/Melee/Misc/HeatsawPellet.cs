using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Polarities.Content.Items.Weapons.Melee.Misc
{
    public class HeatsawPellet : ModProjectile
    {
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.hide = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = 999;

            Projectile.ArmorPenetration = 999;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }
        public override void AI()
        {
            int x = (int)(Projectile.Center.X / 16);
            int y = (int)(Projectile.Center.Y / 16);
            if (Main.tile[x, y].TileType == TileID.Trees) WorldGen.KillTile(x, y);

            if (!Main.dedServ && Projectile.ai[0] < 25)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.OrangeTorch, Scale: 1.5f);
                d.noGravity = true;
                d.velocity = Projectile.velocity / 8f;
            }

            if (Projectile.ai[0] > 5)
            {
                if (Projectile.ai[0] < 10)
                {
                    Projectile.velocity.Y += 0.5f;
                }
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(Main.player[Projectile.owner].Center) * 32f, 0.06f);
                if (Projectile.Center.Distance(Main.player[Projectile.owner].Center) < 10)
                {
                    Projectile.Kill();
                }
            }
            Projectile.ai[0]++; base.AI();
        }
    }
}
