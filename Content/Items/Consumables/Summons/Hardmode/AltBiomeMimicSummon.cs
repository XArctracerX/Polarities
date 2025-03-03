using Polarities.Content.Items.Materials;
using Polarities.Global;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.Summons.Hardmode
{
    public interface IBiomeMimicSummon
    {
        int SpawnMimicType { get; }
    }

    public class AltBiomeMimicSummon : ModItem, IBiomeMimicSummon
    {
        public int SpawnMimicType => Main.hardMode ? (WorldGen.crimson ? NPCID.BigMimicCorruption : NPCID.BigMimicCrimson) : NPCID.None;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.NightKey);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.NightKey)
                //.AddIngredient(ItemType<EvilDNA>())
                .Register();
        }
    }
}