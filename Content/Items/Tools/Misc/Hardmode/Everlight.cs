using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
    public class Everlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.holdStyle = 1;
        }

        public override void HoldItem(Player player)
        {
            if (Lighting.Mode == LightMode.White || Lighting.Mode == LightMode.Color)
                Lighting.AddLight(Main.MouseWorld, new Vector3(10f, 10f, 10f));
            else
                Lighting.AddLight(Main.MouseWorld, new Vector3(2f, 2f, 2f));
            player.scope = true;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center + new Vector2(0, Item.width / 2);
        }
    }

    public class EverlightII : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.holdStyle = 1;
        }

        public override void HoldItem(Player player)
        {
            if (Lighting.Mode == LightMode.White || Lighting.Mode == LightMode.Color)
                Lighting.AddLight(Main.MouseWorld, new Vector3(25f, 25f, 25f));
            else
                Lighting.AddLight(Main.MouseWorld, new Vector3(5f, 5f, 5f));

            for (int i = 0; i < 6; i++)
            {
                Dust.NewDustPerfect(Main.MouseWorld + Main.rand.NextVector2Circular(128, 128), DustID.Torch, Scale: 1.5f).noGravity = true;
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.DistanceSQ(Main.MouseWorld) < 128 * 128 && npc.active && !npc.friendly)
                {
                    npc.AddBuff(ModContent.BuffType<Buffs.Hardmode.Incinerating>(), 2);
                }
            }
            player.scope = true;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center + new Vector2(0, Item.width / 2);
        }
    }
}