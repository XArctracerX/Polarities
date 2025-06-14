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

namespace Polarities.Content.Items.Weapons.Melee.Flawless
{
	public class WandererFlawless : ModItem
	{
        public override void Load()
        {
			On_Player.PickTile += AddGauge;
        }

		public static void AddGauge(On_Player.orig_PickTile orig, Player player, int x, int y, int pickPower)
        {
			if (player.HeldItem.ModItem is WandererFlawless drill)
            {
				drill.charge++;
				if (drill.charge > 100) drill.charge = 100;
            }
			orig(player, x, y, pickPower);
        }

        public override void SetStaticDefaults()
		{
			ItemID.Sets.IsDrill[Type] = true;
			Item.ResearchUnlockCount = 1;
			PolaritiesItem.IsFlawless.Add(Type);
		}

		public bool isInDrillMode = false;
		public float charge = 100;
		bool isHeld = false;

		Color gaugeColor = Color.White;

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, System.Single scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			if (isHeld)
            {
				float percent = 1 - charge / 100f;
				Texture2D gaugeTex = Request<Texture2D>("Polarities/Content/Items/Weapons/Melee/Flawless/ConvectiveGauge").Value;
				Texture2D pixel = TextureAssets.MagicPixel.Value;
				int barTop = Main.screenHeight / 2 - gaugeTex.Height / 2 + 10;
				barTop += (int)(40 * percent);

				Color targetColor = isInDrillMode ? new Color(5, 227, 255) : new Color(227, 71, 17);
				gaugeColor = Color.Lerp(gaugeColor, targetColor, 0.05f);

				spriteBatch.Draw(pixel, new Rectangle(Main.screenWidth / 2 - 40, barTop, 2, 40 - (int)(40 * percent)), gaugeColor);
				spriteBatch.Draw(gaugeTex, new Vector2(Main.screenWidth / 2 - 50, Main.screenHeight / 2 - gaugeTex.Height / 2), Color.White);
            }
		}

