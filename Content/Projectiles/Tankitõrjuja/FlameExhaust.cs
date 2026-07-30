using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.Tankitõrjuja
{
    public class ExhaustFlame : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 13;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
        }
        public override void AI()
        {
            Projectile.netUpdate = true;
            //spawn dust for visual effect
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemAmber,
                Main.rand.NextVector2Circular(2f, 2f),
                150,
                default,
                Main.rand.NextFloat(1f,2.5f)
            );
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.Flare,
                Main.rand.NextVector2Circular(2f, 2f),
                150,
                default,
                Main.rand.NextFloat(1f, 2.5f)
            );

            // Manual player damage bypassing PvP thanks copilot cause I couldn't figure this out! also fyucj you for giving bad code that didnt work till i did a fuck ton of modifications
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (!p.active || p.dead)
                    continue;

                // Prevent self-hit
                if (p.whoAmI == Projectile.owner)
                {
                    //Main.NewText($"Debug: Exhaust flame hit (Owner): {p.name}");
                    continue;
                }

                // Check hitbox collision
                if (Projectile.Hitbox.Intersects(p.Hitbox))
                {
                    //Main.NewText($"Debug: Exhaust flame hit player: {p.name}");
                    // Damage packet (server-side)
                    if (true)//(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        p.Hurt(
                            PlayerDeathReason.ByCustomReason(
                                NetworkText.FromLiteral($"{p.name} was burned by exhaust flame.")),
                            Projectile.damage, // damage
                            0,  // direction
                            false, // pvp
                            false  // quiet
                        );

                        p.AddBuff(BuffID.OnFire, 300);
                    }
                    else
                    {
                        Main.NewText($"Debug: Failed to do damage to player: {p.name}"); //debug
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
