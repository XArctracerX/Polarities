using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Assets;
using Polarities.Content.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Boomerangs.Hardmode
{
	public class Phagocyte : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Phagocyte");
			//Tooltip.SetDefault("Sticks to enemies, dealing damage over time that increases significantly the more phagocytes latch on");
		}

		public override void SetDefaults()
		{
			Item.damage = 189;
            Item.DamageType = DamageClass.Melee;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 80000;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PhagocyteProjectile>();
            Item.shootSpeed = 15f;
            Item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			for (int i=0; i<Main.maxProjectiles; i++)
            {
				if (Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active && Main.projectile[i].type == Item.shoot && Main.projectile[i].ai[0] != 1)
                {
					return false;
                }
            }
			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
	}

    public class PhagocyteProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Content/Items/Weapons/Melee/Boomerangs/Hardmode/Phagocyte";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phagocyte");
        }

		public override void SetDefaults()
		{
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.alpha = 0;
            Projectile.timeLeft = 3000;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }

		public override void OnKill(int timeLeft)
        {            
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
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

		private const int MAX_STICKY_JAVELINS = 30; // This is the max. amount of javelins being able to attach
		private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javelins

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			IsStickingToTarget = true; // we are sticking to a target
			TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            Projectile.velocity =
				(target.Center - Projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.netUpdate = true; // netUpdate this javelin
            target.AddBuff(BuffType<PhagocyteBuff>(), 900); // Adds the ExampleJavelin debuff for a very small DoT

            Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

			// It is recommended to split your code into separate methods to keep code clean and clear
			UpdateStickyJavelins(target);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.immune[Projectile.owner] = 0;
        }

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
					&& currentProjectile.ModProjectile is PhagocyteProjectile javelinProjectile // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
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
			//else NormalAI();
		}

        private void StickyAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            const int aiFactor = 15;
            Projectile.localAI[0] += 1f;

            bool hitEffect = Projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = TargetWhoAmI;
            if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            {
                Projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            {
                Main.npc[projTargetIndex].GetGlobalNPC<PolaritiesNPC>().chlorophyteDarts++;
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect)
                {
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
            Projectile.ai[0] = -1f;
			if (Projectile.velocity.X != oldVelocity.X)
			{
                Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
                Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}

    public class PhagocyteBuff : ModBuff
    {
        public override string Texture => "Polarities/Content/Buffs/PreHardmode/Desiccating";

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<PolaritiesNPC>().phagocytes = true;
        }
    }
}