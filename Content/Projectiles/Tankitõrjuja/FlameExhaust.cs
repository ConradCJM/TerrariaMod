using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.Tankitõrjuja
{
    public class ExhaustFlame : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 10;
            Projectile.alpha = 255;
            Projectile.hostile = true;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            //spawn dust for visual effect
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemAmber,
                Main.rand.NextVector2Circular(2f, 2f),
                150,
                default,
                4f
            );
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.Flare,
                Main.rand.NextVector2Circular(2f, 2f),
                150,
                default,
                4f
            );
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
        public override bool CanHitPlayer(Player target)
        {
            return target.whoAmI != Projectile.owner;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
