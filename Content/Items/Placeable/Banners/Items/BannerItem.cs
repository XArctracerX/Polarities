using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Content.Items.Placeable.Banners.Tiles;

namespace Polarities.Content.Items.Placeable.Banners.Items
{
    public abstract class BaseBannerItem : ModItem
    {
        protected abstract int Tile { get; }
        public sealed override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(Tile, 0);
            Item.width = 12;
            Item.height = 28;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
        }
    }

    public class AequoreanBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<AequoreanBannerTile>();
    }

    public class SpitterBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SpitterBannerTile>();
    }

    public class RattlerBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<RattlerBannerTile>();
    }

    public class BrineFlyBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<BrineFlyBannerTile>();
        public override void SetStaticDefaults() => ItemID.Sets.KillsToBanner[Type] = 200;
    }

    public class SalthopperBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SalthopperBannerTile>();
    }

    public class MusselBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<MusselBannerTile>();
    }

    public class InfernalArrowBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<InfernalArrowBannerTile>();
    }

    public class GraniteStomperBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<GraniteStomperBannerTile>();
    }

    public class ZombatBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<ZombatBannerTile>();
    }

    public class BloodBatBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<BloodBatBannerTile>();
    }

    public class StalagBeetleBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<StalagBeetleBannerTile>();
    }

    public class AlkaliSpiritBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<AlkaliSpiritBannerTile>();
    }

    public class LimeshellBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<LimeshellBannerTile>();
    }

    public class ConeShellBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<ConeShellBannerTile>();
    }

    public class StellatedSlimeBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<StellatedSlimeBannerTile>();
    }

    public class BatSlimeBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<BatSlimeBannerTile>();
    }

    public class NestGuardianBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<NestGuardianBannerTile>();
    }

    public class AlkalabominationBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<AlkalabominationBannerTile>();
    }

    public class LightEaterBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<LightEaterBannerTile>();
    }

    public class TendrilAmalgamBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<TendrilAmalgamBannerTile>();
    }

    public class CrimagoBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<CrimagoBannerTile>();
    }

    public class UraraneidBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<UraraneidBannerTile>();
    }

    public class HydraBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<HydraBannerTile>();
    }

    public class GlowWormBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<GlowWormBannerTile>();
    }

    public class IlluminantScourerBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<IlluminantScourerBannerTile>();
    }

    public class PegasusBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<PegasusBannerTile>();
    }

    public class SunServitorBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SunServitorBannerTile>();
    }

    public class SunKnightBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SunKnightBannerTile>();
    }

    public class TrailblazerBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<TrailblazerBannerTile>();
    }

    public class PainbowBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<PainbowBannerTile>();
    }

    public class BrineDwellerBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<BrineDwellerBannerTile>();
    }

    public class SlimeyBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SlimeyBannerTile>();
    }

    public class MantleOWarBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<MantleOWarBannerTile>();
    }

    public class KrakenBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<KrakenBannerTile>();
    }

    public class SeaSerpentBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<SeaSerpentBannerTile>();
    }

    public class LivingSpineBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<LivingSpineBannerTile>();
    }

    public class RavenousCursedBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<RavenousCursedBannerTile>();
    }

    public class FlowWormBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<FlowWormBannerTile>();
    }

    public class GraniteCrawlerBanner : BaseBannerItem
    {
        protected override int Tile => ModContent.TileType<GraniteCrawlerBannerTile>();
    }
}