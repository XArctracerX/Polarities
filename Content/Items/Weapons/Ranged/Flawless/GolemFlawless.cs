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

namespace Polarities.Content.Items.Weapons.Ranged.Flawless
{
	public class GolemFlawless : ModItem
	{
		public override void SetStaticDefaults() {
            //Tooltip.SetDefault("Creates a field of electricity upon impact");
            Item.ResearchUnlockCount = 1;
			PolaritiesItem.IsFlawless.Add(Type);
		}

		public override void SetDefaults() {
			Item.damage = 32;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.knockBack = 1f;
			Item.value = 50;
			Item.rare = RarityType<GolemFlawlessRarity>();
			Item.shoot = ProjectileType<GolemFlawlessArrow>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 7f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

        /*public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Items.SmiteSoul>());
            recipe.AddIngredient(ItemID.WoodenArrow,111);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this,111);
            recipe.AddRecipe();
        }*/
	}
    
    public class GolemFlawlessArrow : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Electric Arrow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
        }

		Projectile sun;
        public override void AI() {
			if (Projectile.localAI[0] == 0)
            {
				sun = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileType<GolemFlawlessSun>(), Projectile.damage, 0f, Projectile.owner);
				Projectile.localAI[0] = 1;
            }

			if (sun.ai[2] != 1)
            {
				sun.position = Projectile.position + Projectile.velocity;
				sun.velocity = Projectile.velocity / 2;
			}
            Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Yellow, Scale: 1f)];
            d.noGravity = true;
            d.velocity /= 4;

            Projectile.rotation = Projectile.velocity.ToRotation();
Projectile.velocity.Y += 0.1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.Kill();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurt) {
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
		}
    }

	public class GolemFlawlessSun : ModProjectile
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
			Projectile.width = 2;
			Projectile.height = 2;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;

			Projectile.scale = 0.5f;
			Projectile.alpha = 0;
			Projectile.timeLeft = 540;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.scale = 0.75f;
		}

		public override void AI()
		{
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Yellow, Scale: 0.75f)].noGravity = true;

			foreach (NPC npc in Main.npc)
            {
				if (!npc.active) continue;
				if (npc.life <= 0) continue;
				if (npc.Distance(Projectile.position) > 210) continue;
				npc.AddBuff(BuffID.OnFire3, 10);
            }
			Projectile.tileCollide = tilelessTicks-- < 0;
			Projectile.frameCounter++;
			Projectile.spriteDirection = (int)Projectile.ai[0];
		}

		int tilelessTicks = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[2] = 1;
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				Projectile.velocity.X = -oldVelocity.X;

			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				Projectile.velocity.Y = -oldVelocity.Y;

			tilelessTicks = 2;

			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire3, 120);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
		{
			target.AddBuff(BuffID.OnFire3, 120);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Yellow, Scale: 1f).noGravity = true;
			}
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
				scale *= Projectile.scale;
				Color color = new Color(255, (int)(195 * scale + 512 * (1 - scale)), (int)(32 * scale + 512 * (1 - scale)));
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
				scale *= 0.75f * Projectile.scale;
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
