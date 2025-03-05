using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Polarities.Content.Buffs.Hardmode
{
    public class ToxicShock : ModBuff
    {
        public override string Texture => "Polarities/Content/Buffs/PreHardmode/Desiccating";

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.knockBackResist > 0f)
            {
                if (npc.velocity.Length() > 0.1f)
                {
                    npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * 0.1f;
                }
            }
        }
    }
}