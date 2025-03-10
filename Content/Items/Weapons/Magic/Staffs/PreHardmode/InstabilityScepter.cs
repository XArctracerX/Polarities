using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Staffs.PreHardmode
{
	public class InstabilityScepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Summons a short-lived rift");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.width = 46;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item8;
			Item.shoot = ProjectileType<InstabilityScepterProjectile>();
			Item.shootSpeed = 1f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

	public class InstabilityScepterProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Unstable Rift");
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 54;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.light = 0.75f;
		}

        public override void AI()
        {
			Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.spriteDirection = Projectile.direction;

			if (Projectile.localAI[0] == 0)
            {
				SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
            }

			Projectile.localAI[0]++;
		}

        public override bool ShouldUpdatePosition()
        {
			return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float xScale = Math.Min(1, Math.Max(1 / 8f, Math.Min((Projectile.localAI[0] - 8) / 8f, (Projectile.timeLeft - 8) / 8f)));
			float yScale = Math.Min(1, Math.Max(0, Math.Min(Projectile.localAI[0] / 8f, Projectile.timeLeft / 8f)));

			hitbox.Inflate((int)((xScale - 1) * hitbox.Width / 2), (int)((yScale - 1) * hitbox.Height / 2));
        }

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();

			float xScale = Math.Min(1, Math.Max(1 / 8f, Math.Min((Projectile.localAI[0] - 8) / 8f, (Projectile.timeLeft - 8) / 8f)));
			float yScale = Math.Min(1, Math.Max(0, Math.Min(Projectile.localAI[0] / 8f, Projectile.timeLeft / 8f)));

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, new Vector2(xScale, yScale), Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

			return false;
        }
    }
}