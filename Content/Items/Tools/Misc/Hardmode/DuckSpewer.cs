using Microsoft.Xna.Framework;
using Polarities.Content.Items.Weapons.Magic.Guns.Hardmode;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Tools.Misc.Hardmode
{
    public class DuckSpewer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 1;
            Item.value = 80000;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<WormSpewerProjectile>();
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Duck, player.position);
            int duck = NPC.NewNPC(source, (int)position.X, (int)position.Y, Main.rand.NextBool() ? NPCID.Duck2 : NPCID.DuckWhite2);
            Main.npc[duck].velocity = velocity;
            Main.npc[duck].SpawnedFromStatue = true;
            Main.npc[duck].direction = -player.direction;
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 4);
        }

        //TODO: Require a convective duck and have a small chance to shoot them when they're added
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<WormSpewer>())
                .AddIngredient(ItemID.Duck, 20)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}