using Terraria;
using Terraria.ModLoader;

namespace Polarities.Content.Biomes.Fractal
{
    public class FractalWastesBiome : ModBiome
    {
        public static int TileCount;

        public override bool IsBiomeActive(Player player)
        {
            return TileCount > 150;
        }

        public override float GetWeight(Player player)
        {
            return 0.75f;
        }

        public override string MapBackground => "Polarities/Content/Biomes/Fractal/FractalMapBackground";
        public override string BackgroundPath => MapBackground;
        public override string BestiaryIcon => "Polarities/Content/Biomes/Fractal/FractalWastesBestiaryIcon";

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/FractalPalace");
    }
}