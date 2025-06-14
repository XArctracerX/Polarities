using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Assets;
using Polarities.Content.Items.Materials.Hardmode;
using Polarities.Content.Items.Placeable.Bars;
using Polarities.Content.Items.Placeable.Furniture.Fractal;
using Polarities.Content.Items.Armor.MultiClass.Hardmode.FractalArmor;
using Polarities.Content.Items.Accessories.ExpertMode.Hardmode;
using Polarities.Content.Projectiles;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Armor.MultiClass.Hardmode.SelfsimilarArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class SelfsimilarBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Breastplate");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "5% increased mining speed" + "\nMaximum life lost to fractalization is divided by 1.33" + "\nGrants resistance to up to " + (Main.expertMode ? "4 minutes" : "6 minutes") + " of fractalization");
			tooltips.Insert(3, line);
		}

        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = 200000;
			Item.defense = 18;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateEquip(Player player)
		{
			player.pickSpeed *= 0.95f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 4 / 3f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 4 / 10;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
               .AddIngredient(ItemType<SelfsimilarBar>(), 24)
               .AddIngredient(ItemType<FractalBreastplate>())
               .AddTile(TileType<FractalAssemblerTile>())
               .Register();
        }
	}

	[AutoloadEquip(EquipType.Legs)]
	public class SelfsimilarGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Greaves");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "5% increased mining speed" + "\nMaximum life lost to fractalization is divided by 1.2" + "\nGrants resistance to up to " + (Main.expertMode ? "3 minutes" : "4 minutes and 30 seconds") + " of fractalization");
			tooltips.Insert(3, line);
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = 200000;
			Item.defense = 12;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateEquip(Player player)
		{
			player.pickSpeed *= 0.95f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 6 / 5f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 3 / 10;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 18)
                .AddIngredient(ItemType<FractalGreaves>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Head)]
	public class SelfsimilarHelmetMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Faceplate");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "10% increased melee damage" + "\nMaximum life lost to fractalization is divided by 1.25" + "\nGrants resistance to up to " + (Main.expertMode ? "3 minutes" : "4 minutes and 30 seconds") + " of fractalization");
			tooltips.Insert(3, line);
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 32;
			Item.value = 200000;
			Item.defense = 24;
			Item.rare = ItemRarityID.Yellow;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetDamage(DamageClass.Melee) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 3 / 10;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<SelfsimilarBreastplate>() && legs.type == ItemType<SelfsimilarGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10 defense" + "\n10% increased melee damage" + "\nCreates a shield of selfsimilar energy while moving"+"\nHitting enemies with this shield gives you a brief period of invulerability";

			player.statDefense += 10;
			player.GetDamage(DamageClass.Melee) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalMeleeShield = true;
			player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier = 1;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 12)
                .AddIngredient(ItemType<FractalHelmetMelee>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Head)]
	public class SelfsimilarHelmetRanger : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Visor");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "10% increased ranged damage" + "\nMaximum life lost to fractalization is divided by 1.25" + "\nGrants resistance to up to " + (Main.expertMode ? "3 minutes" : "4 minutes and 30 seconds") + " of fractalization");
			tooltips.Insert(3, line);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = 200000;
			Item.defense = 15;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 3 / 10;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<SelfsimilarBreastplate>() && legs.type == ItemType<SelfsimilarGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "\n18% increased ranged critical strike chance" + "\n50% chance not to consume ammo"+ "\nStriking enemies with ranged attacks causes exploding targets to appear around the cursor";

			player.GetModPlayer<PolaritiesPlayer>().ammoChance *= 2f;
			player.GetCritChance(DamageClass.Ranged) += 18;
			player.GetModPlayer<PolaritiesPlayer>().fractalRangerTargets = true;
			player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier = 1;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 12)
                .AddIngredient(ItemType<FractalHelmetRanger>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Head)]
	public class SelfsimilarHelmetMage : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Hood");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "10% increased magic damage" + "\nMaximum life lost to fractalization is divided by 1.25" + "\nGrants resistance to up to " + (Main.expertMode ? "3 minutes" : "4 minutes and 30 seconds") + " of fractalization");
			tooltips.Insert(3, line);
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = 200000;
			Item.defense = 12;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 3 / 10;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<SelfsimilarBreastplate>() && legs.type == ItemType<SelfsimilarGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max mana by 60" + "\n10% increased magic damage and critical strike chance"+ "\nConsuming mana summons fractal blades, which launch at the cursor when you stop";

			player.statManaMax2 += 60;
			player.GetCritChance(DamageClass.Magic) += 10;
			player.GetDamage(DamageClass.Magic) += 0.10f;
			player.GetModPlayer<PolaritiesPlayer>().fractalMageSwords = true;
			player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier = 1;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 12)
                .AddIngredient(ItemType<FractalHelmetMage>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Head)]
	public class SelfsimilarHelmetSummoner : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Selfsimilar Mask");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "Tooltip1", "10% increased minion damage" + "\nMaximum life lost to fractalization is divided by 1.25" + "\nGrants resistance to up to " + (Main.expertMode ? "3 minutes" : "4 minutes and 30 seconds") + " of fractalization");
			tooltips.Insert(3, line);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = 200000;
			Item.defense = 4;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffIgnoreTicks += FractalSubworld.POST_SENTINEL_TIME * 3 / 10;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<SelfsimilarBreastplate>() && legs.type == ItemType<SelfsimilarGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "24% increased minion damage" + "\nIncreases your max number of minions" + "\nSummons orbs around your head to damage enemies";

			player.maxMinions += 2;
			player.GetDamage(DamageClass.Summon) += 0.24f;
			player.GetModPlayer<PolaritiesPlayer>().fractalSummonerOrbs = true;
			player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier = 1;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<SelfsimilarBar>(), 12)
                .AddIngredient(ItemType<FractalHelmetSummoner>())
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	public class SelfsimilarSummonerOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Selfsimilar Orb");

			Main.projFrames[Projectile.type] = 2;

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 3600;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

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

			if (index >= player.maxMinions * 2 - 1 || !player.GetModPlayer<PolaritiesPlayer>().fractalSummonerOrbs || player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier != 1 || !player.active || player.dead || (Projectile.Center - player.Center).Length() > 2000f)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 2;
			}

			Projectile.frame = (index == 0) ? 0 : 1;

			int positionXIndex = (index % 2 == 0) ? (index / 2) : -(index / 2 + 1);
			float movement = 2 * (float)Math.Atan(positionXIndex / 2f);
			Vector2 basePosition = new Vector2((int)player.Center.X, (int)player.Center.Y) + new Vector2(16 * positionXIndex, -48 * (float)Math.Exp(-movement * movement / 4f));

			int targetID = -1;
            Projectile.Minion_FindTargetInRange(1600, ref targetID, skipIfCannotHitWithOwnBody: false);
			if (targetID != -1)
			{
				NPC target = Main.npc[targetID];

				if (Projectile.ai[0] == 1)
				{
					Vector2 goalPosition = target.Center;
					Vector2 goalVelocity = (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;

					if ((goalPosition - Projectile.Center).Length() < 24) goalVelocity = goalPosition - Projectile.Center;

					Projectile.velocity = goalVelocity;
				}
			}
			else
			{
				Projectile.ai[0] = 0;
			}

			if (Projectile.ai[0] == 0)
			{
				//go to baseposition
				Vector2 goalPosition = basePosition;
				Vector2 goalVelocity = (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;

				if ((goalPosition - Projectile.Center).Length() < 24) goalVelocity = goalPosition - Projectile.Center;

				Projectile.velocity = goalVelocity;

				if (targetID != -1 && (goalPosition - Projectile.Center).Length() < 24 && Projectile.ai[1] >= 60)
				{
					Projectile.ai[0] = 1;
					Projectile.ai[1] = Main.rand.Next(0, 30);
				}
			}

			Projectile.ai[1]++;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate++;
			Projectile.ai[0] = 0;

			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedByRandom(MathHelper.TwoPi), ProjectileType<SentinelHeartWisp>(), Projectile.damage / 2, 2f, Projectile.owner);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White * (1 - i / (float)Projectile.oldPos.Length) * 0.25f, 0f, frame.Size() / 2, 1f, (SpriteEffects)0, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, 0f, frame.Size() / 2, 1f, (SpriteEffects)0, 0f);

			return false;
		}
	}

	public class SelfsimilarMeleeShield : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Selfsimilar Shield");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Melee;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.GetModPlayer<PolaritiesPlayer>().fractalMeleeShield || player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier != 1 || player.dead || !player.active)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 2;
			}

			if (Projectile.ai[0] == 0)
			{
				Projectile.rotation = player.velocity.ToRotation();
				Projectile.scale = 0;
			}

			if (player.velocity == Vector2.Zero)
			{
				Projectile.scale -= 0.05f;
				if (Projectile.scale < 0) Projectile.scale = 0;
			}
			else
			{
				if (Projectile.scale == 0)
				{
					Projectile.rotation = player.velocity.ToRotation();
				}

				Projectile.scale += 0.05f;
				if (Projectile.scale > 1) Projectile.scale = 1;

				Projectile.rotation = Utils.AngleLerp(Projectile.rotation, player.velocity.ToRotation(), 0.1f);
			}

			Projectile.Center = new Vector2((int)player.Center.X, (int)player.Center.Y);
			Projectile.velocity = player.velocity;

			Projectile.ai[0]++;

			//count down immunity cooldown
			if (Projectile.ai[1] > 0)
            {
				Projectile.ai[1]--;
            }
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
		{
			return Projectile.scale > 0.5f;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Projectile.knockBack *= Math.Min(Math.Abs(Projectile.velocity.X), 8);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate++;

			//grant immunity
			if (Projectile.ai[1] == 0)
			{
				Main.player[Projectile.owner].immune = true;
				Main.player[Projectile.owner].immuneTime = 60;

				Projectile.ai[1] = 90;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineCenter = Projectile.Center + new Vector2(24 * Projectile.scale, 0).RotatedBy(Projectile.rotation);
			Vector2 lineOffset = new Vector2(0, 24 * Projectile.scale).RotatedBy(Projectile.rotation);
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineCenter + lineOffset, lineCenter - lineOffset);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Vector2 center = frame.Size() / 2;

			float scaleMultiplier;
			for (int i = 1; i < 6; i++)
			{
				scaleMultiplier = 1 + (float)Math.Sin((Projectile.ai[0] - 2 * i) * 0.1f) * 0.2f;
				float alpha = 0.25f * (1 - i / 6f);
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(24 * Projectile.scale, 0).RotatedBy(Projectile.oldRot[i]) - Main.screenPosition, frame, Color.White * alpha, Projectile.oldRot[i], center, Projectile.scale * scaleMultiplier, (SpriteEffects)0, 0f);
			}

			scaleMultiplier = 1 + (float)Math.Sin(Projectile.ai[0] * 0.1f) * 0.1f;
            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(24 * Projectile.scale, 0).RotatedBy(Projectile.rotation) - Main.screenPosition, frame, Color.White, Projectile.rotation, center, Projectile.scale * scaleMultiplier, (SpriteEffects)0, 0f);

			return false;
		}
	}

	public class SelfsimilarMagicSword : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Selfsimilar Blade");

			Main.projFrames[Projectile.type] = 1;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			switch (Projectile.ai[0])
			{
				case 0:
					Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation() + MathHelper.PiOver4;
					if (!player.controlUseItem || !player.active || player.dead)
					{
						Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 36;
						Projectile.ai[0] = 1;

						/*for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(projectile.Center, projectile.velocity / 24f * Main.rand.NextFloat(4, 8), ProjectileType<Items.Accessories.SentinelHeartWisp>(), projectile.damage / 2, 2f, projectile.owner, ai1: Main.rand.Next(1000));
						}*/
					}
					break;
				case 1:
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

					Projectile.ai[1]++;
					if ((int)(Math.Log(Projectile.ai[1] / 10f) / Math.Log(2)) < (int)(Math.Log((Projectile.ai[1] + 1) / 10f) / Math.Log(2)))
                    {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity / 2f, ProjectileType<SelfsimilarMagicSwordMini>(), Projectile.damage / 2, 2f, Projectile.owner, ai0: Projectile.ai[1]);
                        //Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(1, 0).RotatedBy(randFloat + i * MathHelper.TwoPi / player.maxMinions), ProjectileType<GlobusCrucigerMinionBlade2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
                    }
					break;
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
		{
			return Projectile.ai[0] == 1;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White * (1 - i / (float)Projectile.oldPos.Length) * 0.25f, Projectile.oldRot[i], frame.Size() / 2, 1f, (SpriteEffects)0, 0f);
			}

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, 1f, (SpriteEffects)0, 0f);

			return false;
		}
	}

	public class SelfsimilarMagicSwordMini : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Selfsimilar Blade");

			Main.projFrames[Projectile.type] = 1;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Projectile.ai[0]++;

            int targetID = -1;
            Projectile.Minion_FindTargetInRange(750, ref targetID, skipIfCannotHitWithOwnBody: false);
            if (targetID != -1)
			{
				NPC target = Main.npc[targetID];

				Vector2 goalPosition = target.Center;
				Vector2 goalVelocity = Projectile.velocity.Length() * (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.velocity = (Projectile.velocity + 2f * (goalVelocity - Projectile.velocity) / Projectile.ai[0]).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Center - Projectile.position - Main.screenPosition, frame, Color.White * (1 - i / (float)Projectile.oldPos.Length) * 0.25f, Projectile.oldRot[i], frame.Size() / 2, 1f, (SpriteEffects)0, 0f);
			}

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, 1f, (SpriteEffects)0, 0f);

			return false;
		}
	}

	public class SelfsimilarRangerTarget : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Selfsimilar Blast");

			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.DamageType = DamageClass.Ranged;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Projectile.frame = Projectile.timeLeft >= 32 ? 0 : ((32 - Projectile.timeLeft) / 4);

			if (Projectile.timeLeft > 32)
            {
				Projectile.velocity = (Main.MouseWorld - Projectile.Center) * ((Projectile.timeLeft - 32) / 32f) / 10f;
            }
			else if (Projectile.timeLeft == 32)
			{
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

				Projectile.velocity = Vector2.Zero;
			}

			Projectile.scale = Projectile.timeLeft >= 48 ? (60 - Projectile.timeLeft) / 12f : 1f;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
		{
			return Projectile.timeLeft < 32 && Projectile.timeLeft >= 24;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft < 32 || Projectile.timeLeft >= 40 || Projectile.timeLeft % 6 < 3)
			{
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
				Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, Projectile.scale, (SpriteEffects)0, 0f);
			}

			return false;
		}
	}
}