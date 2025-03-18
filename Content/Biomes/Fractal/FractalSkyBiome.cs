using Terraria;
using Terraria.ModLoader;

namespace Polarities.Content.Biomes.Fractal
{
    public class FractalSkyBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player)
        {
            return player.InModBiome<FractalBiome>() && player.Center.ToTileCoordinates().Y < FractalSubworld.skyHeight;
        }

        public override float GetWeight(Player player)
        {
            return 0.6f;
        }

        public override string MapBackground => "Polarities/Content/Biomes/Fractal/FractalMapBackground";
        public override string BackgroundPath => MapBackground;
        public override string BestiaryIcon => "Polarities/Content/Biomes/Fractal/FractalSkyBestiaryIcon";

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/FractalPalace");
    }
}