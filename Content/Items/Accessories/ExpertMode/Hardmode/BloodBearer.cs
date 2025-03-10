using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Polarities.Content.Projectiles;
using Polarities.Content.Buffs.Hardmode;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.ExpertMode.Hardmode
{
	public class BloodBearer : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Blood Bearer");
			//Tooltip.SetDefault("Summons tentacles around the player to spew blood at enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
            Item.height = 40;

            Item.accessory = true;
            Item.value = 40000 * 5;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PolaritiesPlayer>().bloodBearer = true;
		}

		public override void UpdateInventory(Player player)
		{
            Item.rare = ItemRarityID.Expert;
		}
    }

    public class BloodBearerTentacle : ModProjectile
    {
        public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/Hemorrphage/HemorrphageTentacle";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Tentacle");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -2;

            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.timeLeft = 480;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.GetModPlayer<PolaritiesPlayer>().bloodBearer || player.dead || !player.active)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                Projectile.timeLeft = 2;
            }

            int index = 0;
            for (int i = 0; i < Projectile.whoAmI; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner)
                    index++;
            }

            int targetID = FindTarget(750f);
            NPC target = null;
            if (targetID != -1)
            {
                target = Main.npc[targetID];

                Projectile.ai[0]++;
                if (Projectile.ai[0] == 2)
                {
                    Projectile.ai[0] = 0;

                    //Projectile.NewProjectile(Projectile.Center, (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * 16, ProjectileType<FriendlyCirclingBloodShot>(), 56, 1f, Main.myPlayer);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[Projectile.owner].Center, Main.player[Projectile.owner].velocity, ProjectileType<FriendlyCirclingBloodShot>(), 30, 2, Projectile.owner, Main.rand.NextFloat(MathHelper.Pi * 2), Projectile.whoAmI);
                }
            } 
            else
            {
                Projectile.ai[0] = 0;
            }

            Vector2 goalVelocity = (player.Center + new Vector2(120, 0).RotatedBy(index*MathHelper.TwoPi/8) - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;
            Projectile.velocity += (goalVelocity - Projectile.velocity) / 10;

            Projectile.rotation = (player.Center - Projectile.Center).RotatedBy(MathHelper.PiOver2).ToRotation();

            if (Projectile.Distance(player.Center) > 750)
            {
                Projectile.Center = player.Center;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        private int FindTarget(float maxDistance = 400f)
        {
            int targetID = -1;

            Player player = Main.player[Projectile.owner];

            Vector2 center = Projectile.Center;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC nPC = Main.npc[k];
                if (nPC.CanBeChasedBy(Projectile))
                {
                    float num62 = Vector2.Distance(nPC.Center, center);
                    float angle = (nPC.Center - center).ToRotation() - (center - player.Center).ToRotation();
                    if (angle > MathHelper.Pi) angle -= MathHelper.TwoPi;

                    if (Math.Abs(angle) < MathHelper.Pi/16 && num62 < maxDistance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC.position, nPC.width, nPC.height))
                    {
                        maxDistance = num62;
                        targetID = k;
                    }
                }
            }

            return targetID;
        }

        private static Asset<Texture2D> legTexture;

        public override void Load()
        {
            legTexture = Request<Texture2D>("Polarities/Content/NPCs/Bosses/Hardmode/Hemorrphage/HemorrphageLeg_Chain2");
        }

        public override void Unload()
        {
            legTexture = null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 mainCenter = player.Center;
            Vector2 center = Projectile.Center;


            Vector2 distToNPC = mainCenter - center;

            float projRotation = distToNPC.ToRotation() + MathHelper.PiOver2;
            float distance = distToNPC.Length();

            distToNPC.Normalize();
            distToNPC *= 11f;
            center += distToNPC;

            int tries = 100;
            while (distance > 12f && !float.IsNaN(distance) && tries > 0)
            {
                tries--;
                //Draw chain
                Main.spriteBatch.Draw(legTexture.Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 18, 12), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), projRotation,
                    new Vector2(18 * 0.5f, 12 * 0.5f), 1f, SpriteEffects.None, 0f);

                distToNPC.Normalize();                 //get unit vector
                distToNPC *= 12f;                      //speed = 24
                center += distToNPC;                   //update draw position
                distToNPC = mainCenter - center;    //update distance
                distance = distToNPC.Length();
            }
            Main.spriteBatch.Draw(legTexture.Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                new Rectangle(0, 0, 18, 12), Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16)), projRotation,
                new Vector2(18 * 0.5f, 12 * 0.5f), 1f, SpriteEffects.None, 0f);

            return true;
        }
    }

    public class FriendlyCirclingBloodShot : ModProjectile
    {
        public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/Hemorrphage/BloodSpray";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Spray");

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.width = 16;
            Projectile.height = 16;
            DrawOffsetX = 3;
            DrawOriginOffsetY = -2;

            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Projectile.oldPos[k] = Projectile.position;
                }

                Projectile.localAI[0] = 1;
                Projectile.ai[0] = Main.rand.NextFloat(-1, 1) * 0.015f;
            }

            Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]) * 1.015f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HitEffect();
            return false;
        }

        private void HitEffect()
        {
            Projectile.friendly = false;
            if (Projectile.timeLeft > Projectile.oldPos.Length + 2)
            {
                Projectile.timeLeft = Projectile.oldPos.Length + 2;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        private static Asset<Texture2D> texture;

        public override void Load()
        {
            texture = Request<Texture2D>("Polarities/Content/Projectiles/CallShootProjectile");
        }

        public override void Unload()
        {
            texture = null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color mainColor = new Color(168, 0, 0);

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                if (Projectile.oldPos.Length - k < Projectile.timeLeft)
                {
                    float amount = ((Projectile.oldPos.Length - k) / Projectile.oldPos.Length);

                    Color color = mainColor * (1 - Projectile.alpha / 255f);
                    float scale = 2f * Projectile.scale * amount;

                    float rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation();

                    Main.spriteBatch.Draw(texture.Value, Projectile.Center - (Projectile.position - Projectile.oldPos[k]) * 0.9f - Main.screenPosition, new Rectangle(0, 0, 1, 1), color, rotation, new Vector2(0, 0.5f), new Vector2((Projectile.oldPos[k + 1] - Projectile.oldPos[k]).Length(), scale), SpriteEffects.None, 0f);
                }
            }

            return false;
        }
    }
}