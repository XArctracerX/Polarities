﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Content.Items.Consumables.Summons.Hardmode;
using Polarities.Content.Items.Weapons.Ranged.Atlatls.Hardmode;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Global
{
    public class PolaritiesItem : GlobalItem
    {
        public static HashSet<int> IsFlawless { get; private set; }
        public static HashSet<int> IsFractalWeapon { get; private set; }

        public override bool InstancePerEntity => true;

        public override void Load()
        {
            IsFlawless = new HashSet<int>() { ItemID.EmpressBlade, };
            IsFractalWeapon = new HashSet<int>();

            //custom biome mimic summons
            Terraria.On_NPC.BigMimicSummonCheck += NPC_BigMimicSummonCheck;
            Terraria.UI.On_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;
        }

        private static void ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(Terraria.UI.On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
        {
            bool doDraw = true;
            IInventoryDrawItem inventoryDrawItem = null;
            if (inv[slot] != null && inv[slot].ModItem != null && inv[slot].ModItem is IInventoryDrawItem)
            {
                inventoryDrawItem = inv[slot].ModItem as IInventoryDrawItem;
                doDraw = inventoryDrawItem.PreInventoryDraw(spriteBatch, inv, context, slot, position, lightColor);
            }
            if (doDraw)
            {
                orig(spriteBatch, inv, context, slot, position, lightColor);

                if (inventoryDrawItem != null)
                {
                    inventoryDrawItem.PostInventoryDraw(spriteBatch, inv, context, slot, position, lightColor);
                }
            }
        }

        private static bool NPC_BigMimicSummonCheck(Terraria.On_NPC.orig_BigMimicSummonCheck orig, int x, int y, Player user)
        {
            //adapted from vanilla
            if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode)
            {
                return false;
            }
            int chestIndex = Chest.FindChest(x, y);
            if (chestIndex < 0)
            {
                return false;
            }
            bool doSummon = false;
            int numItems = 0;
            int summonType = 0;
            for (int i = 0; i < 40; i++)
            {
                ushort num5 = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].TileType;
                int num6 = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].TileFrameX / 36;
                if (TileID.Sets.BasicChest[num5] && (num5 != 21 || num6 < 5 || num6 > 6) && Main.chest[chestIndex].item[i] != null && Main.chest[chestIndex].item[i].type > ItemID.None)
                {
                    if (Main.chest[chestIndex].item[i].ModItem != null && Main.chest[chestIndex].item[i].ModItem is IBiomeMimicSummon biomeMimicSummon)
                    {
                        summonType = biomeMimicSummon.SpawnMimicType;
                        doSummon = true;
                    }
                    numItems += Main.chest[chestIndex].item[i].stack;

                    if (numItems > 1)
                    {
                        doSummon = false;
                        break;
                    }
                }
            }
            if (doSummon && summonType != 0)
            {
                _ = 1;
                if (TileID.Sets.BasicChest[Main.tile[x, y].TileType])
                {
                    if (Main.tile[x, y].TileFrameX % 36 != 0)
                    {
                        x--;
                    }
                    if (Main.tile[x, y].TileFrameY % 36 != 0)
                    {
                        y--;
                    }
                    int number = Chest.FindChest(x, y);
                    for (int j = 0; j < 40; j++)
                    {
                        Main.chest[chestIndex].item[j] = new Item();
                    }
                    Chest.DestroyChest(x, y);
                    for (int k = x; k <= x + 1; k++)
                    {
                        for (int l = y; l <= y + 1; l++)
                        {
                            if (TileID.Sets.BasicChest[Main.tile[k, l].TileType])
                            {
                                Main.tile[k, l].ClearTile();
                            }
                        }
                    }
                    int number2 = 1;
                    if (Main.tile[x, y].TileType == 467)
                    {
                        number2 = 5;
                    }
                    if (Main.tile[x, y].TileType >= 625)
                    {
                        number2 = 101;
                    }
                    NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, number2, x, y, 0f, number, Main.tile[x, y].TileType);
                    NetMessage.SendTileSquare(-1, x, y, 3);
                }
                int num8 = NPC.NewNPC(user.GetSource_TileInteraction(x, y), x * 16 + 16, y * 16 + 32, summonType);
                Main.npc[num8].whoAmI = num8;
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num8);
                Main.npc[num8].BigMimicSpawnSmoke();
                return false;
            }

            return orig(x, y, user);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            var clone = (PolaritiesItem)base.Clone(item, itemClone);
            return clone;
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.EmpressBlade)
            {
                item.rare = ModContent.RarityType<EmpressFlawlessRarity>();
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            try
            {
                int i;
                for (i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Name == "JourneyResearch" || tooltips[i].Name == "BestiaryNotes")
                    {
                        break;
                    }
                }

                if (IsFlawless.Contains(item.type))
                {
                    tooltips.Insert(i, new TooltipLine(Mod, "Flawless", Language.GetTextValue("Mods.Polarities.ItemTooltip.TooltipFlawless")));
                    i++;
                }
            }
            catch
            {
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum && player.GetModPlayer<PolaritiesPlayer>().manaStarMultiplier > 1f)
            {
                int manaVal = (int)(100 * (player.GetModPlayer<PolaritiesPlayer>().manaStarMultiplier - 1f));
                if (manaVal > 0)
                {
                    player.statMana += manaVal;
                    player.ManaEffect(manaVal);
                }
            }
            return true;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.WormScarf:
                    if (player.GetModPlayer<PolaritiesPlayer>().wormScarf)
                    {
                        player.endurance -= 0.12f;
                    }
                    player.GetModPlayer<PolaritiesPlayer>().wormScarf = true;
                    break;
            }
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            // In addition to this code, we also do similar code in Common/GlobalNPCs/ExampleNPCLoot.cs to edit the boss loot for non-expert drops. Remember to do both if your edits should affect non-expert drops as well.
            if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<JunglesRage>(), 4));
            }
        }
    }
}

