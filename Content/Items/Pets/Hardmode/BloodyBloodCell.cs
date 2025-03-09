using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Pets.Hardmode
{
	public class BloodyBloodCell : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloody Blood Cell");
			//Tooltip.SetDefault("Summons a blood spider" + "\n'Do not bend'");
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
            Item.useStyle = 1;
            Item.shoot = ProjectileType<BloodSpider>();
            Item.width = 30;
            Item.height = 36;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.buffType = BuffType<BloodSpiderBuff>();
		}

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }

    public class BloodSpiderBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Spider");
            //Description.SetDefault("AAAAAA");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BloodSpider>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<BloodSpider>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }

    public class BloodSpider : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Spider");
            Main.projFrames[Projectile.type] = 7;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;

            DrawOffsetX = -1;
            DrawOriginOffsetY = -2;
            DrawOriginOffsetX = 0;

            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }

        private float goalRotation;

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 7)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }

            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;

                for (int i = 0; i < 8; i++)
                {
                    legPositions[i] = Projectile.Center;
                }
                goalRotation = Projectile.rotation;
            }

            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                player.ClearBuff(BuffType<BloodSpiderBuff>());
            }
            if (player.HasBuff(BuffType<BloodSpiderBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Vector2 goalPosition = player.MountedCenter + new Vector2(player.direction * -96, -64);
            Vector2 goalVelocity = player.velocity / 2f + (goalPosition - Projectile.Center) / 20f;
            Projectile.velocity += (goalVelocity - Projectile.velocity) / 30f;
            goalRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.Distance(player.MountedCenter) > 1000)
            {
                Projectile.Center = player.MountedCenter;
            }

            if (goalRotation > MathHelper.TwoPi) goalRotation -= MathHelper.TwoPi;
            else if (goalRotation < 0) goalRotation += MathHelper.TwoPi;
            if (Projectile.rotation > MathHelper.TwoPi) Projectile.rotation -= MathHelper.TwoPi;
            else if (Projectile.rotation < 0) Projectile.rotation += MathHelper.TwoPi;

            float angleOffset = goalRotation - Projectile.rotation;
            if (angleOffset > MathHelper.Pi) angleOffset -= MathHelper.TwoPi;
            else if (angleOffset < -MathHelper.Pi) angleOffset += MathHelper.TwoPi;

            float maxTurn = 0.05f;

            if (Math.Abs(angleOffset) < maxTurn) { Projectile.rotation = goalRotation; }
            else if (angleOffset > 0)
            {
                Projectile.rotation += maxTurn;
            }
            else
            {
                Projectile.rotation -= maxTurn;
            }

            Projectile.velocity = new Vector2(Math.Max(2f, Projectile.velocity.Length()), 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2);

            UpdateLegData();
        }

        private Vector2[] legPositions = new Vector2[8];
        private bool[] legStates = new bool[8];

        private void UpdateLegData()
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 center;
                if (i % 2 == 0)
                {
                    center = Projectile.Center + new Vector2(4, 8).RotatedBy(Projectile.rotation) + Projectile.velocity;
                }
                else
                {
                    center = Projectile.Center + new Vector2(-4, 8).RotatedBy(Projectile.rotation) + Projectile.velocity;
                }

                int xOffset = i % 2 == 0 ? 36 : -36;
                int yOffset = (i / 2) * 14 - 18;

                Vector2 legBasePosition = Projectile.Center + new Vector2(xOffset, yOffset).RotatedBy(Projectile.rotation);

                if (legStates[i]) //this is if the leg is in its stationary state
                {
                    if ((legBasePosition - legPositions[i]).Length() > 32)
                    {
                        legStates[i] = false;
                    }
                }
                else //this is if the leg is in its moving state
                {
                    if ((legBasePosition - legPositions[i]).Length() < 4)
                    {
                        legPositions[i] = legBasePosition;
                        legStates[i] = true;
                    }
                    else
                    {

                        float dRadius = (legBasePosition - center).Length() - (legPositions[i] - center).Length();
                        float dAngle = (legBasePosition - center).ToRotation() - (legPositions[i] - center).ToRotation();

                        while (dAngle > MathHelper.Pi)
                        {
                            dAngle -= MathHelper.TwoPi;
                        }
                        while (dAngle < -MathHelper.Pi)
                        {
                            dAngle += MathHelper.TwoPi;
                        }

                        legPositions[i] += new Vector2(dRadius, dAngle * (legPositions[i] - center).Length()).RotatedBy((legPositions[i] - center).ToRotation()).SafeNormalize(Vector2.Zero) * 4;

                        //legPositions[i] += (legBasePosition - legPositions[i]).SafeNormalize(Vector2.Zero) * 4;
                    }
                    legPositions[i] += Projectile.velocity;
                }

                if ((legPositions[i] - center).Length() >= 46)
                {
                    legPositions[i] = center + (legPositions[i] - center).SafeNormalize(Vector2.Zero) * 46;
                    legStates[i] = false;
                }
            }
        }

        public static Asset<Texture2D> ClawTexture;
        public static Asset<Texture2D> LegTexture;

        public override void Load()
        {
            LegTexture = Request<Texture2D>(Texture + "Leg");
            ClawTexture = Request<Texture2D>(Texture + "Claw");
        }

        public override void Unload()
        {
            LegTexture = null;
            ClawTexture = null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 center;

            for (int i = 0; i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    Vector2 drawPosition = legPositions[i] - Main.screenPosition;

                    center = Projectile.Center - Main.screenPosition + new Vector2(4, 8).RotatedBy(Projectile.rotation);

                    int segmentLength = 24;

                    float rotation = (center - drawPosition).ToRotation() + MathHelper.PiOver2 + (float)Math.Acos(((drawPosition - center) / 2).Length() / segmentLength);
                    float rotation2 = (center - drawPosition).ToRotation() + MathHelper.PiOver2 - (float)Math.Acos(((drawPosition - center) / 2).Length() / segmentLength);

                    Main.spriteBatch.Draw(ClawTexture.Value, drawPosition,
                        new Rectangle(0, 0, 20, 32), lightColor, rotation,
                        new Vector2(7, 28), 1f, SpriteEffects.FlipHorizontally, 0f);

                    Main.spriteBatch.Draw(LegTexture.Value, center,
                        new Rectangle(0, 0, 14, 30), lightColor, rotation2,
                        new Vector2(5, 5), 1f, SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    Vector2 drawPosition = legPositions[i] - Main.screenPosition;

                    center = Projectile.Center - Main.screenPosition + new Vector2(-4, 8).RotatedBy(Projectile.rotation);

                    int segmentLength = 24;

                    float rotation = (drawPosition - center).ToRotation() - MathHelper.PiOver2 - (float)Math.Acos(((drawPosition - center) / 2).Length() / segmentLength);
                    float rotation2 = (drawPosition - center).ToRotation() - MathHelper.PiOver2 + (float)Math.Acos(((drawPosition - center) / 2).Length() / segmentLength);

                    Main.spriteBatch.Draw(ClawTexture.Value, drawPosition,
                        new Rectangle(0, 0, 20, 32), lightColor, rotation,
                        new Vector2(7, 28), 1f, SpriteEffects.None, 0f);

                    Main.spriteBatch.Draw(LegTexture.Value, center,
                        new Rectangle(0, 0, 14, 30), lightColor, rotation2,
                        new Vector2(5, 5), 1f, SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}