using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Content.Buffs.Hardmode
{
	public class Magnetized : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{player.GetModPlayer<PolaritiesPlayer>().magnetized = true;}
	}
}