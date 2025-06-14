using Polarities.Content.Buffs.Hardmode;
using Polarities.Core;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Books.PreHardmode
{
    public class RiftDenizenBook : BookBase
    {
        public override int BuffType => BuffType<RiftDenizenBookBuff>();
        public override int BookIndex => 12;
    }

    public class RiftDenizenBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<RiftDenizenBook>();

        public override void Update(Player player, ref int buffIndex)
        {
            PolaritiesPlayer p = player.GetModPlayer<PolaritiesPlayer>();

            float fractalizationKillTime = 18000 * p.fractalSubworldDebuffLifeLossResistance;
            float fractalizationFraction = Math.Max(0, (p.GetFractalization() - p.fractalSubworldDebuffResistance) / fractalizationKillTime);

            player.GetDamage(DamageClass.Generic) += fractalizationFraction;

            base.Update(player, ref buffIndex);
        }
    }
}