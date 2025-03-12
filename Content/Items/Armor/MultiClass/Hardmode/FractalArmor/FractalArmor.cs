using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Assets;
using Polarities.Content.Items.Materials.Hardmode;
using Polarities.Content.Items.Placeable.Bars;
using Polarities.Content.Items.Placeable.Furniture.Fractal;
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

namespace Polarities.Content.Items.Armor.MultiClass.Hardmode.FractalArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class FractalBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			//DisplayName.SetDefault("Fractal Breastplate");
			//Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.33");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = 50000;
			Item.defense = 16;
			Item.rare = ItemRarityID.Pink;
		}

        public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 4 / 3f;
		}

        public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 24)
                .AddIngredient(ItemType<FractalResidue>(), 4)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Legs)]
	public class FractalGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			//DisplayName.SetDefault("Fractal Greaves");
			//Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.2");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = 50000;
			Item.defense = 10;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 6 / 5f;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 18)
                .AddIngredient(ItemType<FractalResidue>(), 4)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }
	}

	[AutoloadEquip(EquipType.Head)]
	public class FractalHelmetMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Faceplate");
			//Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.25"+"\n10% increased melee damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 50000;
			Item.defense = 24;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetDamage(DamageClass.Melee) += 0.1f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<FractalBreastplate>() && legs.type == ItemType<FractalGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10% increased melee damage"+"\nCreates a shield of fractal energy while moving";

			player.GetDamage(DamageClass.Melee) += 0.1f;
			player.GetModPlayer<PolaritiesPlayer>().fractalMeleeShield = true;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 12)
                .AddIngredient(ItemType<FractalResidue>(), 4)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }

		public bool Override()
		{
			return false;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class FractalHelmetRanger : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Visor");
			//Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.25"+"\n10% increased ranged damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 50000;
			Item.defense = 12;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetDamage(DamageClass.Ranged) += 0.1f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<FractalBreastplate>() && legs.type == ItemType<FractalGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Striking enemies with ranged attacks causes an exploding target to appear at the cursor";

			player.GetModPlayer<PolaritiesPlayer>().fractalRangerTargets = true;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 12)
                .AddIngredient(ItemType<FractalResidue>(), 4)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }

		public bool Override()
		{
			return false;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class FractalHelmetMage : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fractal Hood");
			//Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.25"+"\n10% increased magic damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 50000;
			Item.defense = 6;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetDamage(DamageClass.Magic) += 0.1f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<FractalBreastplate>() && legs.type == ItemType<FractalGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max mana by 60" + "\nConsuming mana summons fractal blades, which launch at the cursor when you stop";

			player.statManaMax2 += 60;
			player.GetModPlayer<PolaritiesPlayer>().fractalMageSwords = true;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemType<FractalBar>(), 12)
                .AddIngredient(ItemType<FractalResidue>(), 4)
                .AddTile(TileType<FractalAssemblerTile>())
                .Register();
        }

		public bool Override()
		{
			return false;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class FractalHelmetSummoner : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Mask");
			// Tooltip.SetDefault("Maximum life lost to fractalization is divided by 1.25"+"\n10% increased minion damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 50000;
			Item.defense = 4;
			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffLifeLossResistance *= 5 / 4f;
			player.GetDamage(DamageClass.Summon) += 0.1f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<FractalBreastplate>() && legs.type == ItemType<FractalGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases your max number of minions"+"\nSummons orbs around your head to damage enemies";

			player.maxMinions += 1;
			player.GetModPlayer<PolaritiesPlayer>().fractalSummonerOrbs = true;
		}

		public override void AddRecipes()
		{
            CreateRecipe()
				.AddIngredient(ItemType<FractalBar>(), 12)
				.AddIngredient(ItemType<FractalResidue>(), 4)
				.AddTile(TileType<FractalAssemblerTile>())
				.Register();
		}

		public bool Override()
		{
			return false;
		}
	}

	public class FractalSummonerOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Orb");

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

			if (index >= player.maxMinions * 2 - 1 || !player.GetModPlayer<PolaritiesPlayer>().fractalSummonerOrbs || player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier != 0 || !player.active || player.dead || (Projectile.Center - player.Center).Length() > 2000f)
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
            Projectile.Minion_FindTargetInRange(1000, ref targetID, skipIfCannotHitWithOwnBody: false);
            if (targetID != -1)
			{
				NPC target = Main.npc[targetID];

				if (Projectile.ai[0] == 1)
				{
					Vector2 goalPosition = target.Center;
					Vector2 goalVelocity = (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;

					if ((goalPosition - Projectile.Center).Length() < 16) goalVelocity = goalPosition - Projectile.Center;

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
				Vector2 goalVelocity = (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;

				if ((goalPosition - Projectile.Center).Length() < 16) goalVelocity = goalPosition - Projectile.Center;

				Projectile.velocity = goalVelocity;

				if (targetID != -1 && (goalPosition - Projectile.Center).Length() < 16 && Projectile.ai[1] >= 60)
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

	public class FractalMagicSword : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Blade");

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
						Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 24;
						Projectile.ai[0] = 1;
					}
					break;
				case 1:
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
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

	public class FractalRangerTarget : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Blast");

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

			if (Projectile.timeLeft == 32)
            {
				SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
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

	public class FractalMeleeShield : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fractal Shield");

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
			if (!player.GetModPlayer<PolaritiesPlayer>().fractalMeleeShield || player.GetModPlayer<PolaritiesPlayer>().fractalSetBonusTier != 0 || player.dead || !player.active)
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

			Projectile.ai[0] ++;
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
}