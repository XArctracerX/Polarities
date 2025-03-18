using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace Polarities.Content.Items.Ammo.Hardmode
{
	public class ElectricArrow : ModItem
	{
		public override void SetStaticDefaults() {
            //Tooltip.SetDefault("Creates a field of electricity upon impact");
            Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.consumable = true;             //You need to set the Item consumable so that the ammo would automatically consumed
			Item.knockBack = 1f;
			Item.value = 50;
			Item.rare = ItemRarityID.Red;
			Item.shoot = ProjectileType<ElectricArrowProjectile>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

        /*public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Items.SmiteSoul>());
            recipe.AddIngredient(ItemID.WoodenArrow,111);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this,111);
            recipe.AddRecipe();
        }*/
	}
    
    public class ElectricArrowProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Electric Arrow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
        }

        public override void AI() {
            Main.dust[Dust.NewDust(Projectile.position,Projectile.width,Projectile.height,DustID.Electric,newColor:Color.LightBlue,Scale:1f)].noGravity = true;

            Projectile.rotation = Projectile.velocity.ToRotation()+(float)Math.PI/2;

            Projectile.velocity.Y += 0.1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.Kill();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurt) {
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,Vector2.Zero,ProjectileID.Electrosphere,Projectile.damage,0f,Projectile.owner);
		}
    }
}
