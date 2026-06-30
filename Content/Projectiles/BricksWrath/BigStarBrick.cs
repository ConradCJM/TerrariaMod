using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace SomethingCreative.Content.Projectiles.BricksWrath
{
    public class BigStarBrickExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.scale = 9f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = 50;
            Projectile.timeLeft = 10;
            
            Projectile.alpha = 255;

            Projectile.DamageType = DamageClass.Melee;


        }

        public override void AI()
        {
            //dust
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.TeleportationPotion,
                    Main.rand.NextVector2Circular(66f, 66f),
                    150,
                    default,
                    5f);
                Dust d2 = Dust.NewDustPerfect(
                     Projectile.Center,
                     DustID.HeatRay,
                     Main.rand.NextVector2Circular(66f, 66f),
                     150,
                     default,
                     3f

                 );
                Dust d3 = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.Demonite,
                        Main.rand.NextVector2Circular(66f, 66f),
                        150,
                        default,
                        5f

                 );
                d3.noGravity = true;
                d.noGravity = true;
                d2.noGravity = true;

            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                int extraDamage = damageDone * 4;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                    target.Center,
                    DustID.TeleportationPotion,
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
                        DustID.Demonite,
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
    }
        public class BigStarBrick : ModProjectile
        {

            public override void SetDefaults()
            {

                Projectile.width = 32;
                Projectile.height = 32;

                Projectile.scale = 9f;

                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.tileCollide = false;

                Projectile.penetrate = 1;
                Projectile.timeLeft = 240;
                Projectile.alpha = 0;


                Projectile.DamageType = DamageClass.Melee;
            }


            public override void AI()
            {
                //Main.NewText("Alpha: " + Projectile.alpha);

                Projectile.ai[0]++;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);

                Dust d = Dust.NewDustPerfect(
                 Projectile.Center,
                 DustID.TeleportationPotion,
                 Main.rand.NextVector2Circular(30f, 30f),
                 150,
                 default,
                 3f

              );
                Dust d2 = Dust.NewDustPerfect(
                 Projectile.Center,
                 DustID.HeatRay,
                 Main.rand.NextVector2Circular(30f, 30f),
                 150,
                 default,
                 3f

              );
                Dust d3 = Dust.NewDustPerfect(
                 Projectile.Center,
                 DustID.Demonite,
                 Main.rand.NextVector2Circular(30f, 30f),
                 150,
                 default,
                 3f

              );
                d.noGravity = true;
                d2.noGravity = true;
                d3.noGravity = true;

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
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center);

            //summon explosion here as projectile
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BigStarBrickExplosion>(), Projectile.damage, 0f, Projectile.owner); // explosion


                //summon star projectiles
                for (int i = 0; i < 20; i++) 
                {
                    var randomAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                    var direction = new Vector2((float)System.Math.Cos(randomAngle), (float)System.Math.Sin(randomAngle));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * 10f, ModContent.ProjectileType<StarBrick>(), Projectile.damage/20, 0f, Projectile.owner); // star brick

            }

            }



            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {


                if (hit.Crit)
                {
                    int extraDamage = damageDone * 4;
                    target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                    SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                    for (int i = 0; i < 20; i++)
                    {
                        Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.TeleportationPotion,
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
                            DustID.Demonite,
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
                Projectile.Kill();





            }
        }
    
}
