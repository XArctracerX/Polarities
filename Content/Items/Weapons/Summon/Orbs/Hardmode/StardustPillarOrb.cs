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

namespace Polarities.Content.Items.Weapons.Summon.Orbs.Hardmode
{
	public class StardustPillarOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;

			// DisplayName.SetDefault("Stardust Tower Orb");
			// Tooltip.SetDefault("Summons a channeled minion" + "\nSummons an image of the stardust pillar");

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 11));
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Summon;
			Item.width = 34;
			Item.height = 42;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ProjectileType<StardustPillarMinion>();
		}

		public override void HoldItem(Player player)
		{
			if (player.channel)
			{
				player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
				/*player.itemTime++;
				player.itemAnimation++;*/
				if (!player.ItemTimeIsZero) player.itemTime = player.itemTimeMax;
				player.itemAnimation = player.itemAnimationMax;
			}

			player.itemLocation += new Vector2(-player.direction * 16, 8 - Item.height / 2);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < player.GetModPlayer<PolaritiesPlayer>().orbMinionSlots; i++)
			{
				Main.projectile[Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI)].originalDamage = damage;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

		public bool Override()
		{
			return true;
		}
		int timer = 0;
		public void Render(Player drawPlayer)
		{
			int textureFrames = 11;
			int frame = (timer++ / 4) % 11;

			Texture2D texture = ModContent.Request<Texture2D>("Items/Weapons/StardustPillarOrbHeld").Value;

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			if (drawPlayer.gravDir == -1)
			{
				DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y) - 200), (Rectangle?)new Rectangle(0, frame * texture.Height / textureFrames, texture.Width, texture.Height / textureFrames), Color.White, drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, 0f), drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
				//Main.playerDrawData.Add(drawData);
			}
			else
			{
				Vector2 value21 = Vector2.Zero;
				int type6 = drawPlayer.inventory[drawPlayer.selectedItem].type;
				DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y) - 200), (Rectangle?)new Rectangle(0, frame * texture.Height / textureFrames, texture.Width, texture.Height / textureFrames), Color.White, drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Height / textureFrames * 0.5f * (float)drawPlayer.direction, (float)texture.Height / textureFrames) + value21, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
				//Main.playerDrawData.Add(drawData);
			}
		}
	}

	public class StardustPillarMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Tower");
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 84;
			Projectile.height = 164;
			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 3600;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.channel || !player.active || player.dead)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 2;
			}

			int index = 0;
			int ownedProjectiles = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
				{
					ownedProjectiles++;
					if (i < Projectile.whoAmI)
					{
						index++;
					}
				}
			}

			Vector2 playerCenter = player.Center + new Vector2(player.direction * 13, -34);

			for (int dist = Main.rand.Next(1, 8); dist < (Projectile.Center - playerCenter).Length() - 4; dist += Main.rand.Next(1, 8))
			{
				Dust.NewDustPerfect(playerCenter + (Projectile.Center - playerCenter).SafeNormalize(Vector2.Zero).RotatedByRandom(8 / (Projectile.Center - playerCenter).Length()) * dist, 229, (Projectile.Center - playerCenter).SafeNormalize(Vector2.Zero) * 4, 0, Color.Transparent, 0.5f).noGravity = true;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.ai[0]++;
				if (Projectile.ai[0] == 120)
				{
					Projectile.ai[0] = 0;

					int weaverCount = 0;
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						//if it's a stardust weaver head
						if (Main.projectile[i].active && Main.projectile[i].type == ProjectileType<StardustWeaverMinion>() && Main.projectile[i].ai[0] == 0)
						{
							weaverCount++;
						}
					}

					//spawn more weavers
					if (weaverCount < player.maxMinions * ownedProjectiles)
					{
						int shot = 0;

						for (int j = 0; j < player.maxMinions + 1; j++)
						{
							if (j == 0)
							{
								//spawn head
								shot = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(10, 0).RotatedByRandom(MathHelper.TwoPi), ProjectileType<StardustWeaverMinion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
							}
							else if (j < player.maxMinions + 1)
							{
								//spawn body
								int newShot = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileType<StardustWeaverMinion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, shot + 1);
								Main.projectile[shot].ai[1] = newShot + 1;
								shot = newShot;
							}
							else
							{
								//spawn tail
								int newShot = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileType<StardustWeaverMinion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, shot + 1);
								Main.projectile[shot].ai[1] = newShot + 1;
								shot = newShot;
							}
						}
					}
				}
			}

			Projectile.ai[1]++;

			Vector2 goalPosition = player.Center + new Vector2(128 * (2 * index - ownedProjectiles + 1), -210 + 30 * (float)Math.Sin(Projectile.ai[1] / 40f));
			Projectile.velocity = (goalPosition - Projectile.Center) / 60;

			Projectile.rotation = Projectile.velocity.X * 0.04f;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPvp(Player target)
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = (SpriteEffects)1;
			}

			Color color78 = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));

			Vector2 vector96 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D26 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle19 = texture2D26.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Color alpha8 = Projectile.GetAlpha(color78);
			Vector2 origin17 = rectangle19.Size() / 2f;

			alpha8.A = (byte)(alpha8.A / 2);

			Main.spriteBatch.Draw(texture2D26, vector96, (Rectangle?)rectangle19, alpha8, Projectile.rotation, origin17, Projectile.scale, spriteEffects, 0f);

			if (Main.player[Projectile.owner].ghostFade != 0f)
			{
				float scaleFactor4 = Main.player[Projectile.owner].ghostFade * 5f;
				for (float num401 = 0f; num401 < 4f; num401 += 1f)
				{
					Main.spriteBatch.Draw(texture2D26, vector96 + Vector2.UnitY.RotatedBy(num401 * (MathHelper.Pi * 2f) / 4f) * scaleFactor4, (Rectangle?)rectangle19, alpha8 * 0.1f, Projectile.rotation, origin17, Projectile.scale, spriteEffects, 0f);
				}
			}

			return false;
		}
	}

	public class StardustWeaverMinion : ModProjectile
	{

		private int following
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}
		private int follower
		{
			get => (int)Projectile.ai[1] - 1;
			set => Projectile.ai[1] = value + 1;
		}
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Stardust Weaver");
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 22;
			Projectile.height = 22;
			//drawOffsetX = -1;

			Projectile.timeLeft = 1440;
			Projectile.alpha = 255;

			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.channel || !player.active || player.dead)
			{
				for (int i = 0; i < 4; i++) { Dust.NewDustDirect(Projectile.Center, 0, 0, 229, Alpha: 0, newColor: Color.Transparent, Scale: 1f).noGravity = true; }
				Projectile.Kill();
				return;
			}

			if (Projectile.timeLeft < 50)
			{
				Projectile.alpha += 5;
			}
			else
			{
				Projectile.alpha -= 5;
			}
			if (Projectile.alpha < 0) Projectile.alpha = 0;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			bool head = (following < 0);
			bool tail = (follower < 0);

			if (head)
			{
				Projectile.frame = 0;
			}
			else if (tail)
			{
				Projectile.frame = 2;
			}
			else
			{
				Projectile.frame = 1;
			}

			if (head)
			{

				int targetID = -1;
				Projectile.Minion_FindTargetInRange(1200, ref targetID, skipIfCannotHitWithOwnBody: false);
				if (targetID != -1)
				{
					NPC target = Main.npc[targetID];

					Vector2 targetPoint = target.Center + target.velocity * 8;

					if ((Projectile.Center - targetPoint).Length() > 64)
					{
						Vector2 velocityGoal = 18 * (targetPoint - Projectile.Center) / (targetPoint - Projectile.Center).Length();
						Projectile.velocity += (velocityGoal - Projectile.velocity) / 60;
					}
				}
				else
				{
					Vector2 targetPoint = player.Center + new Vector2(0, -240);

					Vector2 targetCenter = Vector2.Zero;
					bool targetCenterFound = false;
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						if (Main.projectile[i].type == ProjectileType<StardustPillarMinion>() && Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner)
						{
							if (!targetCenterFound || (targetCenter - Projectile.Center).Length() > (Main.projectile[i].Center - Projectile.Center).Length())
							{
								targetCenter = Main.projectile[i].Center;

								targetCenterFound = true;
							}
						}
					}
					if (targetCenterFound)
					{
						targetPoint = targetCenter + Projectile.velocity * 8;
					}

					if ((Projectile.Center - targetPoint).Length() > 64)
					{
						Vector2 velocityGoal = 18 * (targetPoint - Projectile.Center) / (targetPoint - Projectile.Center).Length();
						Projectile.velocity += (velocityGoal - Projectile.velocity) / 60;
					}
				}

				//avoid stacking
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].type == Projectile.type && Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].ai[0] == 0 && (Projectile.Center - Main.projectile[i].Center).Length() < 32)
					{
						Projectile.velocity += (Projectile.Center - Main.projectile[i].Center).SafeNormalize(Vector2.Zero) * 0.25f;
						break;
					}
				}

				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

				if (Projectile.velocity.Length() < 2 && Projectile.velocity.Length() > 0.01f)
				{
					Projectile.velocity = 2 * Projectile.velocity / Projectile.velocity.Length();
				}
			}
			else
			{
				ChainUpdate();
			}
		}

		public void ChainUpdate()
		{
			Projectile priorSegment = Main.projectile[following];
			Vector2 target = priorSegment.Center - priorSegment.width * (new Vector2(1, 0)).RotatedBy(priorSegment.rotation - MathHelper.PiOver2) / 2;
			Projectile.rotation = (target - Projectile.Center).ToRotation() + MathHelper.PiOver2;
			Vector2 targetVelocity = (target - (Projectile.Center - Projectile.position) - 11f * (target - Projectile.Center) / (target - Projectile.Center).Length() - Projectile.position);
			if (targetVelocity.Length() > priorSegment.velocity.Length())
			{
				Projectile.velocity = priorSegment.velocity.Length() * targetVelocity / targetVelocity.Length();
			}
			else
			{
				Projectile.velocity = targetVelocity;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			bool head = (following < 0);

			if (!head)
			{
				return false;
			}
			int projectileID = Projectile.whoAmI;
			while (Main.projectile[projectileID].ai[1] - 1 >= 0 && Main.projectile[projectileID].active)
			{
				if (Collision.CheckAABBvAABBCollision(Main.projectile[projectileID].position, Main.projectile[projectileID].Hitbox.Size(), targetHitbox.TopLeft(), targetHitbox.Size()))
				{
					return true;
				}
				projectileID = (int)(Main.projectile[projectileID].ai[1] - 1);
			}
			return Collision.CheckAABBvAABBCollision(Main.projectile[projectileID].position, Main.projectile[projectileID].Hitbox.Size(), targetHitbox.TopLeft(), targetHitbox.Size());
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = (SpriteEffects)1;
			}

			Color color78 = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));

			Vector2 vector96 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D26 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle19 = texture2D26.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Color alpha8 = Projectile.GetAlpha(color78);
			Vector2 origin17 = rectangle19.Size() / 2f;

			alpha8.A = (byte)(alpha8.A / 2);

			Main.spriteBatch.Draw(texture2D26, vector96, (Rectangle?)rectangle19, alpha8, Projectile.rotation, origin17, Projectile.scale, spriteEffects, 0f);

			if (Main.player[Projectile.owner].ghostFade != 0f)
			{
				float scaleFactor4 = Main.player[Projectile.owner].ghostFade * 5f;
				for (float num401 = 0f; num401 < 4f; num401 += 1f)
				{
					Main.spriteBatch.Draw(texture2D26, vector96 + Vector2.UnitY.RotatedBy(num401 * (MathHelper.Pi * 2f) / 4f) * scaleFactor4, (Rectangle?)rectangle19, alpha8 * 0.1f, Projectile.rotation, origin17, Projectile.scale, spriteEffects, 0f);
				}
			}

			return false;
		}
	}
}