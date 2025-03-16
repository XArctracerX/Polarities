using Microsoft.Xna.Framework;
//using MultiHitboxNPCLibrary;
//using Polarities.NPCs;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Items.Weapons.Melee.Warhammers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Flawless
{
    public class BonyBackhand : WarhammerBase
    {
        public override int HammerLength => 35;
        public override int HammerHeadSize => 14;
        public override int DefenseLoss => 16;
        public override int DebuffTime => 1200;
        public override bool CollideWithTiles => false;
        public override float MainSwingAmount => MathHelper.TwoPi * 2;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(50, 18, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 44;
            Item.height = 46;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = WarhammerUseStyle;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 4);
            Item.rare = RarityType<SkeletronFlawlessRarity>();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //base.OnHitNPC(player, target, Projectile.damage, PknockBack, crit);

            for (int i = 0; i < 2; i++)
            {
                Vector2 hitboxDisplacement = GetHitboxCenter(player);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), hitboxDisplacement, (Main.MouseWorld - hitboxDisplacement).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.Pi / 16) * 16, ProjectileType<BonyBackhandShard>(), Item.damage / 3, 0f, player.whoAmI);
            }
        }
    }

    public class BonyBackhandShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 10;
            Projectile.height = 10;
            DrawOffsetX = -6;
            DrawOriginOffsetX = 3;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            //Projectile.GetGlobalProjectile<MultiHitboxNPCLibraryProjectile>().badCollision = true;
            //Projectile.GetGlobalProjectile<MultiHitboxNPCLibraryProjectile>().javelinSticking = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate = 0;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }

            return false;
        }

        public int isSticking
        {
            set { Projectile.ai[0] = value; }
            get { return (int)Projectile.ai[0]; }
        }

        public int TargetWhoAmI
        {
            set { Projectile.ai[1] = value; }
            get { return (int)Projectile.ai[1]; }
        }

        public int Timer
        {
            set { Projectile.ai[2] = value; }
            get { return (int)Projectile.ai[2]; }
        }

        public override void AI()
        {
            Timer--;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (isSticking >= 1)
            {
                StickingAI();

                return;
            }
            Projectile.velocity.Y += 0.1f;

        }

        public void StickingAI()
        {

            NPC target = Main.npc[TargetWhoAmI];

            Projectile.Center = target.Center - Projectile.velocity * 2f;
            Projectile.gfxOffY = target.gfxOffY;

            if (Timer <= 0)
            {
                Timer = 30;
                NPC.HitInfo hitInfo = target.CalculateHitInfo(3, 0, false, 0, Projectile.DamageType, false, Main.player[Projectile.owner].luck);
                target.StrikeNPC(hitInfo);

            }

            if (!target.active)
            {

                Projectile.Kill();

            }


        }

        private readonly Point[] stickingArrows = new Point[4];

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TargetWhoAmI = target.whoAmI;
            isSticking = 1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingArrows);
        }
    }
}

