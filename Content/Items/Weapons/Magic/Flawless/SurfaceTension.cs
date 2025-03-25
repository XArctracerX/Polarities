using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using Polarities.Global;
using Polarities.Core;

namespace Polarities.Content.Items.Weapons.Magic.Flawless
{
	public class SurfaceTension : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults() {
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = RarityType<DukeFishronFlawlessRarity>();
			Item.UseSound = SoundID.Item85;
			Item.shoot = ProjectileType<DukeBubble>();
			Item.shootSpeed = 10f;
		}

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            //if (player.GetModPlayer<PolaritiesPlayer>().fractalManaReduction)
            //{
            //    mult = 0;
            //}
        }

        public override void UpdateInventory(Player player)
        {
            Item.autoReuse = false;
        }
    }

    public class DukeBubble : ModProjectile
	{
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        // ai0: timer
        // ai1: radius


        public override void SetStaticDefaults()
        {

        }

        public List<Vector2> bubblePoints;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            bubblePoints = new List<Vector2>();
            for (int i = 0; i < 36; i++)
            {
                bubblePoints.Add(Projectile.position + new Vector2(16, 0).RotatedBy(2 * MathHelper.Pi * i / 36));
            }
        }

        bool playedSound;
        public override void AI() {

            if (Projectile.localAI[0] == 0)
            {
                Projectile.ai[1] = 16;
                for (int i = 0; i < bubblePoints.Count; i++)
                {
                    bubblePoints[i] = Projectile.position + new Vector2(16, 0).RotatedBy(2 * MathHelper.Pi * i / bubblePoints.Count);
                }
                Projectile.localAI[0] = 1;
            }

            Projectile.frameCounter++;

            if (Main.player[Projectile.owner].channel && Projectile.localAI[0] == 1)
            {
                Projectile.timeLeft = 600;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 3, 0.05f);

                if (Projectile.velocity.Length() > 16)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 16;
                }
                Projectile.netUpdate = true;
            }
            else
            {
                Projectile.velocity *= 0.99f;
                Projectile.localAI[0] = 2;
            }

            if (Projectile.ai[0] > 1)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active) continue;
                    if (npc.boss) continue;
                    if (npc.Center.Distance(Projectile.Center) > Projectile.ai[1]) continue;
                    npc.position -= npc.velocity * npc.knockBackResist / 3f;
                }

                foreach (Projectile p in Main.projectile)
                {
                    if (!p.active) continue;
                    if (p.type == Projectile.type)
                    {
                        if (p.whoAmI == Projectile.whoAmI) continue;
                        if (p.Center.Distance(Projectile.Center) > p.ai[1] + Projectile.ai[1]) continue;

                        if (p.ModProjectile is DukeBubble other)
                        {
                            float angle1To2 = Projectile.AngleTo(p.position);
                            if (angle1To2 < 0) angle1To2 += (2 * MathHelper.Pi);
                            float turnFraction = angle1To2 / (2 * MathHelper.Pi);
                            int startIndex = (int)(turnFraction * bubblePoints.Count) + (other.bubblePoints.Count / 2);
                            startIndex %= bubblePoints.Count;
                            foreach (Vector2 bubblePoint in other.bubblePoints)
                            {
                                bubblePoints.Insert(startIndex, bubblePoint);
                            }
                            Projectile.ai[1] += p.ai[1];
                            Projectile.damage += p.damage;
                            p.Kill();
                            Projectile.timeLeft = 600;
                            Projectile.localAI[0] = 1;
                            break;
                        }
                    }
                    else if (p.friendly)
                    {
                        float abs = p.Center.Distance(Projectile.Center) - Projectile.ai[1];
                        if (abs < 0) abs *= -1;
                        if (abs < 1f) Projectile.timeLeft = 2;
                    }
                }
            }

            Projectile.friendly = Projectile.timeLeft < 2;
            if (!playedSound && Projectile.timeLeft < 2)
            {
                SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
                playedSound = true;
            }

            for (int i = 0; i < bubblePoints.Count; i++)
            {
                Vector2 targetPos = Projectile.position + new Vector2(Projectile.ai[1], 0).RotatedBy(2 * MathHelper.Pi * i / bubblePoints.Count);
                if (Projectile.wet) targetPos += Main.rand.NextVector2Circular(0.6f, 0.6f);
                bubblePoints[i] = Vector2.Lerp(bubblePoints[i], targetPos, 0.2f);
                bubblePoints[i] += Main.rand.NextVector2Circular(0.3f, 0.3f);
                bubblePoints[i] += Main.rand.NextVector2Circular(0.1f, 0.1f) * 0.01f * (600f - Projectile.timeLeft);
            }
            Projectile.ai[0]++;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 thing) {
            width = 1;
            height = 1;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private void DrawLoop(List<Vector2> list, params Color[] colors)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count; i++)
			{
                Vector2 element = list[i];
                Vector2 diff = Vector2.Zero;
                if (i == list.Count - 1) diff = list[0] - element;
                else diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                //Color color = Lighting.GetColor(element.ToTileCoordinates(), mainColor);
                Color color = ModUtils.ColorLerpCycle(i, list.Count, colors);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
			}
		}

        public override bool? Colliding(Rectangle useless, Rectangle target)
        {
            return (Projectile.Center.Distance(target.Center.ToVector2()) < Projectile.ai[1]);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLoop(bubblePoints, Color.Cyan, Color.Magenta, Color.Yellow, Color.LimeGreen);
            return false;
        }
    }
}