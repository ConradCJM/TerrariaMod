using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.TerraBrick
{
    public class TerraBrickProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.scale = 7f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = 100;
            Projectile.timeLeft = 40;
            Projectile.alpha = 120;


            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        

        public override void AI()
        {
            //Main.NewText("Alpha: " + Projectile.alpha);

            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.96f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);

            Dust d = Dust.NewDustPerfect(
             Projectile.Center,
             DustID.Terra,
             Main.rand.NextVector2Circular(30f, 30f),
             150,
             default,
             3f

          );
            Dust d2 = Dust.NewDustPerfect(
             Projectile.Center,
             DustID.TerraBlade,
             Main.rand.NextVector2Circular(30f, 30f),
             150,
             default,
             3f

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


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                int extraDamage = damageDone * 5;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                    target.Center,
                    DustID.Terra,
                    Main.rand.NextVector2Circular(10f, 10f),
                    150,
                    default,
                    1.4f

                 );
                    Dust d2 = Dust.NewDustPerfect(
                     target.Center,
                     DustID.TerraBlade,
                     Main.rand.NextVector2Circular(10f, 10f),
                     150,
                     default,
                     1.4f

                 );
                    d.noGravity = true;
                    d2.noGravity = true;
                }
            }
        }
    }
}
