using Terraria;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Books.Hardmode
{
    public class HemorrphageBook : BookBase
    {
        public override int BuffType => BuffType<HemorrphageBookBuff>();
        public override int BookIndex => 27;
    }

    public class HemorrphageBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<HemorrphageBook>();
        /*
         * public override void ModifyBuffTip(ref string tip, ref int rare)
         * {
         *    string hotkey = "an unbound hotkey";
         *    if (Polarities.HemorrphageTeleportHotKey.GetAssignedKeys().ToArray().Length > 0) hotkey = Polarities.HemorrphageTeleportHotKey.GetAssignedKeys()[0];
         *    tip = "You can press " + hotkey + " to teleport while on the ground";
         * }
         */

        int trailTimer = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PolaritiesPlayer>().hemorrphageTeleport = true;
            base.Update(player, ref buffIndex);
        }
    }
}