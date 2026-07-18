using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.XPlusN
{
    public class RecursiveAddBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 10;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.alpha = 100;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

        }
        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemDiamond,
                Main.rand.NextVector2Circular(0.1f, 0.1f),
                150,
                default,
                1.5f
            );
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int damage = damageDone;
            int chance = 50;
            while (Main.rand.Next(0, 100) < chance)
            {
                damage = damage + Main.rand.Next(0,damage*2);
                target.SimpleStrikeNPC(damage, 0, true, 0, DamageClass.Magic, false, 0, false);
                chance--;
            }
        }
    }
}
