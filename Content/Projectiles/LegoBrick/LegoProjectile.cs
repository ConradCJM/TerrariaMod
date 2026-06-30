using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.LegoBrick
{
    public class LegoProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 25;
            Projectile.width = 40;
            Projectile.scale = 1.5f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.alpha = 0;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.DamageType = DamageClass.Melee;
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
            
            if (hit.Crit)
            {
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { Volume = 2f }, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LegoExplosion>(), Projectile.damage * 3, Projectile.knockBack, Projectile.owner);
            }
        }

       

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);


            Dust d = Dust.NewDustPerfect(
             Projectile.Center,
             DustID.GemRuby,
             Main.rand.NextVector2Circular(15f, 15f),
             150,
             default,
             1f
          );
            Dust d2 = Dust.NewDustPerfect(
             Projectile.Center,
             DustID.HeatRay,
             Main.rand.NextVector2Circular(15f, 15f),
             150,
             default,
             1f
          );
            Dust d3 = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.GemTopaz,
                        Main.rand.NextVector2Circular(15f, 15f),
                        150,
                        default,
                        1f
                 );
            d3.noGravity = true;
            d.noGravity = true;
            d2.noGravity = true;
        }
    }
    public class LegoExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 32;
            Projectile.width = 32;
            Projectile.scale = 9f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;

            Projectile.alpha = 255;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 19;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 240);

            if (hit.Crit)
            {
                int extraDamage = damageDone * 3;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                for (int i = 0; i < 10; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                    target.Center,
                    DustID.GemRuby,
                    Main.rand.NextVector2Circular(10f, 10f),
                    150,
                    default,
                    1.4f

                 );
                    Dust d2 = Dust.NewDustPerfect(
                     target.Center,
                     DustID.HeatRay,
                     Main.rand.NextVector2Circular(10f, 10f),
                     150,
                     default,
                     1.4f

                 );
                    Dust d3 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.GemTopaz,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f

                 );
                    d3.noGravity = true;
                    d.noGravity = true;
                    d2.noGravity = true;


                }
            }
        }


        public override void AI()
        {

            Projectile.ai[0]++;

            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);

            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustPerfect(
                 Projectile.Center,
                 DustID.GemRuby,
                 Main.rand.NextVector2Circular(20f, 20f),
                 150,
                 default,
                 3f

              );
                Dust d2 = Dust.NewDustPerfect(
                 Projectile.Center,
                 DustID.HeatRay,
                 Main.rand.NextVector2Circular(20f, 20f),
                 150,
                 default,
                 3f

              );
                Dust d3 = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GemTopaz,
                            Main.rand.NextVector2Circular(20f, 20f),
                            150,
                            default,
                            3f

                     );
                d3.noGravity = true;
                d.noGravity = true;
                d2.noGravity = true;
            }
        }
    }
}

