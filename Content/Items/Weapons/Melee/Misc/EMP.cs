using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.DataStructures;

namespace Polarities.Content.Items.Weapons.Melee.Misc
{
	public class EMP : ModItem
	{
		public override void SetStaticDefaults() {
			//DisplayName.SetDefault("E.M.P.");
			//Tooltip.SetDefault("Releases an electromagnetic pulse");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 420;
			Item.DamageType = DamageClass.Melee;
			Item.width = 14;
			Item.height = 46;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 10;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<EMPProjectile>();
			Item.shootSpeed = 0f;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = false;
			Item.noMelee = true;
		}

		float rotDir = 1;
        public override System.Boolean Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).ai[0] = rotDir;
			rotDir *= -1;
			return false;
        }
    }

    public class EMPProjectile : ModProjectile
	{
        public override void SetStaticDefaults() {
			//DisplayName.SetDefault("Electromagnetic Pulse");
		}

		public override void SetDefaults() {
			Projectile.width = 64;
			Projectile.height = 64;
            Projectile.scale = 0.01f;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
            Projectile.timeLeft = 64;
			Projectile.alpha = 0;
		}

		float pushForce = 0.005f;
		public override void AI() {
            Projectile.scale += 0.02f;
			Projectile.width = (int)(640*Projectile.scale);
			Projectile.height = (int)(640*Projectile.scale);
			DrawOffsetX = (int)(0.5f*(Projectile.height-758));
			DrawOriginOffsetY = (int)(0.5f*(Projectile.height-758));
			DrawOriginOffsetX = 0;

			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				Projectile.active = false;
				return;
			}

            Projectile.rotation -= 0.05f/Projectile.scale * Projectile.ai[0];
            Projectile.alpha += 4;
            Projectile.velocity = player.Center-Projectile.Center;

            if (Projectile.timeLeft % 20 == 0) {
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap,Projectile.Center);
            }

			foreach (Projectile p in Main.projectile)
            {
				if (!p.active || p.Distance(player.position) > (Projectile.width / 2)) continue;

				p.velocity += pushForce * (p.position - player.position);
            }
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float nearestX = Math.Max(targetHitbox.X, Math.Min(Projectile.Center.X, targetHitbox.X + targetHitbox.Size().X));
			float nearestY = Math.Max(targetHitbox.Y, Math.Min(Projectile.Center.Y, targetHitbox.Y + targetHitbox.Size().Y));
			return (new Vector2(Projectile.Center.X-nearestX,Projectile.Center.Y-nearestY)).Length() < Projectile.width/2;
		}

		public override bool? CanCutTiles() {
			return false;
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.FinalDamage *= Projectile.timeLeft / 30;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 758, 758), Color.White * (1 - Projectile.alpha / 255f), Projectile.rotation, new Vector2(379, 379), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}