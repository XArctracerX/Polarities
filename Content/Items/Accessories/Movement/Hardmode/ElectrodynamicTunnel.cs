using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Polarities.Content.Items.Accessories.Movement.Hardmode
{
	public class ElectrodynamicTunnel : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Electrodynamic Tunnel");
		}

		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
		{
			try
			{
				string hotkey = "an unbound hotkey";
				if (Polarities.ElectrodynamicTunnelHotkey.GetAssignedKeys().ToArray().Length > 0)
				{
					hotkey = Polarities.ElectrodynamicTunnelHotkey.GetAssignedKeys()[0];

				}
				TooltipLine line = new TooltipLine(Mod, "Tooltip1", string.Format("Press {0} to teleport to the point opposite your cursor, leaving behind a damaging electromagnetic trail", hotkey));
				tooltips.Insert(2, line);
			}
			catch(Exception e)
            {

            }
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Expert;
            Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PolaritiesPlayer>().electrodynamicTunnel = true;
		}
    }

    public class PolaritiesTeleportBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 4;
            //DisplayName.SetDefault("Particle Beam");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 40;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

		public override void AI()
        {
			if (Projectile.ai[0]++ > 10)
            {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
				Projectile.ai[0] = 0;
            }
        }

		public override bool ShouldUpdatePosition() => false;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 startPoint = Projectile.Center;
			Vector2 endPoint = Projectile.Center + Projectile.velocity;
			float collisionPoint = 0f;

			float heightMult = 1 - (float)Math.Pow((1 - Projectile.timeLeft / 30f), 2);

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startPoint, endPoint, 16f * heightMult, ref collisionPoint);
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Vector2 startPoint = Projectile.Center;
			Vector2 unit = Projectile.velocity.SafeNormalize(Vector2.Zero);
			float rotation = Projectile.velocity.ToRotation();
			float length = Projectile.velocity.Length();

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			float heightMult = 1 - (float)Math.Pow((1 - Projectile.timeLeft / 40f), 2);

			for (int dist = 0; dist < length; dist++)
            {
				Rectangle frame = new Rectangle((dist + Projectile.timeLeft * 6) % 32, 0, 1, 32);
				float height = (float)Math.Sin(dist / length * MathHelper.Pi) * heightMult;

				Main.spriteBatch.Draw(texture, startPoint + unit * dist - Main.screenPosition, frame, Color.White, rotation, new Vector2(0, 16), new Vector2(1, height), SpriteEffects.None, 0f);
            }

			return false;
        }
    }
}
