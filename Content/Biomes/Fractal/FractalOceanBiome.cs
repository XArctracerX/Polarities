using Terraria;
using Terraria.ModLoader;

namespace Polarities.Content.Biomes.Fractal
{
    public class FractalOceanBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player)
        {
            var tile = player.Center.ToTileCoordinates();
            return player.InModBiome<FractalBiome>() && FractalSubworld.IsFractalOcean(tile.X, tile.Y);
        }

        public override float GetWeight(Player player)
        {
            return 0.75f;
        }

        public override string MapBackground => "Polarities/Content/Biomes/Fractal/FractalOceanMapBackground";
        public override string BackgroundPath => MapBackground;
        public override string BestiaryIcon => "Polarities/Content/Biomes/Fractal/FractalOceanBestiaryIcon";
    }
}