using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.EnchantedBrick
{
    public class EnchantedBrickProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.scale = 1.5f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = 3;
            Projectile.timeLeft = 100;
            Projectile.alpha = 0;


            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            //Main.NewText("Alpha: " + Projectile.alpha);

            Projectile.ai[0]++;

            Projectile.velocity *= 0.98f;
            Projectile.rotation += 0.5f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);

            if (Projectile.ai[0] < 30)
            {
                Dust d = Dust.NewDustPerfect(
                Projectile.Center,
                DustID.MagicMirror,
                Projectile.velocity * -0.2f,
                150,
                default,
                1.2f

             );
                d.noGravity = true;
            }

            if (Projectile.ai[0] > 30)
            {
                Projectile.alpha += 255/60;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }








            if (Projectile.ai[0] == 30)
            {
                stopProjectile();
            }

        }
        private void stopProjectile() {
            Projectile.velocity = Vector2.Zero;
            Projectile.penetrate = 10;

            for (int i = 0; i < 20; i++)
            {
                Dust d2 = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.MagicMirror,
                    Main.rand.NextVector2Circular(10f, 10f),
                    150,
                    default,
                    1.4f
                );
                d2.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
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
            stopProjectile();
            if (hit.Crit)
            {
                int extraDamage = damageDone * 7;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.MagicMirror,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    d2.noGravity = true;
                }
            }
        }



    }

}
