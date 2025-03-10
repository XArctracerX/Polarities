using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Assets;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Summon.Orbs.PreHardmode
{
	public class ObserverOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Observer Orb");
			//Tooltip.SetDefault("Summons a channeled minion" + "\nSummons a fractal rift to watch your enemies, granting minions a 25% chance to critically strike enemies within its range");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Summon;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ProjectileType<ObserverOrbMinion>();
			Item.shootSpeed = 0f;
		}

		public override void HoldItem(Player player)
		{
			if (player.channel)
			{
				player.direction = (Main.MouseWorld.X - player.Center.X > 0) ? 1 : -1;
				player.itemTime++;
				player.itemAnimation++;
			}

			player.itemLocation += new Vector2(-player.direction * 8, 12 - Item.height / 2);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < player.GetModPlayer<PolaritiesPlayer>().orbMinionSlots; i++)
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[proj].originalDamage = damage;
            }
            return false;
        }
    }

    public class Observed : ModBuff
    {
        public override string Texture => "Polarities/Content/Buffs/PreHardmode/Pinpointed";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Observed");
            // Description.SetDefault("Totally not a pinpointed clone");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }

    public class ObserverOrbMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Observer Rift");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 122;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
        }

        public override void AI()
        {
            Projectile.damage = 1;

            Player player = Main.player[Projectile.owner];

            Projectile.spriteDirection = (player.Center.X > Projectile.Center.X ? 1 : -1) * (player.Center.Y > Projectile.Center.Y ? 1 : -1);
            Projectile.rotation = player.Center.Y > Projectile.Center.Y ? 0 : MathHelper.Pi;

            if (!player.channel || !player.active || player.dead || Projectile.timeLeft < 16)
            {
                return;
            }
            else
            {
                Projectile.timeLeft = 17;
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

            Projectile.localAI[0]++;
        }

        public override bool? CanHitNPC(NPC target)
        {
            float circleRadius = (float)Math.Sqrt(Projectile.localAI[0]) * 20f * Math.Min(1, Math.Max(0, Projectile.timeLeft / 16f));

            if ((target.Center - Projectile.Center).Length() < circleRadius)
            {
                target.buffImmune[BuffType<Observed>()] = false;
                target.AddBuff(BuffType<Observed>(), 2, true);
                target.GetGlobalNPC<PolaritiesNPC>().observers++;
            }
            return false;
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
            Rectangle frame = texture.Frame();

            float xScale = Math.Min(1, Math.Max(1 / 8f, Math.Min((Projectile.localAI[0] - 8) / 8f, (Projectile.timeLeft - 8) / 8f)));
            float yScale = Math.Min(1, Math.Max(0, Math.Min(Projectile.localAI[0] / 8f, Projectile.timeLeft / 8f)));

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() / 2, new Vector2(xScale, yScale), Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            float circleRadius = (float)Math.Sqrt(Projectile.localAI[0]) * 20f * Math.Min(1, Math.Max(0, Projectile.timeLeft / 16f));

            texture = ModContent.Request<Texture2D>("Polarities/Content/Projectiles/CallShootProjectile").Value;
            frame = new Rectangle(0, 0, 1, 1);

            for (int i = 0; i < circleRadius * MathHelper.TwoPi; i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(circleRadius, 0).RotatedBy(i / circleRadius), frame, Color.White, i / circleRadius, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
            }

            return false;
        }
    }
}