using Terraria;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Books.Hardmode
{
    public class EclipxieBook : BookBase
    {
        public override int BuffType => BuffType<EclipxieBookBuff>();
        public override int BookIndex => 26;
    }

    public class EclipxieBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<EclipxieBook>();

        int trailTimer = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            // Do stuff
            // porter's note: AWESOME COMMENT
            trailTimer++;
            if (trailTimer >= 40)
            {
                trailTimer = 0;
                //summon projectile
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(0.25f, 0).RotatedByRandom(MathHelper.Pi * 2), ProjectileType<Projectiles.EclipxieBookTrail>(), 120, 0, player.whoAmI);
            }

            base.Update(player, ref buffIndex);
        }
    }
}