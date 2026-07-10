using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;

namespace SomethingCreative.Content.Projectiles.Pumpkins
{
    public class VoidPumpkinProjectile : ModProjectile
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
            Projectile.ai[0]++;
            Projectile.rotation += 0.2f;

            

            Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.Wraith,
                            Main.rand.NextVector2Circular(5f, 5f),
                            150,
                            default,
                            2f
                        );
            
            d.noGravity = true;
            int sporeDamage = Projectile.damage * 3;
            if (Projectile.ai[0] % 15 == 0) { 
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(7f, 7f), ModContent.ProjectileType<VoidSpore>(), sporeDamage, Projectile.knockBack, Projectile.owner);
            }
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
            int blackholeDamage = Projectile.damage * 6;
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<VoidPumpkinBlackhole>(),
                blackholeDamage,
                Projectile.knockBack,
                Projectile.owner
            );

            SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen with { Pitch = -0.5f , PitchVariance = 0.3f, Volume = 1.4f}, Projectile.Center);
        }
        public class VoidPumpkinBlackhole : ModProjectile
        {
            private float rotationMod = Main.rand.NextFloat(-0.2f, 0.2f);
            public override void SetDefaults()
            {
                Projectile.width = 140;
                Projectile.height = 140;
                Projectile.scale = 5f;
                Projectile.friendly = true;
                Projectile.tileCollide = false;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 250;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 50;
                Projectile.ArmorPenetration = 100;
                Projectile.alpha = 50;
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
            public override void AI()
            {
                
                Projectile.ai[0]++;
                Projectile.rotation += rotationMod;
                

                if (Projectile.ai[0] % 25 == 0)
                {
                    int sporeDamage = Projectile.damage / 2;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(), 
                        Projectile.Center, Main.rand.NextVector2Circular(10f, 10f), 
                        ModContent.ProjectileType<VoidSpore>(), 
                        sporeDamage, 
                        Projectile.knockBack, 
                        Projectile.owner);
                    
                }

                if (Projectile.ai[0] % 25 == 0) { SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy with { Pitch = 1f, PitchVariance = 0.2f , Volume = 0.7f}, Projectile.Center); }
            }

            public override void OnKill(int timeLeft)
            {
                int explosionDamage = Projectile.damage * 4;
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { PitchVariance = 0.1f}, Projectile.Center);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<VoidPumpkinBlackholeExplosion>(),
                    explosionDamage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        public class VoidPumpkinBlackholeExplosion : ModProjectile
        {
            public override void SetDefaults()
            {
                Projectile.width = 140;
                Projectile.height = 140;
                Projectile.scale = 6f;
                Projectile.friendly = true;
                Projectile.tileCollide = false;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 30;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 60;
                Projectile.ArmorPenetration = 300;
                Projectile.alpha = 255;
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
            public override void AI()
            {
                Projectile.ai[0]++;
                for (int i = 0; i < 12; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                                        Projectile.Center,
                                        DustID.Wraith,
                                        Main.rand.NextVector2Circular(60f, 60f),
                                        150,
                                        default,
                                        5f
                        );
                    d.noGravity = true;
                }
                int sporeDamage = Projectile.damage / (2 * 4);
                if (Projectile.ai[0] % 5 == 0)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center, Main.rand.NextVector2Circular(10f, 10f),
                        ModContent.ProjectileType<VoidSpore>(),
                        sporeDamage,
                        Projectile.knockBack,
                        Projectile.owner);
                }
            }
            
        }
        public class VoidSpore : ModProjectile
        {
            private float rotationMod = Main.rand.NextFloat(-0.2f,0.2f);

            public override void SetDefaults()
            {
                Projectile.width = 32;
                Projectile.height = 32;
                Projectile.scale = 1.3f;
                Projectile.friendly = true;
                Projectile.tileCollide = false;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = 2;
                Projectile.timeLeft = 300;

                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 60;

                Projectile.ArmorPenetration = 30;
                Projectile.alpha = 150;
            }
            public override void AI()
            {
                Projectile.ai[0]++;
                Projectile.rotation += rotationMod;
                if (Projectile.ai[0] % 30 == 0)
                {
                    Dust d = Dust.NewDustPerfect(
                                    Projectile.Center,
                                    DustID.Wraith,
                                    Main.rand.NextVector2Circular(5f, 5f),
                                    150,
                                    default,
                                    2f
                    );
                    d.noGravity = true;
                }

                if (Projectile.ai[0] > 60)
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
                           toTarget * 0.5f;                //add homing force
                    }


                }
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                SoundEngine.PlaySound(SoundID.AbigailAttack with { Pitch = -1.5f, PitchVariance = 0.5f, Volume = 0.3f }, Projectile.Center);

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
