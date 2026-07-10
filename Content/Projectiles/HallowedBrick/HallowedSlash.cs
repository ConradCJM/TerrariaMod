using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Projectiles.HallowedBrick
{
    public class HallowedSlash : ModProjectile
    {
        
        public override void SetDefaults()
        {
            Projectile.height = 34;
            Projectile.width = 34;
            Projectile.scale = 3f;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.alpha = 0;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
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
            for (int i = 0; i < Main.rand.Next(4,6);i++) { Dust.NewDustPerfect(target.Center, DustID.GemDiamond, Main.rand.NextVector2Circular(3f, 3f), 150, default, 1f); }
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing with { Pitch = 1.2f, PitchVariance = 0.2f }, target.Center);

            if (hit.Crit)
            {
                SoundEngine.PlaySound(SoundID.DeerclopsIceAttack with { Pitch = 1.2f, PitchVariance = 0.2f }, target.Center);
                int extraDamage = damageDone * 13;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                for (int j = 0; j < 10; j++)
                {
                    Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.HallowedWeapons,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.HallowSpray,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    d2.noGravity = true;
                    d.noGravity = true;
                }
            }
        }



        public override void AI()
        {
            

            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 1f);


            
        }
    }
   
}

