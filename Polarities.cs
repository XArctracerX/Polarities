using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;
using Polarities.Core;

namespace Polarities
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class Polarities : Mod
	{
        public static bool AprilFools => (DateTime.Now.Day == 1) && (DateTime.Now.Month == 4);
        public static bool SnakeDay => (DateTime.Now.Day == 16) && (DateTime.Now.Month == 7);

        public static Dictionary<int, int> customNPCBestiaryStars = new Dictionary<int, int>();
        public static Dictionary<int, Asset<Texture2D>> customTileGlowMasks = new Dictionary<int, Asset<Texture2D>>();
        public static Dictionary<int, Asset<Texture2D>> customNPCGlowMasks = new Dictionary<int, Asset<Texture2D>>();

        //pre-generated random data
        //the size is odd because we only ever move 4 steps along the data stream so this way we can loop 4 times without actually repeating
        public static PreGeneratedRandom preGeneratedRand = new PreGeneratedRandom(358297, 4095);

        public static ModKeybind ArmorSetBonusHotkey { get; private set; }
        public static ModKeybind ConvectiveDashHotkey { get; private set; }

        public override void Load()
        {
            //ModUtils.Load();

            //for handling custom NPC rarities
            //Terraria.ID.On_ContentSamples.FillNpcRarities += ContentSamples_FillNpcRarities;

            //music edit
            //Terraria.On_Main.UpdateAudio_DecideOnNewMusic += Main_UpdateAudio_DecideOnNewMusic;

            //IL_ResizeArrays += Polarities_IL_ResizeArrays;

            //register hotkeys
            ArmorSetBonusHotkey = KeybindLoader.RegisterKeybind(this, "Convective Set Bonus", Keys.K);
            ConvectiveDashHotkey = KeybindLoader.RegisterKeybind(this, "Convective Dash", Keys.I);

            //string texture = GetModNPC(ModContent.NPCType<StormCloudfish>()).BossHeadTexture + "_2";
            //AddBossHeadTexture(texture, -1);
            //NPCs.StormCloudfish.StormCloudfish.secondStageHeadSlot = ModContent.GetModBossHeadSlot(texture);

            //shaders
            //Ref<Effect> miscEffectsRef = new Ref<Effect>(Assets.Request<Effect>("Effects/MiscEffects", AssetRequestMode.ImmediateLoad).Value);
            //Ref<Effect> filtersRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Filters", AssetRequestMode.ImmediateLoad).Value);

            //Filters.Scene["Polarities:ScreenWarp"] = new Filter(new ScreenShaderData(filtersRef, "ScreenWarpPass"), EffectPriority.VeryHigh);
            //Filters.Scene["Polarities:ScreenWarp"].Load();

            //GameShaders.Misc["Polarities:TriangleFade"] = new MiscShaderData(miscEffectsRef, "TriangleFadePass"); //currently unused
            //GameShaders.Misc["Polarities:WarpZoomRipple"] = new MiscShaderData(miscEffectsRef, "WarpZoomRipplePass");
            //GameShaders.Misc["Polarities:EclipxieSun"] = new MiscShaderData(miscEffectsRef, "EclipxieSunPass");
            //GameShaders.Misc["Polarities:RadialOverlay"] = new MiscShaderData(miscEffectsRef, "RadialOverlayPass");
            //GameShaders.Misc["Polarities:DrawAsSphere"] = new MiscShaderData(miscEffectsRef, "DrawAsSpherePass");
            //GameShaders.Misc["Polarities:DrawWavy"] = new MiscShaderData(miscEffectsRef, "DrawWavyPass");
        }

        public override void Unload()
        {
            //ModUtils.Unload();

            customNPCBestiaryStars = null;
            customTileGlowMasks = null;
            customNPCGlowMasks = null;
            preGeneratedRand = null;

            //reset to vanilla size
            //Array.Resize<Asset<Texture2D>>(ref TextureAssets.GlowMask, Main.maxGlowMasks);

            //IL_ResizeArrays -= Polarities_IL_ResizeArrays;

            //unload hotkeys
            ArmorSetBonusHotkey = null;
            ConvectiveDashHotkey = null;
        }
    }
}
