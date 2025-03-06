using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Polarities.Content.NPCs.Enemies.Fractal;
//using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
using Polarities.Core;
using Polarities.Global;
using Polarities.Assets;
using Polarities.Content.NPCs.Enemies.GraniteCaves.Hardmode;
using Polarities.Content.NPCs.Enemies.HallowInvasion;
using Polarities.Content.NPCs.Enemies.LavaOcean;
using Polarities.Content.NPCs.Enemies.LimestoneCaves.Hardmode;
using Polarities.Content.NPCs.Enemies.LimestoneCaves.PreHardmode;
using Polarities.Content.NPCs.Enemies.MarbleCaves.Hardmode;
using Polarities.Content.NPCs.Enemies.SaltCaves.Hardmode;
using Polarities.Content.NPCs.Enemies.SaltCaves.PreHardmode;
using Polarities.Content.NPCs.Enemies.WorldEvilInvasion;
using Polarities.Content.NPCs.Enemies.Desert.PreHardmode;
using Polarities.Content.NPCs.Enemies.Desert.Hardmode;
using Polarities.Content.NPCs.Enemies.Surface.PreHardmode;
using Polarities.Content.NPCs.Enemies.Surface.Hardmode;
using Polarities.Content.NPCs.Enemies.Underground.PreHardmode;
using Polarities.Content.NPCs.Enemies.Ocean.Hardmode;
using Polarities.Content.NPCs.Enemies.BloodMoon.PreHardmode;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Placeable.Banners.Tiles
{
    public abstract class BaseBannerTile : ModTile
    {
        protected abstract int NPC { get; }
        protected abstract Color MapColor { get; }

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Banners, 0));
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(MapColor, Language.GetText("MapObject.Banner"));
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            Tile tile = Main.tile[i, j];
            TileObjectData data = TileObjectData.GetTileData(tile);
            int topLeftX = i - tile.TileFrameX / 18 % data.Width;
            int topLeftY = j - tile.TileFrameY / 18 % data.Height;
            if (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY))
            {
                offsetY -= 8;
            }
        }

        public override bool CreateDust(int i, int j, ref int type) => false;

        public virtual void SafeNearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.hasBanner = true;
                Main.SceneMetrics.NPCBannerBuff[NPC] = true;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            SafeNearbyEffects(i, j, closer);
        }
    }

    public class AequoreanBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Aequorean>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SpitterBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Spitter>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class RattlerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Rattler>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class BrineFlyBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<BrineFly>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SalthopperBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Salthopper>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class MusselBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Mussel>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class InfernalArrowBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<InfernalArrow>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class GraniteStomperBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<GraniteStomper>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class ZombatBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Zombat>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class BloodBatBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<BloodBat>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class StalagBeetleBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<StalagBeetle>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class AlkaliSpiritBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<AlkaliSpirit>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class LimeshellBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Limeshell>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class ConeShellBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<ConeShell>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class StellatedSlimeBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<StellatedSlime>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class BatSlimeBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<BatSlime>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class NestGuardianBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<NestGuardian>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class AlkalabominationBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Alkalabomination>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class LightEaterBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<LightEater>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class TendrilAmalgamBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<TendrilAmalgam>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class CrimagoBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Crimago>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class UraraneidBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Uraraneid>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class HydraBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<HydraBody>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class GlowWormBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<GlowWorm>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class IlluminantScourerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<IlluminantScourer>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class PegasusBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Pegasus>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SunServitorBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<SunServitor>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SunKnightBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<SunKnight>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class TrailblazerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Trailblazer>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class PainbowBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Painbow>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class BrineDwellerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<BrineDweller>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SlimeyBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Slimey>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class MantleOWarBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<MantleOWar>();
        protected override Color MapColor => Color.SandyBrown;
    }

    //public class TurbulenceSparkBanner : BannerBase { public override int BannerIndex => 42; public override int NPCType => NPCType<TurbulenceSpark>(); }
    //public class SparkCrawlerBanner : BannerBase { public override int BannerIndex => 15; public override int NPCType => NPCType<SparkCrawler>(); }
    //public class ShockflakeBanner : BannerBase { public override int BannerIndex => 36; public override int NPCType => NPCType<Shockflake>(); }
    //public class SeaAnomalyBanner : BannerBase { public override int BannerIndex => 39; public override int NPCType => NPCType<SeaAnomaly>(); }
    //public class OrthoconicBanner : BannerBase { public override int BannerIndex => 32; public override int NPCType => NPCType<Orthoconic>(); }
    //public class MegaMengerBanner : BannerBase { public override int BannerIndex => 21; public override int NPCType => NPCType<MegaMenger>(); }
    //public class FractalSpiritBanner : BannerBase { public override int BannerIndex => 23; public override int NPCType => NPCType<FractalSpirit>(); }
    //public class FractalSlimeBanner : BannerBase { public override int BannerIndex => 40; public override int NPCType => NPCType<FractalSlimeSmall>(); }
    //public class FractalPointBanner : BannerBase { public override int BannerIndex => 25; public override int NPCType => NPCType<FractalPoint>(); }
    //public class FractalFernBanner : BannerBase { public override int BannerIndex => 16; public override int NPCType => NPCType<FractalFern>(); }
    //public class EuryopterBanner : BannerBase { public override int BannerIndex => 17; public override int NPCType => NPCType<Euryopter>(); }
    //public class DustSpriteBanner : BannerBase { public override int BannerIndex => 41; public override int NPCType => NPCType<DustSprite>(); }
    //public class ChaosCrawlerBanner : BannerBase { public override int BannerIndex => 38; public override int NPCType => NPCType<ChaosCrawler>(); }
    //public class BisectorBanner : BannerBase { public override int BannerIndex => 37; public override int NPCType => NPCType<BisectorHead>(); }
    //public class AmphisbaenaBanner : BannerBase { public override int BannerIndex => 24; public override int NPCType => NPCType<Amphisbaena>(); }
    //public class KrakenBanner : BannerBase { public override int BannerIndex => 13; public override int NPCType => NPCType<Kraken>(); }
    //public class SeaSerpentBanner : BannerBase { public override int BannerIndex => 14; public override int NPCType => NPCType<SeaSerpent>(); }
    //public class LivingSpineBanner : BannerBase { public override int BannerIndex => 30; public override int NPCType => NPCType<LivingSpine>(); }
    //public class RavenousCursedBanner : BannerBase { public override int BannerIndex => 31; public override int NPCType => NPCType<RavenousCursed>(); }
    //public class FlowWormBanner : BannerBase { public override int BannerIndex => 34; public override int NPCType => NPCType<FlowWorm>(); }
    //public class GraniteCrawlerBanner : BannerBase { public override int BannerIndex => 51; public override int NPCType => NPCType<GraniteCrawler>(); }

}