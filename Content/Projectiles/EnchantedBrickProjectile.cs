using Microsoft.Build.Framework.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles
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

            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.Opacity = 0.25f;

            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            Projectile.velocity *= 0.98f;
            Projectile.rotation += 0.5f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            //center of the sprite
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            //draw the projectile manually
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,                //recenters the sprite
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
                int extraDamage = damageDone * 7;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
            }
        }



    }

}
