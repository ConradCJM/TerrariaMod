using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace SomethingCreative.Content.Projectiles.AntiLegoAFKBoss
{
    public class BossBullet1 : ModProjectile
    {
        private Vector2 velocity;
        private bool isDecelerating = false;
        public override void SetDefaults()
        {
            Projectile.width = 82;
            Projectile.height = 82;
            Projectile.scale = 0.5f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600; // 10 seconds
        }
        public override void AI()
        {
            if (Projectile.ai[0] % 45 == 0 && Projectile.ai[0] > 1) {
                isDecelerating = !isDecelerating;
            }

            Projectile.scale = 0.65f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1) {
                velocity = Projectile.velocity;
            }

            if (isDecelerating)
            {
                Projectile.velocity *= 0.96f;
            }
            else
            {
                Projectile.velocity = velocity;
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
    }
}