        public override void SetDefaults()
		{
			Item.damage = 300;
			Item.DamageType = DamageClass.MeleeNoSpeed; // ignores melee speed bonuses. There's no need for drill animations to play faster, nor drills to dig faster with melee speed.
			Item.width = 50;
			Item.height = 24;
			Item.useTime = 2;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			//Item.value = Item.buyPrice(gold: 12, silver: 60);
			Item.rare = RarityType<ConvectiveWandererFlawlessRarity>();
			Item.UseSound = SoundID.Item23;
			Item.shoot = ProjectileType<WandererDrill>();
			Item.shootSpeed = 64f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;

			Item.tileBoost = -2;

			Item.pick = 0; // or 220
		}

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
			isHeld = player.HeldItem == Item;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
            {
				isInDrillMode = !isInDrillMode;
				if (isInDrillMode) Item.pick = 220;
				else Item.pick = 0;
				return false;
            }
			return true;
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }
    }

	public class WandererDrill : ModProjectile
	{
		//public override string Texture => "PolaritiesReworkIdeas/Content/Items/Weapons/Wanderer/WandererFlawless";

		public override void SetStaticDefaults()
		{
			// Prevents jitter when stepping up and down blocks and half blocks
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			Main.projFrames[Type] = 2;
		}

		public Vector2 baseOffset = new Vector2(-8, 4);

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
			Projectile.aiStyle = -1;
			Projectile.hide = true; 
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire3, 180);
        }

		int timer = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player.HeldItem.ModItem is WandererFlawless drill)
            {
				Projectile.frame = drill.isInDrillMode ? 1 : 0;

				if (drill.charge > 0 && timer++ % 3 == 0 && !player.controlDown)
                {
					Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.DirectionTo(Main.MouseWorld) * Projectile.velocity.Length() / 8f,
										 ProjectileType<WandererProjectile>(), Projectile.damage / 4, Projectile.knockBack / 4, Owner: Projectile.owner);
					drill.charge -= 0.5f;
					if (drill.charge < 0) drill.charge = 0;
				}
            }

			Projectile.timeLeft = 60;

			if (Projectile.soundDelay <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
				Projectile.soundDelay = 20;
			}

			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel)
				{
					float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
					Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
					//holdoutOffset += new Vector2(baseOffset.X * player.direction, baseOffset.Y);
					if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}

					Projectile.velocity = holdoutOffset;
				}
				else
				{
					Projectile.Kill();
				}
			}

			if (Projectile.velocity.X > 0f)
			{
				player.ChangeDir(1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				player.ChangeDir(-1);
			}

			Projectile.spriteDirection = Projectile.direction;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			Projectile.Center = playerCenter;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

			Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.01f;
			Projectile.velocity.Y *= 1f + Main.rand.Next(-1, 1) * 0.01f;

			if (Main.rand.NextBool(10))
			{
				int dType = DustID.Torch;
				if (player.HeldItem.ModItem is WandererFlawless d)
                {
					if (d.isInDrillMode) dType = DustID.Flare_Blue;
                }
				Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity * Main.rand.Next(6, 10) * 0.15f, Projectile.width, Projectile.height, dType, 0f, 0f, 80, Color.White, 1f);
				dust.position.X -= 4f;
				dust.noGravity = true;
				dust.velocity.X *= 0.5f;
				dust.velocity.Y = -Main.rand.Next(3, 8) * 0.1f;
			}
		}
	}

	public class WandererProjectile : ModProjectile
    {
		public override string Texture => "Terraria/Images/Projectile_644";

		public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Type] = 999;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

        public override void SetDefaults()
        {
			Projectile.width = 50;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.DamageType = DamageClass.Melee;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		Vector2 ogMouseWorld;
		Vector2 controlPoint1, controlPoint2;
		float progress;

		int ticksToExtend = 240;
		Vector2[] positions = new Vector2[240];

        public override void AI()
        {
			Player player = Main.player[Projectile.owner];

			try
			{
				if (player.HeldItem.ModItem is WandererFlawless drill)
				{
					if (drill.isInDrillMode)
					{
						for (int i = -1; i < 2; i++)
                        {
							for (int j = -1; j < 2; j++)
                            {
								int x = (int)(Projectile.Center.X / 16) + i;
								int y = (int)(Projectile.Center.Y / 16) + j;
								WorldGen.KillTile(x, y);
								if (Main.tile[x, y].HasTile) ticksToExtend = (int)Projectile.ai[0];
							}
                        }
					}
				}
			}
			catch (Exception e) { }


			if (Projectile.ai[0] == 0)
			{
				ogMouseWorld = Main.MouseWorld + Main.rand.NextVector2Circular(40, 40);
				controlPoint1 = Vector2.Lerp(player.position, ogMouseWorld, 0.5f) + Main.rand.NextVector2Circular(400, 400);
				controlPoint2 = Vector2.Lerp(player.position, ogMouseWorld, 0.5f) + Main.rand.NextVector2Circular(400, 400);
			}

			Projectile.ai[0]++;

			progress = Projectile.ai[0] / ticksToExtend;

			if (progress > 2) Projectile.Kill();
			else if (progress > 1)
			{
				int positionToReference = ticksToExtend - ((int)Projectile.ai[0] % ticksToExtend);
				Projectile.position = positions[positionToReference];
			}
			else
			{
				Vector2 cubPos1 = Vector2.Lerp(player.position, controlPoint1, progress);
				Vector2 cubPos2 = Vector2.Lerp(controlPoint1, controlPoint2, progress);
				Vector2 cubPos3 = Vector2.Lerp(controlPoint2, ogMouseWorld, progress);
				Vector2 bezPos1 = Vector2.Lerp(cubPos1, cubPos2, progress);
				Vector2 bezPos2 = Vector2.Lerp(cubPos2, cubPos3, progress);
				Projectile.position = Vector2.Lerp(bezPos1, bezPos2, progress);
				positions[(int)Projectile.ai[0] % ticksToExtend] = Projectile.position;
			}
			Projectile.rotation = Projectile.oldPos[0].AngleTo(Projectile.position) + MathHelper.PiOver2;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if (Main.player[Projectile.owner].HeldItem.ModItem is WandererFlawless drill)
			{
				if (!drill.isInDrillMode)
				{
					modifiers.FinalDamage *= 1.5f;
					target.AddBuff(BuffType<Buffs.Hardmode.Incinerating>(), 90);
				}
				else target.AddBuff(BuffType<Buffs.Hardmode.Incinerating>(), 180);
            }
			if (progress < 1) ticksToExtend = (int)Projectile.ai[0];
        }

		public float Sine(float x)
        {
			return new Vector2(1, 0).RotatedBy(x).Y;
        }

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor1 = Color.LightGoldenrodYellow;
			Color mainColor2 = new Color(246, 109, 65, 128);

			Color fringeColor1 = new Color(246, 109, 65, 128);
			Color fringeColor2 = new Color(224, 55, 0, 32);

			if (Main.player[Projectile.owner].HeldItem.ModItem is WandererFlawless drill)
            {
				if (drill.isInDrillMode)
                {
					mainColor1 = Color.AliceBlue;
					mainColor2 = new Color(152, 255, 255, 128);

					fringeColor1 = new Color(49, 160, 247, 128);
					fringeColor2 = new Color(0, 128, 255, 32);
                }
            }

			Color mainColor = Color.Lerp(mainColor1, mainColor2, MathHelper.Max(0, progress - 1));
			Color fringeColor = Color.Lerp(fringeColor1, fringeColor2, progress / 2);
			mainColor = Color.White;

			int positionToReference = ticksToExtend - ((int)Projectile.ai[0] % ticksToExtend);
			for (int i = 0; i < positions.Length - 1; i++)
            {
				if (i == 0 || i == ticksToExtend) continue;
				if (progress >= 1f && i >= positionToReference) continue;
				if (progress > 1.99f) continue;
				Vector2 position = positions[i];

				float scale = 0.4f * ((1f * positions.Length - i) / (positions.Length * 2) + 0.5f);
				//scale = i / 100f;
				/*if (i == positionToReference)
				{
					mainColor = Color.LightGreen;
					fringeColor = Color.YellowGreen;
				}*/

				float rotation = (positions[i + 1] - positions[i]).ToRotation() + MathHelper.PiOver2;
				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + position - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), fringeColor, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale * (scale * 1.5f + (0.1f * Sine(MathHelper.ToRadians(2f * i)))), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Projectile.position + position - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale * scale, SpriteEffects.None, 0f);
				Lighting.AddLight(position, fringeColor.ToVector3());
			}

			//if (progress < 1.99f) Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale , SpriteEffects.None, 0f);
			return false;
		}
	}
}