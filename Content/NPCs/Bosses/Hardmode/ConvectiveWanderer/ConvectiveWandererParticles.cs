using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Polarities.Global;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.NPCs.Bosses.Hardmode.ConvectiveWanderer
{
    public class ConvectiveWandererVortexParticle : Particle
    {
        public override string Texture => "Polarities/Assets/Glow58";

        public override void Initialize()
        {
            Color = Color.White;
            Glow = true;
            TimeLeft = 60;
        }

        public int owner = -1;
        public int projectileOwner = -1;
        public int npcOwner = -1;
        public float angling;

        public override void AI()
        {
            if (owner >= 0 && Main.npc[owner].active && Main.npc[owner].ai[0] == 4 && Main.npc[owner].ai[1] >= 150 && Main.npc[owner].ai[1] < 780)
            {
                Vector2 goalPos = new Vector2(Main.npc[owner].ai[2], Main.npc[owner].ai[3]);

                float goalRadius = Math.Max(0, 64 * (Main.npc[owner].ai[1] - 180f) / 600f);

                TimeLeft = 60;

                if ((goalPos - Position).Length() - goalRadius < Velocity.Length())
                {
                    Kill();
                    return;
                }

                Velocity = (goalPos - Position).SafeNormalize(Vector2.Zero).RotatedBy(angling) * Velocity.Length();

                Alpha = Math.Min(1, ((goalPos - Position).Length() - goalRadius) / 60f);
            }
            else
            {
                Velocity *= 0.95f;
                owner = -1;

                Scale = InitialScale * (float)(1 - Math.Pow(1 - Math.Min(1, TimeLeft / 60f), 2));
                Alpha = Math.Max(1, TimeLeft / 60f);
            }

            Rotation = Velocity.ToRotation();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            Asset<Texture2D> particleTexture = particleTextures[Type];

            Vector2 drawPosition = Position - Main.screenPosition;
            if (projectileOwner > -1) drawPosition += Main.projectile[projectileOwner].Center;
            if (npcOwner > -1) drawPosition += Main.npc[npcOwner].Center + new Vector2(32, 0).RotatedBy(Main.npc[npcOwner].rotation);

            //only draw if onscreen to reduce lag
            if (Math.Abs(drawPosition.X - Main.screenWidth / 2) < Main.screenWidth * 0.6 && Math.Abs(drawPosition.Y - Main.screenHeight / 2) < Main.screenHeight * 0.6)
            {
                Color drawColor = Glow ? Color * Alpha : Lighting.GetColor(drawPosition.ToTileCoordinates()).MultiplyRGBA(Color * Alpha);

                spritebatch.Draw(particleTexture.Value, drawPosition, particleTexture.Frame(), drawColor, Rotation, particleTexture.Size() / 2, Scale * new Vector2(Velocity.Length() / 2f * Alpha, 1), SpriteEffects.None, 0f);
            }
        }
    }

    public class ConvectiveWandererExplosionPulseParticle : Particle
    {
        public override string Texture => "Polarities/Content/NPCs/Bosses/Hardmode/ConvectiveWanderer/ConvectiveWandererHeatVortex_Pulse";

        public override void Initialize()
        {
            Color = Color.White;
            Glow = true;
            TimeLeft = 120;
        }

        public float ScaleIncrement = 12f;

        public override void AI()
        {
            Scale += ScaleIncrement / MaxTimeLeft;
            Alpha = 1 - (float)Math.Pow(1 - TimeLeft / (float)MaxTimeLeft, 2);

            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
    }
}
  