using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Polarities.Global;
//using Polarities.Biomes;
//using Polarities.Biomes.Fractal;
//using Polarities.Buffs;
//using Polarities.Items;
//using Polarities.Items.Accessories;
//using Polarities.Items.Armor.MechaMayhemArmor;
//using Polarities.Items.Books;
//using Polarities.Items.Weapons.Magic;
//using Polarities.Items.Weapons.Melee;
//using Polarities.Items.Weapons.Melee.Warhammers;
//using Polarities.Items.Weapons.Ranged;
//using Polarities.Items.Weapons.Ranged.Atlatls;
//using Polarities.Items.Weapons.Summon.Orbs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.ModLoader.ModContent;

namespace Polarities
{
    //public enum NPCCapSlotID
    //{
        //HallowInvasion,
        //WorldEvilInvasion,
        //WorldEvilInvasionWorm,
    //}

    public class PolaritiesNPC : GlobalNPC
    {
        //public Dictionary<int, int> hammerTimes;

        //public bool flawless = true;

        //public static Dictionary<int, bool> bestiaryCritter = new Dictionary<int, bool>();

        //public override void Load()
        //{
            //Terraria.On_NPC.GetNPCColorTintedByBuffs += NPC_GetNPCColorTintedByBuffs;

            //Terraria.IL_NPC.StrikeNPC += NPC_StrikeNPC;

            //counts weird critters
            //Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.AddEmptyEntries_CrittersAndEnemies_Automated += BestiaryDatabaseNPCsPopulator_AddEmptyEntries_CrittersAndEnemies_Automated;
            //Terraria.GameContent.Bestiary.IL_NPCWasNearPlayerTracker.ScanWorldForFinds += NPCWasNearPlayerTracker_ScanWorldForFinds;
            //Terraria.On_NPC.HittableForOnHitRewards += NPC_HittableForOnHitRewards;

            //avoid bad spawns
            //IL_ChooseSpawn += PolaritiesNPC_IL_ChooseSpawn;

            //flawless continuity for EoW
            //Terraria.On_NPC.Transform += NPC_Transform;

            //force counts things for the radar
            //Terraria.IL_Main.DrawInfoAccs += Main_DrawInfoAccs;

            //allows npcs to spawn in lava
            //moves prismatic lacewings to post-sun-pixie
            //Terraria.IL_NPC.SpawnNPC += NPC_SpawnNPC;
        //}

        //public override void SetDefaults(NPC npc)
        //{
        //hammerTimes = new Dictionary<int, int>();

        //switch (npc.type)
        //{
        //case NPCID.DungeonGuardian:
        //npc.buffImmune[BuffType<Incinerating>()] = true;
        //break;
        //}
        //}

        //private static bool? IsBestiaryCritter(int npcType)
        //{
        //return bestiaryCritter.ContainsKey(npcType) ? bestiaryCritter[npcType] : null;
        //}

        //private override void NPC_Transform(Terraria.On_NPC.orig_Transform orig, NPC self, int newType)
        //{
            //bool flawless = self.GetGlobalNPC<PolaritiesNPC>().flawless;
            //Dictionary<int, int> hammerTimes = self.GetGlobalNPC<PolaritiesNPC>().hammerTimes;

            //orig(self, newType);

            //self.GetGlobalNPC<PolaritiesNPC>().flawless = flawless;
            //self.GetGlobalNPC<PolaritiesNPC>().hammerTimes = hammerTimes;
        //}

    }
}

