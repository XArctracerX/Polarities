using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Projectiles;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Summon.Minions.Hardmode
{
	public class HemorrphagicStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Hemorrphagic Staff");
			//Tooltip.SetDefault("Summons a baby Hemorrphage to protect you");
		}

		public override void SetDefaults()
		{
			Item.damage = 41;
			Item.DamageType = DamageClass.Summon;
			Item.width = 40;
			Item.height = 40;
			Item.mana = 10;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(gold:10);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.autoReuse = true;
			Item.buffType = BuffType<HemorrphminiBuff>();
			Item.shoot = ProjectileType<HemorrphminiLeg>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int baseProjectile = -1;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ProjectileType<HemorrphminiBody>())
				{
					baseProjectile = i;
					break;
				}
			}

			if (baseProjectile == -1)
			{
				baseProjectile = Projectile.NewProjectile(source, Main.MouseWorld, velocity, ProjectileType<HemorrphminiBody>(), 0, 0, player.whoAmI);
			}

			player.AddBuff(Item.buffType, 18000, true);
			Main.projectile[Projectile.NewProjectile(source, Main.projectile[baseProjectile].Center, velocity, type, damage, knockback, player.whoAmI, ai0: baseProjectile)].originalDamage = damage;
			return false;
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemType<EsophageousStaff>())
				.AddIngredient(ItemType<Materials.Hardmode.HemorrhagicFluid>(), 13)
				.AddTile(TileID.MythrilAnvil)
				.Register();
        }
    }

	public class HemorrphminiBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ProjectileType<HemorrphminiLeg>()] > 0)
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
}