using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using Polarities.Global;

namespace Polarities.Content.Items.Weapons.Melee.Misc
{
    public class HeatsawLava : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        int stickTarget = -1;
        Vector2 stickOffset = Vector2.Zero;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;

            Projectile.ArmorPenetration = 999;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.extraUpdates = 3;
        }

        bool grounded = false;
        public override void AI()
        {
            if (stickTarget != -1)
            {
                NPC target = Main.npc[stickTarget];
                Projectile.position = target.position + stickOffset;
            }
            else if (!grounded) Projectile.velocity.Y += 0.05f;

            Projectile.ai[0]++; base.AI();
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor1 = Color.LightGoldenrodYellow;
			Color mainColor2 = new Color(246, 109, 65, 128);

			Color fringeColor1 = new Color(246, 109, 65, 128);
			Color fringeColor2 = new Color(224, 55, 0, 32);

			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
                float progress = 1f * i / Projectile.oldPos.Length;

                Color mainColor = Color.Lerp(mainColor1, mainColor2, progress);
                Color fringeColor = Color.Lerp(fringeColor1, fringeColor2, progress);

                Vector2 position = Projectile.oldPos[i];

				float scale = 0.4f * ((1f * Projectile.oldPos.Length - i) / (Projectile.oldPos.Length * 2) + 0.5f);

				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + position - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), fringeColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale * scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + position - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale * scale / 2, SpriteEffects.None, 0f);
				Lighting.AddLight(position, fringeColor.ToVector3());
			}

			//if (progress < 1.99f) Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale , SpriteEffects.None, 0f);
			return false;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            grounded = true;
            return false;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);
            if (stickTarget == -1)
            {
                stickTarget = target.whoAmI;
                stickOffset = Projectile.position - target.position;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (stickTarget != -1) modifiers.FinalDamage /= 2;
        }
    }
}
