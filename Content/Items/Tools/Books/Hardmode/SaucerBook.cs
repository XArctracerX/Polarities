using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Books.Hardmode
{
    public class SaucerBook : BookBase
    {
        public override int BuffType => BuffType<SaucerBookBuff>();
        public override int BookIndex => 30;
    }

    public class SaucerBookBuff : BookBuffBase
    {
        public override int ItemType => ItemType<SaucerBook>();

        
    }
}