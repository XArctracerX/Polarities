using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Polarities.Content.Items.Weapons.Melee.Warhammers;
using Polarities.Content.Buffs.PreHardmode;
using Polarities.Content.Items.Placeable.Blocks;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.PreHardmode.Other
{
    public class HaliteWarhammer : WarhammerBase
    {
        public override int HammerLength => 52;
        public override int HammerHeadSize => 16;
        public override int DefenseLoss => 8;
        public override int DebuffTime => 600;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(18, 12, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 74;
            Item.height = 74;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Blue;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Desiccating>(), 300);

            // base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SaltCrystals>(), 16)
                .AddIngredient(ItemType<Salt>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

