using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Assets;
using Polarities.Content.NPCs.Enemies.GraniteCaves.Hardmode;
using Polarities.Content.NPCs.Enemies.GraniteCaves.PreHardmode;
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
using Polarities.Content.NPCs.Enemies.Fractal;
using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
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

    public class KrakenBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Kraken>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SeaSerpentBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<SeaSerpent>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class LivingSpineBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<LivingSpine>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class RavenousCursedBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<RavenousCursed>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class FlowWormBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<FlowWorm>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class GraniteCrawlerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<GraniteCrawler>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class TurbulenceSparkBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<TurbulenceSpark>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SparkCrawlerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<SparkCrawler>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class ShockflakeBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Shockflake>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class SeaAnomalyBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<SeaAnomaly>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class FractalSlimeBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<FractalSlimeSmall>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class FractalFernBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<FractalFern>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class EuryopterBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Euryopter>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class DustSpriteBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<DustSprite>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class BisectorBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Bisector>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class AmphisbaenaBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Amphisbaena>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class FractalPointBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<FractalPoint>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class ChaosCrawlerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<ChaosCrawler>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class OrthoconicBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<Orthoconic>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class MegaMengerBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<MegaMenger>();
        protected override Color MapColor => Color.SandyBrown;
    }

    public class FractalSpiritBannerTile : BaseBannerTile
    {
        protected override int NPC => ModContent.NPCType<FractalSpirit>();
        protected override Color MapColor => Color.SandyBrown;
    }
}