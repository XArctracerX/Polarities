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
using Polarities.Global;
using Polarities.Content.Events;
using Polarities.Content.Items.Placeable.Relics;
using Polarities.Content.Items.Placeable.Trophies;
using Polarities.Content.Items.Placeable.MusicBoxes;
using Polarities.Content.NPCs.Enemies.WorldEvilInvasion;
using Polarities.Content.NPCs.Enemies.HallowInvasion;
using Polarities.Content.Items.Consumables.Summons.Hardmode;
using Polarities.Content.Items.Consumables.Summons.PreHardmode;
using Polarities.Content.Items.Vanity.Hardmode;
using Polarities.Content.Items.Vanity.PreHardmode;
using Polarities.Content.Items.Pets.Hardmode;
using Polarities.Content.Items.Pets.PreHardmode;
using Polarities.Content.NPCs.Bosses.PreHardmode.StormCloudfish;
using Polarities.Content.NPCs.Bosses.PreHardmode.StarConstruct;
using Polarities.Content.NPCs.Bosses.PreHardmode.Gigabat;
using Polarities.Content.NPCs.Bosses.PreHardmode.RiftDenizen;
using Polarities.Content.Items.Weapons.Ranged.Flawless;
using Polarities.Content.Items.Weapons.Melee.Flawless;
using Polarities.Content.Items.Weapons.Magic.Flawless;
using Polarities.Content.Items.Weapons.Summon.Flawless;
using Polarities.Content.NPCs.Critters.PreHardmode;
using Polarities.Content.NPCs.Critters.Hardmode;
using Polarities.Content.NPCs.Bosses.Hardmode.SunPixie;
using Polarities.Content.NPCs.Bosses.Hardmode.Esophage;
using Polarities.Content.NPCs.Bosses.Hardmode.ConvectiveWanderer;
using Polarities.Content.NPCs.Bosses.Hardmode.Eclipxie;
using Polarities.Content.NPCs.Bosses.Hardmode.Hemorrphage;
using Polarities.Content.NPCs.Bosses.Hardmode.SelfsimilarSentinel;
using Polarities.Content.NPCs.Bosses.Hardmode.MagnetonElectris;

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
		public static ModKeybind RiftDodgeHotKey { get; private set; }

		public const string CallShootProjectile = "Polarities/Content/Projectiles/CallShootProjectile";

		public override void Load()
		{
			ModUtils.Load();

			//for handling custom NPC rarities
			Terraria.ID.On_ContentSamples.FillNpcRarities += ContentSamples_FillNpcRarities;

			//music edit
			//Terraria.On_Main.UpdateAudio_DecideOnNewMusic += Main_UpdateAudio_DecideOnNewMusic;

			//IL_ResizeArrays += Polarities_IL_ResizeArrays;

			//register hotkeys
			ArmorSetBonusHotkey = KeybindLoader.RegisterKeybind(this, "Convective Set Bonus", Keys.K);
			ConvectiveDashHotkey = KeybindLoader.RegisterKeybind(this, "Convective Dash", Keys.I);
			RiftDodgeHotKey = KeybindLoader.RegisterKeybind(this, "Rift Dodge", Keys.I);

			//string texture = GetModNPC(ModContent.NPCType<StormCloudfish>()).BossHeadTexture + "_2";
			//AddBossHeadTexture(texture, -1);
			//NPCs.StormCloudfish.StormCloudfish.secondStageHeadSlot = ModContent.GetModBossHeadSlot(texture);

			//shaders
			Asset<Effect> miscEffectsRef = Assets.Request<Effect>("Global/MiscEffects", AssetRequestMode.ImmediateLoad);
			Asset<Effect> filtersRef = Assets.Request<Effect>("Global/Filters", AssetRequestMode.ImmediateLoad);

			Filters.Scene["Polarities:ScreenWarp"] = new Filter(new ScreenShaderData(filtersRef, "ScreenWarpPass"), EffectPriority.VeryHigh);
			Filters.Scene["Polarities:ScreenWarp"].Load();

			GameShaders.Misc["Polarities:TriangleFade"] = new MiscShaderData(miscEffectsRef, "TriangleFadePass"); //currently unused
			GameShaders.Misc["Polarities:WarpZoomRipple"] = new MiscShaderData(miscEffectsRef, "WarpZoomRipplePass");
			GameShaders.Misc["Polarities:EclipxieSun"] = new MiscShaderData(miscEffectsRef, "EclipxieSunPass");
			GameShaders.Misc["Polarities:RadialOverlay"] = new MiscShaderData(miscEffectsRef, "RadialOverlayPass");
			GameShaders.Misc["Polarities:DrawAsSphere"] = new MiscShaderData(miscEffectsRef, "DrawAsSpherePass");
			GameShaders.Misc["Polarities:DrawWavy"] = new MiscShaderData(miscEffectsRef, "DrawWavyPass");
		}

		public override void Unload()
		{
			ModUtils.Unload();

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

		/*private void Polarities_IL_ResizeArrays(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchCall(typeof(LoaderManager).GetMethod("ResizeArrays", BindingFlags.Static | BindingFlags.NonPublic))
			))
			{
				GetInstance<Polarities>().Logger.Debug("Failed to find patch location");
				return;
			}

			c.Emit(OpCodes.Ldarg, 0);
			c.EmitDelegate<Action<bool>>((unloading) =>
			{
				//Currently unused, anything put in here will run after ResizeArrays
				//Potentially useful for loading, to change the size of vanilla arrays or arrays from the mod
			});
		}

		//I should not need to do this in order to have something run after ResizeArrays
		private static event ILContext.Manipulator IL_ResizeArrays
		{
			add => MonoModHooks.Modify(typeof(ModContent).GetMethod("ResizeArrays", BindingFlags.NonPublic | BindingFlags.Static), value);
			remove => HookEndpointManager.Unmodify(typeof(ModContent).GetMethod("ResizeArrays", BindingFlags.NonPublic | BindingFlags.Static), value);
		}*/

		public override void PostSetupContent()
		{
			short maskIndex = (short)TextureAssets.GlowMask.Length;

			Array.Resize<Asset<Texture2D>>(ref TextureAssets.GlowMask, TextureAssets.GlowMask.Length + customTileGlowMasks.Count);

			foreach (int type in customTileGlowMasks.Keys)
			{
				Main.tileGlowMask[type] = maskIndex;
				TextureAssets.GlowMask[maskIndex] = customTileGlowMasks[type];
				maskIndex++;
			}

			//boss checklist support
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call(
					"LogEvent", 
					this, 
					"Rapture",
					11.4f,
					() => PolaritiesSystem.downedHallowInvasion,
					new List<int> { ModContent.NPCType<Pegasus>(), ModContent.NPCType<IlluminantScourer>(), ModContent.NPCType<SunServitor>(), ModContent.NPCType<Aequorean>(), ModContent.NPCType<SunKnight>(), ModContent.NPCType<Trailblazer>(), ModContent.NPCType<Painbow>() },
					new Dictionary<string,object>()
					{
						["spawnItems"] = ModContent.ItemType<HallowInvasionSummonItem>()
					}
				);
				
				bossChecklist.Call(
					"LogEvent", 
					this, 
					"Pestilence",
					11.6f,
					() => PolaritiesSystem.downedWorldEvilInvasion,
					new List<int> { ModContent.NPCType<RavenousCursed>(), ModContent.NPCType<LivingSpine>(), ModContent.NPCType<LightEater>(), ModContent.NPCType<Crimago>(), ModContent.NPCType<TendrilAmalgam>(), ModContent.NPCType<Uraraneid>() },
					new Dictionary<string,object>()
					{
						["spawnItems"] = ModContent.ItemType<WorldEvilInvasionSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<PestilenceMusicBox>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/WorldEvilInvasion").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				
				/*bossChecklist.Call(
					"AddEvent", 
					this, 
					"$Mods.Polarities.BiomeName.HallowInvasion",
					new List<int> { NPCType<Pegasus>(), NPCType<IlluminantScourer>(), NPCType<SunServitor>(), NPCType<Aequorean>(), NPCType<SunKnight>(), NPCType<Trailblazer>(), NPCType<Painbow>() }, //enemies
					11.4f, () => PolaritiesSystem.downedHallowInvasion, () => true,
					new List<int> { }, //collection
					ModContent.ItemType<HallowInvasionSummonItem>(), //spawning
					"Use a [i:" + ItemType<HallowInvasionSummonItem>() + "], or wait after defeating any mechanical boss."
				);
				
				bossChecklist.Call(
					"AddEvent", 
					this, 
					"$Mods.Polarities.BiomeName.WorldEvilInvasion",
					new List<int> { NPCType<RavenousCursed>(), NPCType<LivingSpine>(), NPCType<LightEater>(), NPCType<Crimago>(), NPCType<TendrilAmalgam>(), NPCType<Uraraneid>() }, //enemies
					11.6f, () => PolaritiesSystem.downedWorldEvilInvasion, () => true,
					new List<int> { ItemType<PestilenceMusicBox>() }, //collection
					ModContent.ItemType<WorldEvilInvasionSummonItem>(), //spawning
					"Use a [i:" + ItemType<WorldEvilInvasionSummonItem>() + "], or wait after defeating every mechanical boss.",
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/WorldEvilInvasion").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				);*/

				bossChecklist.Call(
					"LogBoss",
					this,
					"StormCloudfish",
					1.9f,
					() => PolaritiesSystem.downedStormCloudfish,
					ModContent.NPCType<StormCloudfish>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<StormCloudfishSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<StormCloudfishTrophy>(), ModContent.ItemType<StormCloudfishMask>(), ModContent.ItemType<StormCloudfishRelic>(), ModContent.ItemType<GoldfishExplorerPetItem>(), ModContent.ItemType<StormCloudfishMusicBox>(), ModContent.ItemType<StormCloudfishPetItem>(), ModContent.ItemType<EyeOfTheStormfish>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/StormCloudfish").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.StormCloudfish", NPCType<StormCloudfish>(), 1.9f, () => PolaritiesSystem.downedStormCloudfish, () => true,
					new List<int> { ItemType<StormCloudfishTrophy>(), ItemType<StormCloudfishMask>(), ItemType<StormCloudfishRelic>(), ItemType<GoldfishExplorerPetItem>(), ItemType<StormCloudfishMusicBox>(), ItemType<StormCloudfishPetItem>(), ItemType<EyeOfTheStormfish>() }, //collection
					ItemType<StormCloudfishSummonItem>(), //spawning
					"Fly a [i:" + ItemType<StormCloudfishSummonItem>() + " as high as you can at the surface.",
					null,
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/StormCloudfish").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				);*/

				bossChecklist.Call(
					"LogBoss",
					this,
					"StarConstruct",
					2.9f,
					() => PolaritiesSystem.downedStarConstruct,
					ModContent.NPCType<StarConstruct>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<StarConstructSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<StarConstructTrophy>(), ModContent.ItemType<StarConstructMask>(), ModContent.ItemType<StarConstructRelic>(), ModContent.ItemType<StarConstructPetItem>(), ModContent.ItemType<StarConstructMusicBox>(), ModContent.ItemType<Stardance>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/StarConstruct").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.StarConstruct", NPCType<StarConstruct>(), 2.9f, () => PolaritiesSystem.downedStarConstruct, () => true,
					new List<int> { ItemType<StarConstructTrophy>(), ItemType<StarConstructMask>(), ItemType<StarConstructRelic>(), ItemType<StarConstructPetItem>(), ItemType<StarConstructMusicBox>(), ItemType<Stardance>() }, //collection
					ItemType<StarConstructSummonItem>(), //spawning
					"Wait for a dormant construct to spawn at night while the player has at least 300 maximum life, or use a [i:" + ItemType<StarConstructSummonItem>() + " at the surface.",
					null,
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/StarConstruct").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				);*/

				bossChecklist.Call(
					"LogBoss",
					this,
					"Gigabat",
					3.9f,
					() => PolaritiesSystem.downedGigabat,
					ModContent.NPCType<Gigabat>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<GigabatSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<GigabatTrophy>(), ModContent.ItemType<GigabatMask>(), ModContent.ItemType<GigabatRelic>(), ModContent.ItemType<GigabatPetItem>(), ModContent.ItemType<GigabatMusicBox>(), ModContent.ItemType<Batastrophe>() }
					}
				);
				
				bossChecklist.Call(
					"LogBoss",
					this,
					"RiftDenizen",
					6.8f,
					() => PolaritiesSystem.downedRiftDenizen,
					ModContent.NPCType<RiftDenizen>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<RiftDenizenSummon>(),
						["collectibles"] = new List<int> { ModContent.ItemType<RiftDenizenTrophy>(), ModContent.ItemType<RiftDenizenMask>(), ModContent.ItemType<RiftDenizenRelic>(), ModContent.ItemType<RiftDenizenMusicBox>(), ModContent.ItemType<BeyondBow>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/RiftDenizen").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.Gigabat", NPCType<Gigabat>(), 3.9f, () => PolaritiesSystem.downedGigabat, () => true,
					new List<int> { ItemType<GigabatTrophy>(), ItemType<GigabatMask>(), ItemType<GigabatRelic>(), ItemType<GigabatPetItem>(), ItemType<GigabatMusicBox>(), ItemType<Batastrophe>() }, //collection
					new List<int> { ItemType<AmethystGemflyItem>(), ItemType<TopazGemflyItem>(), ItemType<SapphireGemflyItem>(), ItemType<EmeraldGemflyItem>(), ItemType<RubyGemflyItem>(), ItemType<DiamondGemflyItem>(), ItemType<AmberGemflyItem>(), ItemType<GigabatSummonItem>() }, //spawning
					"Release gemflies and wait, or use a [i:" + ItemType<GigabatSummonItem>() + "], while underground."
				);*/

				bossChecklist.Call(
					"LogBoss",
					this,
					"SunPixie",
					11.41f,
					() => PolaritiesSystem.downedSunPixie,
					ModContent.NPCType<SunPixie>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<SunPixieSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<SunPixieTrophy>(), ModContent.ItemType<SunPixieMask>(), ModContent.ItemType<SunPixieRelic>(), ModContent.ItemType<SunPixiePetItem>(), ModContent.ItemType<SunPixieMusicBox>(), ModContent.ItemType<RayOfSunshine>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/SunPixie").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.SunPixie", NPCType<SunPixie>(), 11.41f, () => PolaritiesSystem.downedSunPixie, () => true,
					new List<int> { ItemType<SunPixieTrophy>(), ItemType<SunPixieMask>(), ItemType<SunPixieRelic>(), ItemType<SunPixiePetItem>(), ItemType<SunPixieMusicBox>(), ItemType<RayOfSunshine>() }, //collection
					ItemType<SunPixieSummonItem>(), //spawning
					"Reach the end of the Rapture, or use a [i:" + ItemType<SunPixieSummonItem>() + "] anywhere.",
					null,
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/SunPixie").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				);*/

				bossChecklist.Call( // Needs a custom portrait (this one's from the wiki.gg page)
					"LogBoss",
					this,
					"SelfsimilarSentinel",
					11.39f,
					() => PolaritiesSystem.downedSelfsimilarSentinel,
					ModContent.NPCType<SelfsimilarSentinel>(),
					new Dictionary<string, object>() {
						["collectibles"] = new List<int> { ItemType<SelfsimilarSentinelTrophy>(), ItemType<SelfsimilarSentinelMask>(), ItemType<SelfsimilarSentinelRelic>() }, // Missing flawless
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/SelfsimilarSentinel").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				bossChecklist.Call(
					"LogBoss",
					this,
					"Esophage",
					11.61f,
					() => PolaritiesSystem.downedEsophage,
					ModContent.NPCType<Esophage>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<EsophageSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<EsophageTrophy>(), ModContent.ItemType<EsophageMask>(), ModContent.ItemType<EsophageRelic>(), ModContent.ItemType<EsophageMusicBox>(), ModContent.ItemType<Contagun>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/Esophage").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.Esophage", NPCType<Esophage>(), 11.61f, () => PolaritiesSystem.downedEsophage, () => true,
					new List<int> { ItemType<EsophageTrophy>(), ItemType<EsophageMask>(), ItemType<EsophageRelic>(), ItemType<EsophageMusicBox>(), ItemType<Contagun>() }, //collection
					ItemType<EsophageSummonItem>(), //spawning
					"Reach the end of the Pestilence, or use a [i:" + ItemType<EsophageSummonItem>() + "] anywhere.",
					null,
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/Esophage").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
				);*/

				bossChecklist.Call(
						"LogBoss",
						this,
						"ConvectiveWanderer",
						12.99f,
						() => PolaritiesSystem.downedConvectiveWanderer,
						ModContent.NPCType<ConvectiveWanderer>(),
						new Dictionary<string, object>()
						{
							["spawnItems"] = ModContent.ItemType<ConvectiveWandererSummonItem>(),
							["collectibles"] = new List<int> { ModContent.ItemType<ConvectiveWandererTrophy>(), ModContent.ItemType<ConvectiveWandererMask>(), ModContent.ItemType<ConvectiveWandererRelic>() }, // Missing flawless
							["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
								Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/ConvectiveWanderer").Value;
								Vector2 centered = new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
								sb.Draw(texture, centered, texture.Frame(), color, 0f, texture.Size() / 2, 0.2f, SpriteEffects.None, 0f);
							}
						}
				);

				/*bossChecklist.Call("AddBoss", this, "$Mods.Polarities.NPCName.ConvectiveWanderer", NPCType<ConvectiveWanderer>(), 12.99f, () => PolaritiesSystem.downedConvectiveWanderer, () => true,
					new List<int> { ItemType<ConvectiveWandererTrophy>(), ItemType<ConvectiveWandererMask>(), ItemType<ConvectiveWandererRelic>() }, //collection
					new List<int> { ItemType<BabyWandererItem>(), ItemType<ConvectiveWandererSummonItem>() }, //spawning
					"Kill a baby wanderer, or use a [i:" + ItemType<ConvectiveWandererSummonItem>() + "], at the lava ocean.",
					null,
					(SpriteBatch sb, Rectangle rect, Color color) =>
					{
						Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/ConvectiveWanderer").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
						sb.Draw(texture, centered, texture.Frame(), color, 0f, texture.Size() / 2, 0.2f, SpriteEffects.None, 0f);
					}
				);*/

				bossChecklist.Call( // Needs a custom portrait
					"LogBoss",
					this,
					"Eclipxie",
					13.98f,
					() => PolaritiesSystem.downedEclipxie,
					ModContent.NPCType<Eclipxie>(),			
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<SunPixieSummonItem>()
					}
				);

				bossChecklist.Call( // Needs a custom portrait (this one's from the wiki.gg page)
					"LogBoss",
					this,
					"Hemorrphage",
					13.98f,
					() => PolaritiesSystem.downedHemorrphage,
					ModContent.NPCType<Hemorrphage>(),
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<EsophageSummonItem>(),
						["collectibles"] = new List<int> { ModContent.ItemType<HemorrphageTrophy>(), ModContent.ItemType<HemorrphageMask>(), ModContent.ItemType<HemorrphageRelic>(), ModContent.ItemType<BloodyBloodCell>() },
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) => {
							Texture2D texture = Request<Texture2D>("Polarities/Textures/BossChecklist/Esophage").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);

				bossChecklist.Call(
					"LogBoss",
					this,
					"MagnetonElectris",
					17.99f,
					() => PolaritiesSystem.downedPolarities,
					new List<int> { ModContent.NPCType<Magneton>(), ModContent.NPCType<Electris>() },
					new Dictionary<string, object>()
					{
						["spawnItems"] = ModContent.ItemType<MagnetonElectrisSummonItem>(),
						["displayName"] = Language.GetText("The Polarities"),
						["collectibles"] = new List<int>{ ModContent.ItemType<MagnetonMask>(), ModContent.ItemType<ElectrisMask>(), ModContent.ItemType<PolaritiesTrophy>() }, // Needs a relic/flawless/pet
						["spawnInfo"] = Language.GetText("Use a [i:Polarities/MagnetonElectrisSummonItem].") // Localisation is tougher for duos
					}
				);
			}
		}

		private void ContentSamples_FillNpcRarities(Terraria.ID.On_ContentSamples.orig_FillNpcRarities orig)
		{
			orig();
			foreach (int type in customNPCBestiaryStars.Keys)
			{           
				ContentSamples.NpcBestiaryRarityStars[type] = customNPCBestiaryStars[type];
			}
		}

		public override object Call(params object[] args)
		{
			switch (args[0])
			{
				//TODO: Check that this is up to date
				case "AbominationnClearEvents":
					//fargo's mod call for event clearing
					bool eventOccurring = PolaritiesSystem.worldEvilInvasion || PolaritiesSystem.hallowInvasion;
					bool canClearEvents = Convert.ToBoolean(args[1]);
					if (eventOccurring && canClearEvents)
					{
						PolaritiesSystem.worldEvilInvasion = false;
						PolaritiesSystem.hallowInvasion = false;
					}
					return eventOccurring;

				case "InFractalDimension":
					return FractalSubworld.Active;

				case "PestilenceActive":
					return PolaritiesSystem.worldEvilInvasion;
				case "StartPestilence":
					if (!PolaritiesSystem.worldEvilInvasion)
					{
						WorldEvilInvasion.StartInvasion();
						return true;
					}
					return false;
				case "EndPestilence":
					if (!PolaritiesSystem.worldEvilInvasion)
					{
						PolaritiesSystem.worldEvilInvasion = false;
						PolaritiesSystem.worldEvilInvasionSize = 0;
						PolaritiesSystem.worldEvilInvasionSizeStart = 0;
						return true;
					}
					return false;
				case "ActivateEsophageSpawn":
					PolaritiesSystem.esophageSpawnTimer++;
					return true;
				case "DeactivateEsophageSpawn":
					PolaritiesSystem.esophageSpawnTimer = 0;
					return true;

				case "RaptureActive":
					return PolaritiesSystem.hallowInvasion;
				case "StartRapture":
					if (!PolaritiesSystem.hallowInvasion)
					{
						HallowInvasion.StartInvasion();
						return true;
					}
					return false;
				case "EndRapture":
					if (!PolaritiesSystem.hallowInvasion)
					{
						PolaritiesSystem.hallowInvasion = false;
						PolaritiesSystem.hallowInvasionSize = 0;
						PolaritiesSystem.hallowInvasionSizeStart = 0;
						return true;
					}
					return false;
				case "ActivateSunPixieSpawn":
					PolaritiesSystem.sunPixieSpawnTimer++;
					return true;
				case "DeactivateSunPixieSpawn":
					PolaritiesSystem.sunPixieSpawnTimer = 0;
					return true;

				case "GetDowned":
					{
						switch ((string)args[1])
						{
							case "Gray":
							case "StormCloudfish":
								return PolaritiesSystem.downedStormCloudfish;

							case "StarConstruct":
								return PolaritiesSystem.downedStarConstruct;

							case "Gigabat":
								return PolaritiesSystem.downedGigabat;

							case "Fractal":
							case "Denizen":
							case "RiftDenizen":
								return PolaritiesSystem.downedRiftDenizen;

							case "Rapture":
							case "HallowInvasion":
								return PolaritiesSystem.downedHallowInvasion;

							case "SunPixie":
								return PolaritiesSystem.downedSunPixie;

							case "Pestilence":
							case "WorldEvilInvasion":
								return PolaritiesSystem.downedWorldEvilInvasion;

							case "Esophage":
								return PolaritiesSystem.downedEsophage;

							case "Sentinel":
							case "Selfsimilar":
							case "SelfsimilarSentinel":
								return PolaritiesSystem.downedSelfsimilarSentinel;

							case "ConvectiveWanderer":
							case "Convective":
							case "Wanderer":
								return PolaritiesSystem.downedConvectiveWanderer;

							case "Eclipxie":
							case "Eclipixie":
								return PolaritiesSystem.downedEclipxie;

							case "Hemorphage":
							case "Hemmorphage":
							case "Hemorrphage":
								return PolaritiesSystem.downedHemorrphage;

							case "Polarities":
							case "PolaritiesBoss":
								return PolaritiesSystem.downedPolarities;
						}
					}
					return null;
				case "SetDowned":
					{
						switch ((string)args[1])
						{
							case "Gray":
							case "StormCloudfish":
								return PolaritiesSystem.downedStormCloudfish = (bool)args[2];

							case "StarConstruct":
								return PolaritiesSystem.downedStarConstruct = (bool)args[2];

							case "Gigabat":
								return PolaritiesSystem.downedGigabat = (bool)args[2];

							case "Fractal":
							case "Denizen":
							case "RiftDenizen":
								return PolaritiesSystem.downedRiftDenizen = (bool)args[2];

							case "Rapture":
							case "HallowInvasion":
								return PolaritiesSystem.downedHallowInvasion = (bool)args[2];

							case "SunPixie":
								return PolaritiesSystem.downedSunPixie = (bool)args[2];

							case "Pestilence":
							case "WorldEvilInvasion":
								return PolaritiesSystem.downedWorldEvilInvasion = (bool)args[2];

							case "Esophage":
								return PolaritiesSystem.downedEsophage = (bool)args[2];

							case "Sentinel":
							case "Selfsimilar":
							case "SelfsimilarSentinel":
								return PolaritiesSystem.downedSelfsimilarSentinel = (bool)args[2];

							case "ConvectiveWanderer":
							case "Convective":
							case "Wanderer":
								return PolaritiesSystem.downedConvectiveWanderer = (bool)args[2];

							case "Eclipxie":
							case "Eclipixie":
								return PolaritiesSystem.downedEclipxie = (bool)args[2];

							case "Hemorphage":
							case "Hemmorphage":
							case "Hemorrphage":
								return PolaritiesSystem.downedHemorrphage = (bool)args[2];

							case "Polarities":
							case "PolaritiesBoss":
								return PolaritiesSystem.downedPolarities = (bool)args[2];
						}
					}
					return null;
			}
			return null;
		}

		internal static SoundStyle GetSounds(string name, int num, float volume = 1f, float pitch = 0f, float variance = 0f)
		{
			return new SoundStyle("Polarities/Assets/Sounds" + name, 0, num) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
		}

		internal static SoundStyle GetSound(string name, float volume = 1f, float pitch = 0f, float variance = 0f)
		{
			return new SoundStyle("Polarities/Assets/Sounds/" + name) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
		}
	}
}
