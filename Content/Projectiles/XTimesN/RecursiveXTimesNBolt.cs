using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.XTimesN
{
    public class RecursiveXTimesNBolt : ModProjectile
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
                new Color(Main.rand.Next(0,255), Main.rand.Next(0, 255), Main.rand.Next(0, 255)),
                1.5f
            );
            Dust d2 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemAmber,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );
            Dust d3 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemAmethyst,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );
            Dust d4 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemEmerald,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );
            Dust d5 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemRuby,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );
            Dust d6 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemSapphire,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );
            Dust d7 = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemTopaz,
                Main.rand.NextVector2Circular(4f, 4f),
                150,
                default,
                1.5f
            );

            d.noGravity = true;
            d2.noGravity = true;
            d3.noGravity = true;
            d4.noGravity = true;
            d5.noGravity = true;
            d6.noGravity = true;
            d7.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int damage = damageDone;
            int chance = Main.rand.Next(50,75);
            bool crit = hit.Crit;
            int manaRecoverAmount = 35;
            if (crit) healMana(manaRecoverAmount);

            while (Main.rand.Next(0, 100) < chance)
            {
                
                damage = damage * Main.rand.Next(2, 20);
                target.SimpleStrikeNPC(damage, 0, crit, 0, DamageClass.Magic, false, 0, false);

                if (crit) healMana(manaRecoverAmount--);
                
                chance--;
            }
        }
        private void healMana(int amount)
        {
            Player player = Main.player[Projectile.owner];
            player.statMana += amount;
            player.ManaEffect(amount);
        }
    }
}
