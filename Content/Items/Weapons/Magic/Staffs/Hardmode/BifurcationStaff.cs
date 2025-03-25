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

namespace Polarities.Content.Items.Weapons.Magic.Staffs.Hardmode
{
	public class BifurcationStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bifurcator");
			//Tooltip.SetDefault("Enemy hits release additional lightning bolts which jump to other enemies within range");
			Item.staff[Item.type] = true;
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 64;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<BifurcatingLightning>();
			Item.shootSpeed = 4f;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			//if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
			//{
			//	mult = 0;
			//}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position += velocity.SafeNormalize(Vector2.Zero) * 80;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Placeable.Bars.FractalBar>(), 30)
				.AddIngredient(ItemType<Materials.Hardmode.FractalResidue>(), 6)
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
			Texture2D texture = Request("Polarities/Content/Items/Weapons/BifurcationStaff_Mask");

			SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.gravDir != 1f) ? ((drawPlayer.direction != 1) ? 3 : 2) : ((drawPlayer.direction != 1) ? 1 : 0));

			float num79 = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
			int num78 = 0;
			int num77 = 0;
			Vector2 origin5 = new Vector2(0f, texture.Height);
			if (drawPlayer.gravDir == -1f)
			{
				if (drawPlayer.direction == -1)
				{
					num79 += 1.57f;
					origin5 = new Vector2(texture.Width, 0f);
					num78 -= texture.Width;
				}
				else
				{
					num79 -= 1.57f;
					origin5 = Vector2.Zero;
				}
			}
			else if (drawPlayer.direction == -1)
			{
				origin5 = new Vector2(texture.Width, texture.Height);
				num78 -= texture.Width;
			}
			Vector2 holdoutOrigin = Vector2.Zero;
			ItemLoader.HoldoutOrigin(drawPlayer, ref holdoutOrigin);
			DrawData drawData = new DrawData(texture, new Vector2((float)(int)(drawPlayer.itemLocation.X - Main.screenPosition.X + origin5.X + (float)num78), (float)(int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + (float)num77)), (Rectangle?)new Rectangle(0, 0, texture.Width, texture.Height), Color.White, num79, origin5 + holdoutOrigin, drawPlayer.inventory[drawPlayer.selectedItem].scale, spriteEffects, 0);
			Main.playerDrawData.Add(drawData);
		}*/
	}


    public class BifurcatingLightning : ModProjectile
    {
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning Arc");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1023;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1023;
            Projectile.tileCollide = true;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Velocity: Vector2.Zero, Scale: 1f).noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
		{
			if (Projectile.damage > 1)
			{
				float length = (float)Math.Pow(2, 9 - Projectile.ai[0] / 2);
				NPC[] outNPCs = { null, null };
				for (int targetNPC = 0; targetNPC < Main.npc.Length; targetNPC++)
				{
					NPC npc = Main.npc[targetNPC];
					if (npc.CanBeChasedBy(Projectile) && npc.immune[Projectile.owner] == 0)
					{
						if ((npc.Center - Projectile.Center).Length() < length)
						{
							length = (npc.Center - Projectile.Center).Length();

							outNPCs[1] = outNPCs[0];
							outNPCs[0] = npc;
						}
					}
				}
				for (int i = 0; i < 2; i++)
				{
					if (outNPCs[i] != null)
					{
						Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, (outNPCs[i].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 4, Projectile.type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, Projectile.ai[0] + 1);
						p.timeLeft = (int)Math.Pow(2, 7 - Projectile.ai[0] / 2);
						p.velocity = p.velocity.Length() * p.DirectionTo(outNPCs[i].Center);
					}
				}
			}
		}
    }
}