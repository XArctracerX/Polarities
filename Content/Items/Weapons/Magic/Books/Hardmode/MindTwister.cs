using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Books.Hardmode
{
	public class MindTwister : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mind Twister");
			//Tooltip.SetDefault("Surrounds you in a minefield of confusing energy whorls"+"\nGetting hit breaks your concentration, dispelling the minefield");
		}

		public override void SetDefaults()
		{
			Item.damage = 39;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 32;
			Item.mana = 3;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item93;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.shootSpeed = 20f;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Pink;
			Item.shoot = ProjectileType<MindTwisterProjectile>();
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}

		private float theta;
		private int time;

		public override void HoldItem(Player player)
		{
			if (player.channel && !player.GetModPlayer<PolaritiesPlayer>().justHit)
			{
				time++;
				player.itemTime++;
				player.itemAnimation++;
				player.manaRegen = Math.Min(player.manaRegen, 0);
				if (time % 10 == 0)
				{
					//if (!player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
					//{
					//	if (!player.CheckMana(player.inventory[player.selectedItem].mana, true))
					//	{
					//		player.channel = false;
					//	}
					//}

					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(Item.shootSpeed, 0).RotatedBy(theta), Item.shoot, Item.damage, Item.knockBack, player.whoAmI, player.direction);
					theta += player.direction*(MathHelper.Pi * (3 - (float)Math.Sqrt(5)) + 0.02f * 20);
				}
				if (time % 20 == 0)
				{
					SoundEngine.PlaySound(Item.UseSound, player.position);
				}
			}
			else
			{
				player.channel = false;
				if (time!= 0)
                {
					SoundEngine.PlaySound(SoundID.Item15, player.position);
				}
				time = 0;
				theta = MathHelper.PiOver2;
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}

	public class MindTwisterProjectile : ModProjectile
	{
		public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/SunPixie/SunPixieArena";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Energy Whorl");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 32;
			Projectile.height = 32;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;

			Projectile.alpha = 0;
			Projectile.timeLeft = 540;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.DamageType = DamageClass.Magic;

			Projectile.scale = 0.75f;
		}

		public override void AI()
		{
			if (!Main.player[Projectile.owner].channel)
            {
				Projectile.Kill();
				return;
			}

			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 0.75f)].noGravity = true;

			int age = 540 - Projectile.timeLeft;
			Projectile.position = Main.player[Projectile.owner].Center + Projectile.velocity.RotatedBy(0.02f*age*Projectile.ai[0]) * (float)Math.Sqrt(age) - new Vector2(Projectile.width / 2, Projectile.height / 2);

			Projectile.frameCounter++;
			Projectile.spriteDirection = (int)Projectile.ai[0];
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused,120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
        {
			target.AddBuff(BuffID.Confused, 120);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i=0; i<5; i++)
            {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 1f);
            }
        }

        public override bool ShouldUpdatePosition()
        {
			return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }


		public override bool PreDraw(ref Color lightColor)
		{
			int numDraws = 12;
			for (int i = 0; i < numDraws; i++)
			{
				float scale = (1 + (float)Math.Sin(Projectile.frameCounter * 0.1f + (MathHelper.TwoPi * i) / numDraws)) / 2f;
				Color color = new Color((int)(82 * scale + 512 * (1 - scale)), (int)(179 * scale + 512 * (1 - scale)), (int)(203 * scale + 512 * (1 - scale)));
				float alpha = 0.2f;
				float rotation = Projectile.frameCounter * 0.2f;

				if (Projectile.timeLeft < 30)
				{
					scale *= Projectile.timeLeft / 30f;
				}

				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 64, 64), color * alpha, rotation, new Vector2(32, 32), Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			for (int i = 0; i < numDraws; i++)
			{
				float scale = (1 + (float)Math.Sin(Projectile.frameCounter * 0.1f + (MathHelper.TwoPi * i) / numDraws)) / 2f;
				scale *= 0.75f;
				Color color = new Color((int)(82 * scale + 512 * (1 - scale)), (int)(179 * scale + 512 * (1 - scale)), (int)(203 * scale + 512 * (1 - scale)));
				float alpha = 0.2f;
				float rotation = Projectile.frameCounter * 0.3f;

				if (Projectile.timeLeft < 45)
				{
					scale *= (Projectile.timeLeft - 15) / 30f;
				}
				if (Projectile.timeLeft > 15)
				{
					Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 64, 64), color * alpha, rotation, new Vector2(32, 32), Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}

			return false;
		}
	}
}