using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.NPCs.Enemies.Fractal.PostSentinel;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Movement.PreHardmode
{
    [AutoloadEquip(EquipType.Back)]
    public class RCSThrusters : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 12;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            float activeHeight = MathHelper.Lerp(Main.topWorld, Main.bottomWorld, 1 / 6f);
            if (player.position.Y < 2000)
            {
                player.velocity.Y -= player.gravity;
                if (player.controlUp || player.controlJump)
                {
                    player.velocity = Vector2.Lerp(player.velocity, new Vector2(player.velocity.X, -0.5f), 0.1f);
                }
                else if (player.controlDown)
                {
                    player.velocity = Vector2.Lerp(player.velocity, new Vector2(player.velocity.X, 1f), 0.1f);
                }
                if (player.controlLeft)
                {
                    player.velocity = Vector2.Lerp(player.velocity, new Vector2(-3.5f, player.velocity.Y), 0.1f);
                }
                else if (player.controlRight)
                {
                    player.velocity = Vector2.Lerp(player.velocity, new Vector2(3.5f, player.velocity.Y), 0.1f);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RocketBoots)
                .AddIngredient(ItemType<Placeable.Bars.SunplateBar>(), 6)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}