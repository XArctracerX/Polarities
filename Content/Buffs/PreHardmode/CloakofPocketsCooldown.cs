using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Buffs.PreHardmode
{
	public class CloakofPocketsCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cloak of Pockets Cooldown");
			//Description.SetDefault("You have seen too much");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        //public override void Update(Player player, ref int buffIndex)
        //{
            //player.wingTimeMax = 0;
            //player.wingTime = 0;
            //player.wings = 0;
            //player.wingsLogic = 0;
            //player.noFallDmg = true;
            //player.noBuilding = true;

            //player.controlJump = false;
            //player.controlDown = false;
            //player.controlLeft = false;
            //player.controlRight = false;
            //player.controlUp = false;
            //player.controlUseItem = false;
            //player.controlUseTile = false;
            //player.controlThrow = false;
            //player.gravDir = 1f;
            //for (int i = 0; i < 4; i++)
                //player.doubleTapCardinalTimer[i] = 0;

            //player.velocity.Y += player.gravity;
            //if (player.velocity.Y > player.maxFallSpeed)
                //player.velocity.Y = player.maxFallSpeed;

            //player.sandStorm = false;
            //player.blockExtraJumps = true;
            //if (player.mount.Active)
                //player.mount.Dismount(player);
        //}
    }
}