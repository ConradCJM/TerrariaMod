using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SomethingCreative.Content.Projectiles.Pumpkins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.PumpkinFlareGun
{
    public class PumpkinFlareProjectile : ModProjectile
    {
        private bool sticky = false;
        private int stickyTarget = -1;
        private Vector2 stickyPos;
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.scale = 0.4f;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 velocity = new(0, -10f);
            SoundEngine.PlaySound(SoundID.Item74 with { Pitch = 0.6f, PitchVariance = 0.1f}, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<PumpkinFlareFire>(), (int) (Projectile.damage * 0.8f), Projectile.knockBack, Projectile.owner);
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
            if (Projectile.ai[0] % 3 == 0)
            {
                Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.GemRuby,
                                Main.rand.NextVector2Circular(8f, 8f),
                                150,
                                default,
                                1f
                            );
                Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.GemDiamond,
                                Main.rand.NextVector2Circular(8f, 8f),
                                150,
                                default,
                                1f
                            );
                Dust d = Dust.NewDustPerfect(
                                    Projectile.Center,
                                    DustID.GemAmber,
                                    Main.rand.NextVector2Circular(1f, 1f),
                                    150,
                                    default,
                                    2f
                                );
                d.noGravity = true;
            }
            if (sticky)
            {
                if (stickyTarget != -1)
                {
                    NPC npc = Main.npc[stickyTarget];

                    if (npc.active)
                    {
                        Projectile.position = npc.Center - new Vector2(Projectile.width);
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.position = stickyPos;
                }

                Projectile.velocity = Vector2.Zero;
                return;
            }

            if (Projectile.ai[0] > 60)
            {
                Projectile.velocity.Y += 0.23f;
            }
            

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (sticky) return false;
            sticky = true;
            stickyTarget = -1;
            stickyPos = Projectile.position;
            Projectile.velocity = Vector2.Zero;
            return false; 
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                int debuffType = calamityMod.Find<ModBuff>("TrueVulnerabilityHex").Type;
                target.AddBuff(debuffType, 240);
            }
            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.ShadowFlame,240);
            target.AddBuff(BuffID.CursedInferno, 240);

            if (sticky) return;
            sticky = true;
            stickyTarget = target.whoAmI;
            stickyPos = Projectile.position;
            Projectile.velocity = Vector2.Zero;
        }


    }
    public class PumpkinFlareFire : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.light = 0.5f;
            

            Projectile.alpha = 255;
            Projectile.hide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                int debuffType = calamityMod.Find<ModBuff>("TrueVulnerabilityHex").Type;
                target.AddBuff(debuffType, 240);
            }

            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.ShadowFlame, 240);
            target.AddBuff(BuffID.CursedInferno, 240);
        }
        public override void OnKill(int timeLeft)
        {
            int[] pumpkinIdList = new int[]
                {
                    ModContent.ProjectileType<DirtPumpkinProjectile>(),
                    ModContent.ProjectileType<PumpkinProjectile>(),
                    ModContent.ProjectileType<FlamingPumpkinExplosion>(),
                    ModContent.ProjectileType<CrystalSlimePumpkinProjectile>(),
                    ModContent.ProjectileType<ChlorophytePumpkinProjectile>(),
                    ModContent.ProjectileType<VoidPumpkinProjectile>()
                };
            Vector2 spawnPosition = Projectile.Center + new Vector2(0f, -1500f);
            Vector2 velocity = new Vector2(0, 2.5f);
            int damage = Projectile.damage * 40;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, velocity, ModContent.ProjectileType<TrollFacePumpkin>(), damage , Projectile.knockBack, Projectile.owner);
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center);


            for (int i = 0; i < 75; i++)
            {
                Vector2 randomSpawn = new Vector2(Main.rand.NextFloat(-600f, 600f), Main.rand.NextFloat(0f,600f)) + spawnPosition;
                Vector2 randomVelocity = new (Main.rand.NextFloat(-7.5f,7.5f), Main.rand.NextFloat(7.5f,18f));
                
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), randomSpawn, randomVelocity, Main.rand.Next(pumpkinIdList), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[p].timeLeft += 600;
                Dust d = Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.GemRuby,
                                Main.rand.NextVector2Circular(30f, 30f),
                                150,
                                default,
                                2f
                            );
                Dust d2 = Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.GemDiamond,
                                Main.rand.NextVector2Circular(15f, 15f),
                                150,
                                default,
                                1f
                            );
                d.noGravity = true;
                d2.noGravity = true;
                
            }
            
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GemRuby,
                            Main.rand.NextVector2Circular(4f, 4f),
                            150,
                            default,
                            2f
                        );
            Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GemDiamond,
                            Main.rand.NextVector2Circular(4f, 4f),
                            150,
                            default,
                            1f
                        );
            Dust d = Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.GemAmber,
                                Main.rand.NextVector2Circular(1f, 1f),
                                150,
                                default,
                                3f
                            );
            d.noGravity = true;
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
    public class TrollFacePumpkin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 190;
            Projectile.height = 194;
            Projectile.scale = 7f;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1440;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;

            Projectile.alpha = 55;


        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                int hex = calamityMod.Find<ModBuff>("TrueVulnerabilityHex").Type;
                target.AddBuff(hex, 240);
            }


            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.ShadowFlame, 240);
            target.AddBuff(BuffID.CursedInferno, 240);
        }

        public override void AI()
        {

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
            SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen with { Pitch = -0.5f, PitchVariance = 0.3f, Volume = 1.4f }, Projectile.Center);
        }
    }
}
