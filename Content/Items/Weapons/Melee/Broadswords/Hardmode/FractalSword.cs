using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ReLogic.Content;

namespace Polarities.Content.Items.Weapons.Melee.Broadswords.Hardmode
{
	public class FractalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 78;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.height = 32;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(silver: 50);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<FractalSwordShot>();
			Item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers){
			float diff=20f;
			Vector2 position1;
			Vector2 position2;
			//if(player.velocity.Y==0){
			position1 = new Vector2(player.position.X+diff, player.position.Y);
			position2 = new Vector2(player.position.X-diff, player.position.Y);
			//}else{
			//	position1 = new Vector2(player.position.X, player.position.Y+diff);
			//	position2 = new Vector2(player.position.X, player.position.Y-diff);
			//}
			
			Projectile.NewProjectileDirect(
				player.GetSource_FromThis(), 
				position1, 
				player.velocity, 
				Item.shoot, 
				Item.damage,
				Item.knockBack, 
				player.whoAmI
			);
			
			Projectile.NewProjectileDirect(
				player.GetSource_FromThis(), 
				position2, 
				player.velocity, 
				Item.shoot, 
				Item.damage,
				Item.knockBack, 
				player.whoAmI
			);
		}
	}

	public class FractalSwordShot : ModProjectile
	{
		private int mode = -1; // -1 = initialise, 0 = inactive, 1 = attacking, 2 = fading
		private int timer = 90;
		private float time= 0f;
		private float timeDamage = 15f;
		private float dir; // Which direction to slash in
		private float side; // Which side of the player to start at
		
		private static Asset<Texture2D> bladeTex;
		
		public override void Load(){
			bladeTex = ModContent.Request<Texture2D>("Polarities/Content/Items/Weapons/Melee/Broadswords/Hardmode/FractalSwordBlade");
		}
		
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 46;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
			Projectile.alpha = 15;

			Projectile.timeLeft = timer;
		}

		private float smooth(float x, float edge0, float edge1){ // Adapted from thebookofshaders.com
			float t;
			t = Math.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
			return t * t * (3f - 2f * t);
		}

		public override void AI()
		{
			time+=1f;
			Player player = Main.player[Projectile.owner];
			
			if(mode==-1){
				mode=0;
				dir =-(Projectile.position.X-Main.MouseWorld.X)/Math.Abs(Projectile.position.X-Main.MouseWorld.X);
				side=(Projectile.position.X-player.position.X)/Math.Abs(Projectile.position.X-player.position.X);
				Projectile.spriteDirection = (int)-dir;
			}
			
			Projectile.position.X = player.position.X + side*30f;
			Projectile.position.Y = player.position.Y;
			
			if(mode==1 || mode==2){
				float pace=15f;
				float progress = Math.Clamp(time-timeDamage, 0f, pace);
				Projectile.position.X += 90f*dir*smooth(progress, 0f, pace);
				if(mode==1 && progress==pace){mode=2;Projectile.friendly=false;}
			}
			
			if(mode==0){
				if(Projectile.timeLeft<timer-timeDamage){mode=1;Projectile.friendly=true;}
			}
			
			if(mode==2){
				Projectile.alpha+=16;
				if(Projectile.timeLeft<2){Projectile.timeLeft=2;}
				if(Projectile.alpha>=255){Projectile.timeLeft=0;}
			}
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { // Treat as if Projectile.penetrate=1;
			target.immune[Projectile.owner] = 0;
			Projectile.friendly=false;
			if(mode==1){mode=2;}
		}
		
		public override void ModifyDamageHitbox (ref Rectangle hitbox) {
			if(mode>0){
				float t = (Projectile.timeLeft/12f)%2f +1.1f;
				if(Projectile.spriteDirection==1){t*=-1f;}
				Vector2 point = new Vector2(30f*(float)Math.Sin(t),30f*(float)Math.Cos(t));
				hitbox.X += (int)point.X;
				hitbox.Y += (int)point.Y;
			}
		}
		
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--) {
				Projectile.oldPos[k].Y=Projectile.position.Y;
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			
			// Draw sword here
			Texture2D blade = bladeTex.Value;
			Vector2 bladeOrigin = new Vector2(blade.Width * 0.5f, blade.Height * 0.5f);
			if(mode>0){
				float t = (Projectile.timeLeft/12f)%2f +1f;
				if(Projectile.spriteDirection==1){t*=-1f;}
				Vector2 point = new Vector2(30f*(float)Math.Sin(t), 30f*(float)Math.Cos(t));
				Vector2 drawPos = (Projectile.position - Main.screenPosition + point) + bladeOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor);
				float rot=(float)Math.PI-t;
				if(dir==-1f){
					Main.EntitySpriteDraw(blade, drawPos, null, color, rot, bladeOrigin, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
				}else{
					Main.EntitySpriteDraw(blade, drawPos, null, color, rot, bladeOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}
	}
}