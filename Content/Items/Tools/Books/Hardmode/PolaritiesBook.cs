using Polarities.Content.Buffs.Hardmode;
using Polarities.Core;
using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace Polarities.Content.Items.Tools.Books.Hardmode
{
    public class PolaritiesBook : BookBase
    {
        public override int BuffType => BuffType<PolaritiesBookBuff>();
        public override int BookIndex => 32;
    }

    public class PolaritiesBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<PolaritiesBook>();

        public override void Update(Player player, ref int buffIndex)
        {
            //Repel enemies
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!Main.npc[i].friendly && Main.npc[i].knockBackResist != 0 && !Main.npc[i].boss)
                {
                    Vector2 displacement = Main.npc[i].Center - player.Center;
                    Main.npc[i].velocity += displacement.SafeNormalize(Vector2.Zero) * 5000 / Math.Max(5000, displacement.LengthSquared());
                }
            }
            base.Update(player, ref buffIndex);
        }
    }
}