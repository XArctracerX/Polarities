﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Armor.Classless.PreHardmode.AerogelArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class AerogelRobe : ModItem
    {
        public override void Load()
        {
            EquipLoader.AddEquipTexture(Mod, Texture + "_Legs", EquipType.Legs, this);
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.NeedsToDrawArm[equipSlotBody] = true;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.OverridesLegs[equipSlotLegs] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 30;
            Item.value = 1000;
            Item.rare = ItemRarityID.Green;
            Item.defense = 0;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 40)
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddTile(TileID.Solidifier)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 40)
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ItemID.TissueSample, 15)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class AerogelHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aerogel Hood");

            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 1000;
            Item.rare = ItemRarityID.Green;
            Item.defense = 0;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<AerogelRobe>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.Polarities.ArmorSetBonus.Aerogel.Description");

            if (!player.mount._active) { player.GetModPlayer<PolaritiesPlayer>().velocityMultiplier *= new Vector2(1.25f); }
            player.buffImmune[BuffID.OnFire] = true;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 20)
                .AddIngredient(ItemID.Gel, 15)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddTile(TileID.Solidifier)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 20)
                .AddIngredient(ItemID.Gel, 15)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}