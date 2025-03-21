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

namespace Polarities.Content.Items.Weapons.Summon.Minions.Hardmode
{
	public class DendriteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Dendrite Staff");
			//Tooltip.SetDefault("Summons a mini mandelbrot");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 5;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(silver:50);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;
			Item.buffType = BuffType<DendriteStaffBuff>();
			Item.shoot = ProjectileType<DendriteStaffMinion>();
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 18000, true);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Blocks.Fractal.DendriticEnergy>(), 36)
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 10)
				.AddTile(TileType<Placeable.Furniture.Fractal.FractalAssemblerTile>())
				.Register();
		}

		/*
		public bool Override()
		{
			return false;
		}

		public void Render(Player drawPlayer)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/DendriteStaff_Mask");

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			if (drawPlayer.gravDir == -1)
			{
				DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, 0f), drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
				Main.playerDrawData.Add(drawData);
			}
			else
			{
				Vector2 value21 = Vector2.Zero;
				int type6 = drawPlayer.inventory[drawPlayer.selectedItem].type;
				DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Height * 0.5f * (float)drawPlayer.direction, (float)texture.Height) + value21, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
				Main.playerDrawData.Add(drawData);
			}
		}*/
	}

	public class DendriteStaffBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Mandelbrot");
			//Description.SetDefault("The mini mandelbrot will fight for you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<DendriteStaffMinion>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	public class DendriteStaffMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Mandelbrot");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			DrawOffsetX = -12;

			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.friendly = true;
			Projectile.tileCollide = false;

			Projectile.light = 1f;
		}

		int timer = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (player.dead)
			{
				player.ClearBuff(BuffType<DendriteStaffBuff>());
			}
			if (player.HasBuff(BuffType<DendriteStaffBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			int targetID = -1;
			Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
			NPC target = null;
			if (targetID != -1)
			{
				target = Main.npc[targetID];
			}

			int index = 0;
			int numMinions = player.ownedProjectileCounts[Projectile.type];
			if (numMinions == 0)
            {
				numMinions++;
            }
			for (int i=0; i<Projectile.whoAmI; i++)
            {
				if (Main.projectile[i].active)
                {
					if (Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
						index++;
                }
            }

			Vector2 goalPosition = player.Center;
			if (target != null) { goalPosition = target.Center; }

			goalPosition += new Vector2(0, -128).RotatedBy(2 * (index + 1) / (float)(numMinions+1) - 1);

			Projectile.velocity = (goalPosition - Projectile.Center) / 5;

			Projectile.rotation = 0;
			if (target != null)
			{
				Projectile.rotation = (target.Center-Projectile.Center).ToRotation()-MathHelper.PiOver2;

				if (timer++ % 60 == (index*60)/numMinions)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 4).RotatedBy(Projectile.rotation),ProjectileType<MandelbrotMinionLightning>(),Projectile.damage,Projectile.knockBack,Projectile.owner);
				}
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter == 3)
            {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 6;
            }
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Color mainColor = Color.White;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = mainColor * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float scale = Projectile.scale;

				float rotation = Projectile.rotation;

				Main.spriteBatch.Draw(texture, Projectile.Center + (-Projectile.position + Projectile.oldPos[k]) / 2f - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / Main.projFrames[Projectile.type], texture.Width, texture.Height / Main.projFrames[Projectile.type]), mainColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class MandelbrotMinionLightning : ModProjectile
	{
		public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mandelbrot Lightning");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.alpha = 0;
			Projectile.timeLeft = 1023;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1023;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Velocity: Vector2.Zero, Scale: 1f).noGravity = true;
			if (Projectile.timeLeft == 1023 - 32) Projectile.tileCollide = true;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.immune[Projectile.owner] = 1;
        }

        public override bool? CanCutTiles()
        {
			return false;
        }
    }
}