using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
	public class InfinitySponge : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }
        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<InfinitySpongeProjectile>();
			Item.shootSpeed = 10;
		}

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }

	public class InfinitySpongeProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Tools/Misc/Hardmode/InfinitySponge";

        public int ticksToReturn = 60;
        Vector2 ogPos;

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 26;
            Projectile.height = 24;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

		public override void AI()
        {
            for (int i = -4; i < 5; i++)
            {
                for (int j = -4; j < 5; j++)
                {
                    if (i * i + j * j > 4.5 * 4.5) continue;
                    int x = (int)(Projectile.Center.X / 16) + i;
                    int y = (int)(Projectile.Center.Y / 16) + j;
                    Main.tile[x, y].LiquidAmount = 0;
                }
            }
            Liquid.UpdateLiquid();

            if (Projectile.timeLeft > ticksToReturn)
            {
                Projectile.velocity *= 0.95f;
                Projectile.rotation += MathHelper.ToRadians(6);
            }
            else
            {
                if (Projectile.timeLeft == ticksToReturn) ogPos = Projectile.position;
                float lifetime = ticksToReturn - Projectile.timeLeft;
                Projectile.position = Vector2.Lerp(ogPos, Main.player[Projectile.owner].position, lifetime / ticksToReturn);
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0, 0.05f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
    }
}