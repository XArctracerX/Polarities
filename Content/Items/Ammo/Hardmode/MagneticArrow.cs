using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace Polarities.Content.Items.Ammo.Hardmode
{
	public class MagneticArrow : ModItem
	{
		public override void SetStaticDefaults() {
			//Tooltip.SetDefault("Homes in on enemies and sticks to them");
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.consumable = true;             //You need to set the Item consumable so that the ammo would automatically consumed
			Item.knockBack = 1f;
			Item.value = 50;
			Item.rare = ItemRarityID.Red;
			Item.shoot = ProjectileType<MagneticArrowProjectile>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

        /*public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Items.Placeable.PolarizedBar>());
            recipe.AddIngredient(ItemID.WoodenArrow,111);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this,111);
            recipe.AddRecipe();
        }*/
	}
    
    public class MagneticArrowProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magnetic Arrow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
		}

		public bool IsStickingToTarget
		{
			get => Projectile.ai[0] == 1f;
			set => Projectile.ai[0] = value ? 1f : 0f;
		}

		// Index of the current target
		public int TargetWhoAmI
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		private const int MAX_STICKY_JAVELINS = 16; // This is the max. amount of javelins being able to attach
		private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javelins

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			IsStickingToTarget = true; // we are sticking to a target
			TargetWhoAmI = target.whoAmI; // Set the target whoAmI
			Projectile.velocity =
				(target.Center - Projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
			Projectile.netUpdate = true; // netUpdate this javelin
			target.AddBuff(BuffType<MagnetArrowBuff>(), 900); // Adds the ExampleJavelin debuff for a very small DoT

			Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

			// It is recommended to split your code into separate methods to keep code clean and clear
			UpdateStickyJavelins(target);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
		}

		/*
		 * The following code handles the javelin sticking to the enemy hit.
		 */
		private void UpdateStickyJavelins(NPC target)
		{
			int currentJavelinIndex = 0; // The javelin index

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
					&& currentProjectile.active // Make sure the projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
					&& currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as this javelin
					&& currentProjectile.ModProjectile is MagneticArrowProjectile javelinProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
					&& javelinProjectile.IsStickingToTarget // the previous pattern match allows us to use our properties
					&& javelinProjectile.TargetWhoAmI == target.whoAmI)
				{

					_stickingJavelins[currentJavelinIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
					if (currentJavelinIndex >= _stickingJavelins.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
						break;
				}
			}

			// Remove the oldest sticky javelin if we exceeded the maximum
			if (currentJavelinIndex >= MAX_STICKY_JAVELINS)
			{
				int oldJavelinIndex = 0;
				// Loop our point array
				for (int i = 1; i < MAX_STICKY_JAVELINS; i++)
				{
					// Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
					if (_stickingJavelins[i].Y < _stickingJavelins[oldJavelinIndex].Y)
					{
						oldJavelinIndex = i; // Remember the index of the removed javelin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
				Main.projectile[_stickingJavelins[oldJavelinIndex].X].Kill();
			}
		}
		public override void AI()
		{
			// Run either the Sticky AI or Normal AI
			// Separating into different methods helps keeps your AI clean
			if (IsStickingToTarget) StickyAI();
			else NormalAI();
		}

		private void NormalAI()
		{
			int targetID = -1;
			Projectile.Minion_FindTargetInRange(600, ref targetID, skipIfCannotHitWithOwnBody: true);
			//int targetID = PolaritiesProjectile.FindMinionTarget(projectile, 600f, requireLineOfSight: true, strictMaxDistance: true, respectTarget: false);
			if (targetID != -1)
			{
				NPC target = Main.npc[targetID];

				Vector2 goalVelocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 16f;
				Projectile.velocity += (goalVelocity - Projectile.velocity) / 10f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity.Y += 0.1f;
		}

		private void StickyAI()
		{
			// These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
			Projectile.ignoreWater = true; // Make sure the projectile ignores water
			Projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
			const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
			Projectile.localAI[0] += 1f;

			// Every 30 ticks, the javelin will perform a hit effect
			bool hitEffect = Projectile.localAI[0] % 30f == 0f;
			int projTargetIndex = (int)TargetWhoAmI;
			if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
			{ // If the index is past its limits, kill it
				Projectile.Kill();
			}
			else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
			{ // If the target is active and can take damage
			  // Set the projectile's position relative to the target's center
				Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
				Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				if (hitEffect)
				{ // Perform a hit effect here
					Main.npc[projTargetIndex].HitEffect(0, 1.0);
				}
			}
			else
			{ // Otherwise, kill the projectile
				Projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//SoundEngine.PlaySound(SoundID.Tink, Projectile.position);
		}
    }

	public class MagnetArrowBuff : ModBuff
    {
        public override string Texture => "Polarities/Content/Items/Ammo/Hardmode/MagneticArrow";

        public override void Update(NPC npc, ref System.Int32 buffIndex)
        {
			npc.lifeRegen -= 200;
            base.Update(npc, ref buffIndex);
        }
    }
}
