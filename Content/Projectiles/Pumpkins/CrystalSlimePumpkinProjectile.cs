using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SomethingCreative.Content.Projectiles.Pumpkins
{
    public class CrystalSlimePumpkinProjectile : ModProjectile
    {
        private int bounceCount = 0;
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.scale = 0.75f;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 300;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;


        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            Projectile.velocity.Y += 0.3f;

            Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.CrystalPulse,
                            Main.rand.NextVector2Circular(5f, 5f),
                            150,
                            default,
                            0.7f
                        );

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //bounce X
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }

            //bounce Y
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            


            NPC target = null;
            float closestDist = 800f; 

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                    }
                }
            }

            
            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                toTarget.Normalize();

                
                Projectile.velocity =
                    Projectile.velocity * 0.4f +  //keep % of bounce
                    toTarget * 8f;                //add homing force
            }



            if (bounceCount < 5) { bounceCount++; }
            Projectile.timeLeft -= 30;
            spawnSpores(1);
            SoundEngine.PlaySound(SoundID.SplashWeak with { Pitch = -1f, PitchVariance = 0.5f }, Projectile.Center);
            return false;
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
            spawnSpores(2);
            Projectile.timeLeft -= 3;
            target.AddBuff(BuffID.Slimed,300);
            target.AddBuff(BuffID.GelBalloonBuff, 300);

        }

        private void spawnSpores(int div = 1)
        {
            for (int i = 0; i < bounceCount/div; i++)
            {
                
                Vector2 randomDirection = Main.rand.NextVector2Circular(1f, 1f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randomDirection, ModContent.ProjectileType<CrystalSpore>(),(int) (Projectile.damage * 1.5f), 0f, Projectile.owner);
            }
            
        }
    }
    public class CrystalSpore : ModProjectile
    {
        
        private int rotationVar = Main.rand.Next([-1,1]);
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 44;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Main.rand.Next(120,130);

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 59;

            

        }
        public override void AI()
        {
            Projectile.rotation += 0.05f * rotationVar;
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact with { Pitch = 3f, PitchVariance = 1f }, Projectile.Center);
            target.AddBuff(BuffID.Slimed, 300);
            target.AddBuff(BuffID.GelBalloonBuff, 300);
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
