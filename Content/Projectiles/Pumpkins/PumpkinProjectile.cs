using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SomethingCreative.Content.Projectiles.Pumpkins
{
    public class PumpkinProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.scale = 0.6f;
            
            Projectile.friendly = true; 
            Projectile.DamageType = DamageClass.Ranged; 
            Projectile.penetrate = 1; 
            Projectile.timeLeft = 300;
            

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.Pumpkin,
                            Main.rand.NextVector2Circular(5f, 5f),
                            150,
                            default,
                            1f
                        );

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
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.Pumpkin,
                            Main.rand.NextVector2Circular(12f, 12f),
                            150,
                            default,
                            1f
                        );

            }

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<PumpkinProjectileExplosion>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }
    }
    public class PumpkinProjectileExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1; 
            Projectile.timeLeft = 90;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 29;

            Projectile.alpha = 255;

        }
        public override void AI()
        {
            Projectile.ai[0]++;
            // Create dust effect for the explosion

            
            if (Projectile.ai[0] % 10 == 0 || Projectile.ai[0] < 3)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.JungleSpore,
                    Main.rand.NextVector2Circular(20f, 20f),
                    150,
                    new Color(255, 32, 0, 1),
                    6f
                );
                d.noGravity = true;
            }
        }
    }
}
