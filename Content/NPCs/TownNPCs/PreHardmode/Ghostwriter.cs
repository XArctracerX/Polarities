using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Polarities.Global;
using Polarities.Core;
using Polarities.Content.Items.Accessories.Books.Hardmode;
using Polarities.Content.Items.Tools.Books.PreHardmode;
using Polarities.Content.Items.Tools.Books.Hardmode;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.NPCs.TownNPCs.PreHardmode
{
    [AutoloadHead]
    //TODO: Better party drawing
    public class Ghostwriter : ModNPC
    {
        public const string ShopName = "Shop";
        public override void Load()
        {
            Terraria.GameContent.Personalities.On_AllPersonalitiesModifier.ModifyShopPrice_Relationships += AllPersonalitiesModifier_ModifyShopPrice_Relationships;

            Terraria.IL_NPC.checkDead += NPC_checkDead;
        }

        //makes them leave rather than die
        private void NPC_checkDead(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            ILLabel label = null;

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdarg(0),
                i => i.MatchLdfld(typeof(NPC).GetField("type", BindingFlags.Public | BindingFlags.Instance)),
                i => i.MatchLdcI4(369),
                i => i.MatchBeq(out label)
                ))
            {
                GetInstance<Polarities>().Logger.Debug("Failed to find patch location");
                return;
            }

            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(NPC).GetField("type", BindingFlags.Public | BindingFlags.Instance));
            c.EmitDelegate<Func<int>>(() => { return NPCType<Ghostwriter>(); });
            c.Emit(OpCodes.Beq, label);
        }

        //makes them not like the princess
        private void AllPersonalitiesModifier_ModifyShopPrice_Relationships(Terraria.GameContent.Personalities.On_AllPersonalitiesModifier.orig_ModifyShopPrice_Relationships orig, HelperInfo info, Terraria.GameContent.ShopHelper shopHelperInstance)
        {
            if (info.npc.type == NPCType<Ghostwriter>())
            {
                if (info.nearbyNPCsByType[663])
                {
                    shopHelperInstance.AddHappinessReportText("MehAboutPrincess", new { NPCName = NPC.GetFullnameByID(NPCID.Princess) });
                }
                return;
            }
            orig(info, shopHelperInstance);
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = -1; //No attack
            NPCID.Sets.AttackTime[Type] = 0; //If we do somehow attack it takes no time
            NPCID.Sets.AttackAverageChance[Type] = int.MaxValue; //Just don't
            NPCID.Sets.HatOffsetY[Type] = 4; //Party hat Y offset

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Clothier, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Hate)
            ;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.knockBackResist = 0.5f;
            NPC.alpha = 64;

            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				//TODO: Like either books or graveyard instead of this
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                this.TranslatedBestiaryEntry()
            });
        }

        public static bool SpawnCondition()
        {
            return NPC.downedSlimeKing || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || NPC.downedDeerclops || Main.hardMode || PolaritiesSystem.downedStormCloudfish || PolaritiesSystem.downedStarConstruct || PolaritiesSystem.downedGigabat || PolaritiesSystem.downedRiftDenizen;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return SpawnCondition();
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>
            {
                "Anne",
                "Mary",
                "Shelley",
                "Jane",
                "Harper",
                "Terry",
                "Ursula"
            };
        }

        public override string GetChat()
        {
            string baseDialogueString = "Mods.Polarities.NPCs.TownNPCDialogue.Ghostwriter.";

            if (NPC.homeless)
            {
                return Language.GetTextValue(baseDialogueString + "NoHome");
            }

            WeightedRandom<(string, object)> chat = new WeightedRandom<(string, object)>();

            chat.Add((baseDialogueString + "NoHome", null));
            chat.Add((baseDialogueString + "Generic0", null));
            chat.Add((baseDialogueString + "Generic1", null));
            chat.Add((baseDialogueString + "Generic2", null));
            chat.Add((baseDialogueString + "Generic3", null));
            if (NPC.FindFirstNPC(NPCID.Angler) >= 0)
            {
                chat.Add((baseDialogueString + "Angler", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Angler)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Stylist) >= 0)
            {
                chat.Add((baseDialogueString + "Stylist", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Stylist)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.ArmsDealer) >= 0)
            {
                chat.Add((baseDialogueString + "ArmsDealer", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.ArmsDealer)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.DD2Bartender) >= 0)
            {
                chat.Add((baseDialogueString + "DD2Bartender", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.DD2Bartender)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Clothier) >= 0)
            {
                chat.Add((baseDialogueString + "Clothier", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Clothier)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Mechanic) >= 0)
            {
                chat.Add((baseDialogueString + "Mechanic", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Mechanic)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.SantaClaus) >= 0)
            {
                chat.Add((baseDialogueString + "SantaClaus", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.SantaClaus)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Wizard) >= 0)
            {
                chat.Add((baseDialogueString + "Wizard", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Wizard)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Guide) >= 0 && Main.hardMode)
            {
                chat.Add((baseDialogueString + "GuideHardmode", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Guide)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Dryad) >= 0 && Main.hardMode)
            {
                chat.Add((baseDialogueString + "DryadHardmode", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Dryad)].GivenName }));
            }
            if (NPC.FindFirstNPC(NPCID.Princess) >= 0)
            {
                chat.Add((baseDialogueString + "Princess", new { NPCName = Main.npc[NPC.FindFirstNPC(NPCID.Princess)].GivenName }));
            }
            if (Main.halloween)
            {
                chat.Add((baseDialogueString + "Halloween", null));
            }
            if (Main.xMas)
            {
                chat.Add((baseDialogueString + "XMas", null));
            }
            if (NPC.downedBoss1)
            {
                chat.Add((baseDialogueString + "EyeOfCthulhu", null));
            }
            if (NPC.downedBoss3)
            {
                chat.Add((baseDialogueString + "Skeletron", null));
            }
            if (PolaritiesSystem.downedRiftDenizen)
            {
                chat.Add((baseDialogueString + "RiftDenizen", null));
            }
            if (NPC.downedMechBoss2)
            {
                chat.Add((baseDialogueString + "Twins", null));
            }
            if (NPC.downedMechBoss3)
            {
                chat.Add((baseDialogueString + "Destroyer", null));
            }
            if (Main.bloodMoon)
            {
                chat.Add((baseDialogueString + "BloodMoon", null));
            }
            if (Main.pumpkinMoon)
            {
                chat.Add((baseDialogueString + "PumpkinMoon", null));
            }
            if (Main.snowMoon)
            {
                chat.Add((baseDialogueString + "FrostMoon", null));
            }
            if (Main.LocalPlayer.ZoneGraveyard)
            {
                if (Main.LocalPlayer.difficulty == 2) chat.Add((baseDialogueString + "GraveyardHardcore", null));
                else chat.Add((baseDialogueString + "Graveyard", null));
            }
            if (Main.LocalPlayer.ZoneRain)
            {
                chat.Add((baseDialogueString + "Rain", null));
            }
            else if (Main.IsItAHappyWindyDay)
            {
                chat.Add((baseDialogueString + "WindyDay", null));
            }
            //TODO: Maybe some dialogue for certain biomes?
            //TODO: Fractal dimension dialogue that points out how they really shouldn't be there

            (string, object) output = chat;
            return Language.GetTextValueWith(output.Item1, output.Item2);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return !toKingStatue;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName;
            }
        }

        public override void AddShops()
        {
            // all modded ones will need an aditional conditions file to work
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Book)
                .Add<StrangeObituary>(Condition.InGraveyard)
                .Add<ReadingGlasses>(Condition.Hardmode)
                .Add<KingSlimeBook>(Condition.DownedKingSlime)
                .Add<StormCloudfishBook>(PolaritiesConditions.DownedStormCloudfish)
                .Add<EyeOfCthulhuBook>(Condition.DownedEyeOfCthulhu)
                .Add<StarConstructBook>(PolaritiesConditions.DownedStarConstruct)
                .Add<EaterOfWorldsBook>(Condition.DownedEaterOfWorlds)
                .Add<BrainOfCthulhuBook>(Condition.DownedBrainOfCthulhu)
                .Add<GigabatBook>(PolaritiesConditions.DownedGigabat)
                .Add<QueenBeeBook>(Condition.DownedQueenBee)
                .Add<SkeletronBook>(Condition.DownedSkeletron)
                .Add<DeerclopsBook>(Condition.DownedDeerclops)
                // .Add<RiftDenizenBook>(Condition.downedRiftDenizen)
                .Add<WallOfFleshBook>(Condition.Hardmode)
                .Add<QueenSlimeBook>(Condition.DownedQueenSlime)
                .Add<DestroyerBook>(Condition.DownedDestroyer)
                .Add<TwinsBook>(Condition.DownedTwins)
                .Add<SkeletronPrimeBook>(Condition.DownedSkeletronPrime)
                .Add<SunPixieBook>(PolaritiesConditions.DownedSunPixie)
                .Add<EsophageBook>(PolaritiesConditions.DownedEsophage)
                .Add<PlanteraBook>(Condition.DownedPlantera)
                .Add<ConvectiveWandererBook>(PolaritiesConditions.DownedConvectiveWanderer)
                // .Add<SelfsimilarSentinelBook>(Condition.downedSelfsimilarSentinel)
                .Add<GolemBook>(Condition.DownedGolem)
                .Add<DukeFishronBook>(Condition.DownedDukeFishron)
                .Add<BetsyBook>(Condition.DownedOldOnesArmyT3)
                .Add<EmpressOfLightBook>(Condition.DownedEmpressOfLight)
                // .Add<EclipxieBook>(Condition.downedEclipxie)
                // .Add<HemorrphageBook>(Condition.downedHemorrphage)
                .Add<LunaticCultistBook>(Condition.DownedCultist)
                // .Add<PolaritiesBook>(Condition.downedPolarities)
                .Add<MoonLordBook>(Condition.DownedMoonLord);

            npcShop.Register();
        }

        public override void FindFrame(int frameHeight)
        {
            int num = frameHeight;

            int num173 = NPCID.Sets.ExtraFramesCount[Type];
            if (NPC.velocity.Y < 2)
            {
                if (NPC.direction == 1)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = -1;
                }
                int num174 = Main.npcFrameCount[Type] - NPCID.Sets.AttackFrameCount[Type];
                if (NPC.ai[0] == 23f)
                {
                    NPC.frameCounter += 1.0;
                    int num175 = NPC.frame.Y / num;
                    int num182 = num174 - num175;
                    if ((uint)(num182 - 1) > 1u && (uint)(num182 - 4) > 1u && num175 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num190 = ((!(NPC.frameCounter < 6.0)) ? (num174 - 4) : (num174 - 5));
                    if (NPC.ai[1] < 6f)
                    {
                        num190 = num174 - 5;
                    }
                    NPC.frame.Y = num * num190;
                }
                else if (NPC.ai[0] >= 20f && NPC.ai[0] <= 22f)
                {
                    int num191 = NPC.frame.Y / num;
                    NPC.frame.Y = num191 * num;
                }
                else if (NPC.ai[0] == 2f)
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frame.Y / num == num174 - 1 && NPC.frameCounter >= 5.0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    else if (NPC.frame.Y / num == 0 && NPC.frameCounter >= 40.0)
                    {
                        NPC.frame.Y = num * (num174 - 1);
                        NPC.frameCounter = 0.0;
                    }
                    else if (NPC.frame.Y != 0 && NPC.frame.Y != num * (num174 - 1))
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.ai[0] == 11f)
                {
                    NPC.frameCounter += 1.0;
                    if (NPC.frame.Y / num == num174 - 1 && NPC.frameCounter >= 50.0)
                    {
                        if (NPC.frameCounter == 50.0)
                        {
                            int num194 = Main.rand.Next(4);
                            for (int l = 0; l < 3 + num194; l++)
                            {
                                int num195 = Dust.NewDust(NPC.Center + Vector2.UnitX * -NPC.direction * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, DustID.PirateStaff, -NPC.direction, 1f);
                                Dust obj3 = Main.dust[num195];
                                obj3.velocity /= 2f;
                                Main.dust[num195].scale = 0.8f;
                            }
                            if (Main.rand.Next(30) == 0)
                            {
                                int num196 = Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitX * -NPC.direction * 8f, Vector2.Zero, Main.rand.Next(580, 583));
                                Gore obj4 = Main.gore[num196];
                                obj4.velocity /= 2f;
                                Main.gore[num196].velocity.Y = Math.Abs(Main.gore[num196].velocity.Y);
                                Main.gore[num196].velocity.X = (0f - Math.Abs(Main.gore[num196].velocity.X)) * NPC.direction;
                            }
                        }
                        if (NPC.frameCounter >= 100.0 && Main.rand.Next(20) == 0)
                        {
                            NPC.frame.Y = 0;
                            NPC.frameCounter = 0.0;
                        }
                    }
                    else if (NPC.frame.Y / num == 0 && NPC.frameCounter >= 20.0)
                    {
                        NPC.frame.Y = num * (num174 - 1);
                        NPC.frameCounter = 0.0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            EmoteBubble.NewBubble(89, new WorldUIAnchor(NPC), 90);
                        }
                    }
                    else if (NPC.frame.Y != 0 && NPC.frame.Y != num * (num174 - 1))
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.ai[0] == 5f)
                {
                    NPC.frame.Y = num * (num174 - 3);
                    NPC.frameCounter = 0.0;
                }
                else if (NPC.ai[0] == 6f)
                {
                    NPC.frameCounter += 1.0;
                    int num197 = NPC.frame.Y / num;
                    int num181 = num174 - num197;
                    if ((uint)(num181 - 1) > 1u && (uint)(num181 - 4) > 1u && num197 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num198 = 0;
                    num198 = ((!(NPC.frameCounter < 10.0)) ? ((NPC.frameCounter < 16.0) ? (num174 - 5) : ((NPC.frameCounter < 46.0) ? (num174 - 4) : ((NPC.frameCounter < 60.0) ? (num174 - 5) : ((!(NPC.frameCounter < 66.0)) ? ((NPC.frameCounter < 72.0) ? (num174 - 5) : ((NPC.frameCounter < 102.0) ? (num174 - 4) : ((NPC.frameCounter < 108.0) ? (num174 - 5) : ((!(NPC.frameCounter < 114.0)) ? ((NPC.frameCounter < 120.0) ? (num174 - 5) : ((NPC.frameCounter < 150.0) ? (num174 - 4) : ((NPC.frameCounter < 156.0) ? (num174 - 5) : ((!(NPC.frameCounter < 162.0)) ? ((NPC.frameCounter < 168.0) ? (num174 - 5) : ((NPC.frameCounter < 198.0) ? (num174 - 4) : ((NPC.frameCounter < 204.0) ? (num174 - 5) : ((!(NPC.frameCounter < 210.0)) ? ((NPC.frameCounter < 216.0) ? (num174 - 5) : ((NPC.frameCounter < 246.0) ? (num174 - 4) : ((NPC.frameCounter < 252.0) ? (num174 - 5) : ((!(NPC.frameCounter < 258.0)) ? ((NPC.frameCounter < 264.0) ? (num174 - 5) : ((NPC.frameCounter < 294.0) ? (num174 - 4) : ((NPC.frameCounter < 300.0) ? (num174 - 5) : 0))) : 0)))) : 0)))) : 0)))) : 0)))) : 0)))) : 0);
                    if (num198 == num174 - 4 && num197 == num174 - 5)
                    {
                        Vector2 position = NPC.Center + new Vector2(10 * NPC.direction, -4f);
                        for (int m = 0; m < 8; m++)
                        {
                            int num199 = Main.rand.Next(139, 143);
                            int num200 = Dust.NewDust(position, 0, 0, num199, NPC.velocity.X + NPC.direction, NPC.velocity.Y - 2.5f, 0, default(Color), 1.2f);
                            Main.dust[num200].velocity.X += NPC.direction * 1.5f;
                            Dust obj5 = Main.dust[num200];
                            obj5.position -= new Vector2(4f);
                            Dust obj6 = Main.dust[num200];
                            obj6.velocity *= 2f;
                            Main.dust[num200].scale = 0.7f + Main.rand.NextFloat() * 0.3f;
                        }
                    }
                    NPC.frame.Y = num * num198;
                    if (NPC.frameCounter >= 300.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if ((NPC.ai[0] == 7f || NPC.ai[0] == 19f))
                {
                    NPC.frameCounter += 1.0;
                    int num201 = NPC.frame.Y / num;
                    int num180 = num174 - num201;
                    if ((uint)(num180 - 1) > 1u && (uint)(num180 - 4) > 1u && num201 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num202 = 0;
                    if (NPC.frameCounter < 16.0)
                    {
                        num202 = 0;
                    }
                    else if (NPC.frameCounter == 16.0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 112);
                    }
                    else if (NPC.frameCounter < 128.0)
                    {
                        num202 = ((NPC.frameCounter % 16.0 < 8.0) ? (num174 - 2) : 0);
                    }
                    else if (NPC.frameCounter < 160.0)
                    {
                        num202 = 0;
                    }
                    else if (NPC.frameCounter != 160.0 || Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        num202 = ((NPC.frameCounter < 220.0) ? ((NPC.frameCounter % 12.0 < 6.0) ? (num174 - 2) : 0) : 0);
                    }
                    else
                    {
                        EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 60);
                    }
                    NPC.frame.Y = num * num202;
                    if (NPC.frameCounter >= 220.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.ai[0] == 9f)
                {
                    NPC.frameCounter += 1.0;
                    int num204 = NPC.frame.Y / num;
                    int num179 = num174 - num204;
                    if ((uint)(num179 - 1) > 1u && (uint)(num179 - 4) > 1u && num204 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num205 = 0;
                    num205 = ((!(NPC.frameCounter < 10.0)) ? ((!(NPC.frameCounter < 16.0)) ? (num174 - 4) : (num174 - 5)) : 0);
                    if (NPC.ai[1] < 16f)
                    {
                        num205 = num174 - 5;
                    }
                    if (NPC.ai[1] < 10f)
                    {
                        num205 = 0;
                    }
                    NPC.frame.Y = num * num205;
                }
                else if (NPC.ai[0] == 18f)
                {
                    NPC.frameCounter += 1.0;
                    int num206 = NPC.frame.Y / num;
                    int num178 = num174 - num206;
                    if ((uint)(num178 - 1) > 1u && (uint)(num178 - 4) > 1u && num206 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num207 = Main.npcFrameCount[Type] - 2;
                    NPC.frame.Y = num * num207;
                }
                else if (NPC.ai[0] == 10f || NPC.ai[0] == 13f)
                {
                    NPC.frameCounter += 1.0;
                    int num208 = NPC.frame.Y / num;
                    if ((uint)(num208 - num174) > 3u && num208 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num209 = 10;
                    int num210 = 6;
                    int num211 = 0;
                    num211 = ((!(NPC.frameCounter < num209)) ? ((NPC.frameCounter < num209 + num210) ? num174 : ((NPC.frameCounter < num209 + num210 * 2) ? (num174 + 1) : ((NPC.frameCounter < num209 + num210 * 3) ? (num174 + 2) : ((NPC.frameCounter < num209 + num210 * 4) ? (num174 + 3) : 0)))) : 0);
                    NPC.frame.Y = num * num211;
                }
                else if (NPC.ai[0] == 15f)
                {
                    NPC.frameCounter += 1.0;
                    int num212 = NPC.frame.Y / num;
                    if ((uint)(num212 - num174) > 3u && num212 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    float num213 = NPC.ai[1] / NPCID.Sets.AttackTime[Type];
                    int num3 = 0;
                    num3 = ((num213 > 0.65f) ? num174 : ((num213 > 0.5f) ? (num174 + 1) : ((num213 > 0.35f) ? (num174 + 2) : ((num213 > 0f) ? (num174 + 3) : 0))));
                    NPC.frame.Y = num * num3;
                }
                else if (NPC.ai[0] == 12f)
                {
                    NPC.frameCounter += 1.0;
                    int num4 = NPC.frame.Y / num;
                    if ((uint)(num4 - num174) > 4u && num4 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num5 = num174 + NPC.GetShootingFrame(NPC.ai[2]);
                    NPC.frame.Y = num * num5;
                }
                else if (NPC.ai[0] == 14f)
                {
                    NPC.frameCounter += 1.0;
                    int num6 = NPC.frame.Y / num;
                    if ((uint)(num6 - num174) > 1u && num6 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    int num7 = 12;
                    int num8 = ((NPC.frameCounter % num7 * 2.0 < num7) ? num174 : (num174 + 1));
                    NPC.frame.Y = num * num8;
                }
                else if (NPC.ai[0] == 1001f)
                {
                    NPC.frame.Y = num * (num174 - 1);
                    NPC.frameCounter = 0.0;
                }
                else if (NPC.CanTalk && (NPC.ai[0] == 3f || NPC.ai[0] == 4f))
                {
                    NPC.frameCounter += 1.0;
                    int num9 = NPC.frame.Y / num;
                    int num177 = num174 - num9;
                    if ((uint)(num177 - 1) > 1u && (uint)(num177 - 4) > 1u && num9 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    bool flag7 = NPC.ai[0] == 3f;
                    int num10 = 0;
                    int num11 = 0;
                    int num12 = -1;
                    int num14 = -1;
                    if (NPC.frameCounter < 10.0)
                    {
                        num10 = 0;
                    }
                    else if (NPC.frameCounter < 16.0)
                    {
                        num10 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 46.0)
                    {
                        num10 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 60.0)
                    {
                        num10 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 216.0)
                    {
                        num10 = 0;
                    }
                    else if (NPC.frameCounter == 216.0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        num12 = 70;
                    }
                    else if (NPC.frameCounter < 286.0)
                    {
                        num10 = ((NPC.frameCounter % 12.0 < 6.0) ? (num174 - 2) : 0);
                    }
                    else if (NPC.frameCounter < 320.0)
                    {
                        num10 = 0;
                    }
                    else if (NPC.frameCounter != 320.0 || Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        num10 = ((NPC.frameCounter < 420.0) ? ((NPC.frameCounter % 16.0 < 8.0) ? (num174 - 2) : 0) : 0);
                    }
                    else
                    {
                        num12 = 100;
                    }
                    if (NPC.frameCounter < 70.0)
                    {
                        num11 = 0;
                    }
                    else if (NPC.frameCounter != 70.0 || Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        num11 = ((!(NPC.frameCounter < 160.0)) ? ((NPC.frameCounter < 166.0) ? (num174 - 5) : ((NPC.frameCounter < 186.0) ? (num174 - 4) : ((NPC.frameCounter < 200.0) ? (num174 - 5) : ((!(NPC.frameCounter < 320.0)) ? ((NPC.frameCounter < 326.0) ? (num174 - 1) : 0) : 0)))) : ((NPC.frameCounter % 16.0 < 8.0) ? (num174 - 2) : 0));
                    }
                    else
                    {
                        num14 = 90;
                    }
                    if (flag7)
                    {
                        NPC nPC = Main.npc[(int)NPC.ai[2]];
                        if (num12 != -1)
                        {
                            EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), num12, new WorldUIAnchor(nPC));
                        }
                        if (num14 != -1 && nPC.CanTalk)
                        {
                            EmoteBubble.NewBubbleNPC(new WorldUIAnchor(nPC), num14, new WorldUIAnchor(NPC));
                        }
                    }
                    NPC.frame.Y = num * (flag7 ? num10 : num11);
                    if (NPC.frameCounter >= 420.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.CanTalk && (NPC.ai[0] == 16f || NPC.ai[0] == 17f))
                {
                    NPC.frameCounter += 1.0;
                    int num15 = NPC.frame.Y / num;
                    int num176 = num174 - num15;
                    if ((uint)(num176 - 1) > 1u && (uint)(num176 - 4) > 1u && num15 != 0)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                    bool flag8 = NPC.ai[0] == 16f;
                    int num16 = 0;
                    int num17 = -1;
                    if (NPC.frameCounter < 10.0)
                    {
                        num16 = 0;
                    }
                    else if (NPC.frameCounter < 16.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 22.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 28.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 34.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 40.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter == 40.0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        num17 = 45;
                    }
                    else if (NPC.frameCounter < 70.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 76.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 82.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 88.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 94.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 100.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter == 100.0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        num17 = 45;
                    }
                    else if (NPC.frameCounter < 130.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 136.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 142.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 148.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter < 154.0)
                    {
                        num16 = num174 - 4;
                    }
                    else if (NPC.frameCounter < 160.0)
                    {
                        num16 = num174 - 5;
                    }
                    else if (NPC.frameCounter != 160.0 || Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        num16 = ((NPC.frameCounter < 220.0) ? (num174 - 4) : ((NPC.frameCounter < 226.0) ? (num174 - 5) : 0));
                    }
                    else
                    {
                        num17 = 75;
                    }
                    if (flag8 && num17 != -1)
                    {
                        int num18 = (int)NPC.localAI[2];
                        int num19 = (int)NPC.localAI[3];
                        int num20 = (int)Main.npc[(int)NPC.ai[2]].localAI[3];
                        int num21 = (int)Main.npc[(int)NPC.ai[2]].localAI[2];
                        int num22 = 3 - num18 - num19;
                        int num23 = 0;
                        if (NPC.frameCounter == 40.0)
                        {
                            num23 = 1;
                        }
                        if (NPC.frameCounter == 100.0)
                        {
                            num23 = 2;
                        }
                        if (NPC.frameCounter == 160.0)
                        {
                            num23 = 3;
                        }
                        int num25 = 3 - num23;
                        int num26 = -1;
                        int num27 = 0;
                        while (num26 < 0)
                        {
                            num176 = num27 + 1;
                            num27 = num176;
                            if (num176 >= 100)
                            {
                                break;
                            }
                            num26 = Main.rand.Next(2);
                            if (num26 == 0 && num21 >= num19)
                            {
                                num26 = -1;
                            }
                            if (num26 == 1 && num20 >= num18)
                            {
                                num26 = -1;
                            }
                            if (num26 == -1 && num25 <= num22)
                            {
                                num26 = 2;
                            }
                        }
                        if (num26 == 0)
                        {
                            Main.npc[(int)NPC.ai[2]].localAI[3] += 1f;
                            num20++;
                        }
                        if (num26 == 1)
                        {
                            Main.npc[(int)NPC.ai[2]].localAI[2] += 1f;
                            num21++;
                        }
                        UnifiedRandom rand = Main.rand;
                        int[] array = new int[] { 36, 37, 38 };
                        int num28 = Utils.SelectRandom(rand, array);
                        int num29 = num28;
                        switch (num26)
                        {
                            case 0:
                                switch (num28)
                                {
                                    case 38:
                                        num29 = 37;
                                        break;
                                    case 37:
                                        num29 = 36;
                                        break;
                                    case 36:
                                        num29 = 38;
                                        break;
                                }
                                break;
                            case 1:
                                switch (num28)
                                {
                                    case 38:
                                        num29 = 36;
                                        break;
                                    case 37:
                                        num29 = 38;
                                        break;
                                    case 36:
                                        num29 = 37;
                                        break;
                                }
                                break;
                        }
                        if (num25 == 0)
                        {
                            if (num20 >= 2)
                            {
                                num28 -= 3;
                            }
                            if (num21 >= 2)
                            {
                                num29 -= 3;
                            }
                        }
                        EmoteBubble.NewBubble(num28, new WorldUIAnchor(NPC), num17);
                        EmoteBubble.NewBubble(num29, new WorldUIAnchor(Main.npc[(int)NPC.ai[2]]), num17);
                    }
                    NPC.frame.Y = num * (flag8 ? num16 : num16);
                    if (NPC.frameCounter >= 420.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.velocity.X == 0f)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0.0;
                }
                else
                {
                    int num31 = 6;
                    NPC.frameCounter += Math.Abs(NPC.velocity.X) * 2f;
                    NPC.frameCounter += 1.0;
                    int num32 = num * 2;
                    if (NPC.frame.Y < num32)
                    {
                        NPC.frame.Y = num32;
                    }
                    if (NPC.frameCounter > num31)
                    {
                        NPC.frame.Y += num;
                        NPC.frameCounter = 0.0;
                    }
                    if (NPC.frame.Y / num >= Main.npcFrameCount[Type] - num173)
                    {
                        NPC.frame.Y = num32;
                    }
                }
            }
            else
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = num;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            //produces a puff of dust on death
            if (NPC.life <= 0)
            {
                for (int num585 = 0; num585 < 25; num585++)
                {
                    int num586 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
                    Dust dust30 = Main.dust[num586];
                    Dust dust187 = dust30;
                    dust187.velocity *= 1.4f;
                    Main.dust[num586].noLight = true;
                    Main.dust[num586].noGravity = true;
                }
                int num587 = 0;
                Vector2 val15 = new Vector2(NPC.position.X + NPC.width / 2 - 24f, NPC.position.Y + NPC.height / 2 - 24f);
                Vector2 center3 = default(Vector2);
                num587 = Gore.NewGore(NPC.GetSource_Death(), val15, center3, Main.rand.Next(61, 64));
                Main.gore[num587].scale = 1f;
                Main.gore[num587].velocity.X += 1f;
                Main.gore[num587].velocity.Y += 1f;
                Vector2 val16 = new Vector2(NPC.position.X + NPC.width / 2 - 24f, NPC.position.Y + NPC.height / 2 - 24f);
                center3 = default(Vector2);
                num587 = Gore.NewGore(NPC.GetSource_Death(), val16, center3, Main.rand.Next(61, 64));
                Main.gore[num587].scale = 1f;
                Main.gore[num587].velocity.X -= 1f;
                Main.gore[num587].velocity.Y += 1f;
                Vector2 val17 = new Vector2(NPC.position.X + NPC.width / 2 - 24f, NPC.position.Y + NPC.height / 2 - 24f);
                center3 = default(Vector2);
                num587 = Gore.NewGore(NPC.GetSource_Death(), val17, center3, Main.rand.Next(61, 64));
                Main.gore[num587].scale = 1f;
                Main.gore[num587].velocity.X += 1f;
                Main.gore[num587].velocity.Y -= 1f;
                Vector2 val18 = new Vector2(NPC.position.X + NPC.width / 2 - 24f, NPC.position.Y + NPC.height / 2 - 24f);
                center3 = default(Vector2);
                num587 = Gore.NewGore(NPC.GetSource_Death(), val18, center3, Main.rand.Next(61, 64));
                Main.gore[num587].scale = 1f;
                Main.gore[num587].velocity.X -= 1f;
                Main.gore[num587].velocity.Y -= 1f;
            }
        }
    }
}

