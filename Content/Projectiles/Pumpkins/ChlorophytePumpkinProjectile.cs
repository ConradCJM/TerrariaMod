using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;

namespace SomethingCreative.Content.Projectiles.Pumpkins
{
    public class ChlorophytePumpkinProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.scale = 0.75f;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC target = null;
            float closestDist = 400f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                    }
                }
            }


            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                toTarget.Normalize();


                Projectile.velocity = Projectile.velocity * 1f +
                   toTarget * 5f;                //add homing force
            }

            Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.Chlorophyte,
                            Main.rand.NextVector2Circular(5f, 5f),
                            150,
                            default,
                            2f
                        );
            Dust d2 = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.ChlorophyteWeapon,
                            Main.rand.NextVector2Circular(5f, 5f),
                            150,
                            default,
                            2f
                        );
            d.noGravity = true;
            d2.noGravity = true;

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);


            float alphaMult = (255 - Projectile.alpha) / 255f;


            Color drawColor = lightColor * alphaMult;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            int numProjectiles = Main.rand.Next(4,8); // Number of projectiles to spawn
            for (int i = 0; i < numProjectiles; i++)
            {
                float angle = MathHelper.ToRadians(360f / numProjectiles * i);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 10f; // Adjust speed as needed
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<ChlorophyteSpore>(), Projectile.damage/6, Projectile.knockBack, Projectile.owner);
            }

            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact with { Pitch = 0.5f, PitchVariance = 0.25f }, Projectile.Center);
            
        }
        public class ChlorophyteSpore : ModProjectile
        {


            public override void SetDefaults()
            {
                Projectile.width = 16;
                Projectile.height = 32;
                Projectile.scale = 1.2f;
                Projectile.friendly = true;
                Projectile.tileCollide = false;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 300;

                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 20;

                Projectile.ArmorPenetration = 1000;
                

            }
            public override void AI()
            {
                Projectile.ai[0]++;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (Projectile.ai[0] % 2 == 0)
                {
                    Dust d = Dust.NewDustPerfect(
                                    Projectile.Center,
                                    DustID.Chlorophyte,
                                    Main.rand.NextVector2Circular(5f, 5f),
                                    150,
                                    default,
                                    2f
                                );
                    Dust d2 = Dust.NewDustPerfect(
                                    Projectile.Center,
                                    DustID.ChlorophyteWeapon,
                                    Main.rand.NextVector2Circular(5f, 5f),
                                    150,
                                    default,
                                    2f
                                );
                    d.noGravity = true;
                    d2.noGravity = true;
                }

                if (Projectile.ai[0] > 30)
                {
                    NPC target = null;
                    float closestDist = 1000f;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy())
                        {
                            float dist = Vector2.Distance(npc.Center, Projectile.Center);
                            if (dist < closestDist)
                            {
                                closestDist = dist;
                                target = npc;
                            }
                        }
                    }


                    if (target != null)
                    {
                        Vector2 toTarget = target.Center - Projectile.Center;
                        toTarget.Normalize();


                        Projectile.velocity = Projectile.velocity * 1f +
                           toTarget * 5f;                //add homing force
                    }

                    
                }
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                Projectile.timeLeft -= 20;
                SoundEngine.PlaySound(SoundID.AbigailAttack with { Pitch = 1.5f, PitchVariance = 0.5f }, Projectile.Center);

            }

            public override bool PreDraw(ref Color lightColor)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

                Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);


                float alphaMult = (255 - Projectile.alpha) / 255f;


                Color drawColor = lightColor * alphaMult;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    drawColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );

                return false;
            }
        }
    }
}
