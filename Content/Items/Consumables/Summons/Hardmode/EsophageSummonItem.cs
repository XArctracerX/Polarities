using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Polarities.Content.Events;
using Polarities.Content.NPCs.Bosses.Hardmode.Esophage;
using Polarities.Content.NPCs.Bosses.Hardmode.Hemorrphage;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.Summons.Hardmode
{
    //TODO: Inventory blood drip effect thing during the eclipse
    public class EsophageSummonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<Esophage>()) && PolaritiesSystem.esophageSpawnTimer == 0 && !NPC.AnyNPCs(NPCType<Hemorrphage>());
        }

        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }

        public override bool? UseItem(Player player)
        {
            if (!(NPC.AnyNPCs(NPCType<Hemorrphage>())) && Main.bloodMoon)
			{
                Hemorrphage.SpawnOn(player);
			}
			else
            {
                Esophage.SpawnOn(player);
            }
            return true;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
        {
            if (Main.bloodMoon)
            {
                TooltipLine line = new TooltipLine(Mod, "Tooltip1", Language.GetTextValue("Mods.Polarities.Items.EsophageSummonItem.Extra"));
                tooltips.Add(line);
            }
        }
    }
}