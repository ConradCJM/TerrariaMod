using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SomethingCreative.Content.Projectiles.Pumpkins
{
    public class FlamingPumpkinProjectile : ModProjectile
    {
        private bool hitFlag = false;
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.scale = 0.5f;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;



        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.LavaMoss,
                                Main.rand.NextVector2Circular(5f, 5f),
                                150,
                                default,
                                1f
                            );
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.timeLeft = 5; // infinite pierce until it doesnt hit an npc for 5 ticks

            target.AddBuff(BuffID.OnFire, 150); 

        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.LavaMoss,
                            Main.rand.NextVector2Circular(12f, 12f),
                            150,
                            default,
                            1f
                        );

            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            int randomAmount = Main.rand.Next(3, 7);

            for (int i = 0; i < randomAmount; i++)
            {
                Vector2 randomVelocity = Main.rand.NextVector2Circular(25f, 25f);
                int randDiv = Main.rand.Next<int>([2, 3, 4]);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randomVelocity, ModContent.ProjectileType<FlamingPumpkinExplosion>(),(int) (Projectile.damage / randDiv), 0f, Projectile.owner);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlamingPumpkinExplosion>(), (int)(Projectile.damage / 3), 0f, Projectile.owner);
        }

    }
        public class FlamingPumpkinExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.scale = 1.75f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Main.rand.Next(51,74);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;

            Projectile.tileCollide = false;

            Projectile.hide = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 150);
        }

        public override void AI()
            
        {
            Projectile.velocity *= 0.9f;
            Dust d3 = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.FlameBurst,
                            Main.rand.NextVector2Circular(1f, 1f),
                            150,
                            default,
                            3f
                        );
            d3.noGravity = true;
            for (int i = 0; i < 4; i++)
            {
                Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.LavaMoss,
                            Main.rand.NextVector2Circular(12f, 12f),
                            150,
                            default,
                            2f
                        );
                Dust d2 = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GemRuby,
                            Main.rand.NextVector2Circular(12f, 12f),
                            150,
                            default,
                            2f
                        );
                

                d.noGravity = true;
                d2.noGravity = true;
               
            }
        }
        
    }
}
