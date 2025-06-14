using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Pets.Hardmode
{
	public class ApollonianRoe : ModItem
	{
		public override void SetStaticDefaults()
		{
            Item.ResearchUnlockCount = 1;
			//DisplayName.SetDefault("Apollonian Roe");
			//Tooltip.SetDefault("Summons a mini sea anomaly");
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useStyle = 1;
			Item.shoot = ProjectileType<MiniSeaAnomaly>();
			Item.width = 40;
			Item.height = 34;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.buffType = BuffType<MiniSeaAnomalyBuff>();
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
            return base.UseItem(player);
		}
    }

    public class MiniSeaAnomalyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mini Sea Anomaly");
            //Description.SetDefault("A mini sea anomaly is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<MiniSeaAnomaly>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), Vector2.Zero, ProjectileType<MiniSeaAnomaly>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }

    public class MiniSeaAnomaly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mini Sea Anomaly");
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.light = 2f;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        private Vector2[] segmentPositions = new Vector2[22];

        public override void AI()
        {
            //cast light ray
            DelegateMethods.v3_1 = new Vector3(1, 1, 1);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + new Vector2(0, -1).RotatedBy(Projectile.rotation) * 250, 1, DelegateMethods.CastLight);

            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                player.ClearBuff(BuffType<MiniSeaAnomalyBuff>());
            }
            if (player.HasBuff(BuffType<MiniSeaAnomalyBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Vector2 goalPosition = player.Center + (Projectile.Center + Projectile.velocity * 8 - player.Center).SafeNormalize(Vector2.One).RotatedByRandom(0.01f) * 80;
            Vector2 goalVelocity = (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 12;
            Projectile.velocity += (goalVelocity - Projectile.velocity) / 30f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.Distance(player.MountedCenter) > 1000)
            {
                Projectile.Center = player.MountedCenter;
                Projectile.velocity = player.velocity;
            }

            float rotationFade = 3f;
            float rotationAmount = 1f;

            //update segment positions
            segmentPositions[0] = Projectile.Center + Projectile.velocity - new Vector2(0, Projectile.height / 2 - 2).RotatedBy(Projectile.rotation);
            Vector2 rotationGoal = Vector2.Zero;

            for (int i = 1; i < segmentPositions.Length; i++)
            {
                if (i > 1)
                {
                    rotationGoal = ((rotationFade - 1) * rotationGoal + (segmentPositions[i - 1] - segmentPositions[i - 2])) / rotationFade;
                }

                segmentPositions[i] = segmentPositions[i - 1] + (rotationAmount * rotationGoal + (segmentPositions[i] - segmentPositions[i - 1]).SafeNormalize(Vector2.Zero)).SafeNormalize(Vector2.Zero) * 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //draw the thing
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int i = segmentPositions.Length - 1; i > 0; i--)
            {
                Vector2 drawPosition = (segmentPositions[i] + segmentPositions[i - 1]) / 2;
                float rotation = (segmentPositions[i - 1] - segmentPositions[i]).ToRotation() + MathHelper.PiOver2;

                Main.spriteBatch.Draw(texture, drawPosition - Main.screenPosition, new Rectangle(0, (i - 1) * 2, 30, 4), Color.White, rotation, new Vector2(15, 2), new Vector2(1, 1), SpriteEffects.None, 0f);
            }

            return false;
        }
    }
}