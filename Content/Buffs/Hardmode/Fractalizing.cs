using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Polarities.Core;
using Polarities.Global;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Buffs.Hardmode
{
    public class Fractalizing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (FractalSubworld.Active && player.HasBuff(ModContent.BuffType<Fractalizing>()))
            {
                player.buffTime[player.FindBuffIndex(ModContent.BuffType<Fractalizing>())] += player.GetModPlayer<PolaritiesPlayer>().fractalSubworldDebuffRate;
            }
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            var player = Main.LocalPlayer;

            if (!FractalSubworld.Active)
            {
                return;
            }
            int time = player.GetFractalization();
            if (time > FractalSubworld.HARDMODE_DANGER_TIME)
            {
                tip += $"\n{Language.GetTextValue("Mods.Polarities.Buffs.Fractalizing.PostDendriticEnergy")}";
            }
            else
            {
                tip += $"\n{Language.GetTextValue("Mods.Polarities.Buffs.Fractalizing.PreDendriticEnergy")}";
            }
            if (time > FractalSubworld.POST_SENTINEL_TIME)
            {
                tip += $"\n{Language.GetTextValue("Mods.Polarities.Buffs.Fractalizing.PostSentinel")}";
            }
            if (player.HasBuff(ModContent.BuffType<Items.Consumables.Potions.Hardmode.FractalAntidoteBuff>()))
            {
                tip += $"\n{Language.GetTextValue("Mods.Polarities.Buffs.Fractalizing.FractalAntidote")}";
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time;
            return false;
        }
    }
}