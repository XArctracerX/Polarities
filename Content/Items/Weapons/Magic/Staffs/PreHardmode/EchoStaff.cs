﻿using Microsoft.Xna.Framework;
using Polarities.Content.Buffs.PreHardmode;
using Polarities.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Magic.Staffs.PreHardmode
{
    public class EchoStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(0, 0, 0);
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;

            Item.width = 44;
            Item.height = 44;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item8;

            Item.shoot = ProjectileType<EchoStaffProjectile>();
            Item.shootSpeed = 4f;

            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }
    }

    public class EchoStaffProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Assets/Pixel";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.hide = true;

            Projectile.GetGlobalProjectile<PolaritiesProjectile>().doNotStrikeNPC = true;
        }

        public override void AI()
        {
            Projectile.damage = 1;

            Projectile.frameCounter = (Projectile.frameCounter + 1) % 10;
            if (Projectile.frameCounter == 0)
            {
                int numDusts = 20;
                for (int i = 0; i < numDusts; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Frost, Scale: 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                    Main.dust[dust].velocity = new Vector2(4, 0).RotatedBy(i * MathHelper.TwoPi / numDusts);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.buffImmune[BuffType<Pinpointed>()] = false;
            target.AddBuff(BuffType<Pinpointed>(), 300, true);
            Projectile.Kill();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}