using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SomethingCreative.Content.Projectiles.AntiLegoAFKBoss;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;


namespace SomethingCreative.Content.NPCS.Hostile
{
    
    



    [AutoloadBossHead]
    public class AntiLegoAFKBoss : ModNPC
    {
        private const int StateSlot = 0; // npc.ai[0] current attack state
        private const int StateTimerSlot = 1; // npc.ai[1] timer for the current attack state
        private const int SubStateSlot = 2; // npc.ai[2] substate for the current attack state
        private const int PhaseSlot = 3;// current phase of a boss

        public override bool CheckActive()
        {
            // Loop through all players
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];

                // If the player slot is active AND the player is alive,
                // the boss should NOT despawn.
                if (p.active && !p.dead)
                    return false; // do NOT despawn
            }

            // If we reach here, every active player is dead → despawn boss
            return true;
        }


        private void ChangeState(int newState)
        {
            NPC.ai[StateSlot] = newState;
            NPC.ai[StateTimerSlot] = 0;
            NPC.ai[SubStateSlot] = 0;
            NPC.netUpdate = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 600;
            NPC.height = 600;
            NPC.damage = 0;
            NPC.defense = 1000;
            NPC.lifeMax = 670000;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = false;
            
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true; // multiplayer boss bar
            NPCID.Sets.BossBestiaryPriority.Add(Type); // boss classification
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.5f, // shrink bestiary portrait
                PortraitScale = 0.5f, // optional: shrink portrait too
                Velocity = 0f // optional: stop idle animation
            };
        }




        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            bool isAttacking = NPC.ai[StateSlot] == 2 ||
                               NPC.ai[StateSlot] == 3 ||
                               NPC.ai[StateSlot] == 4;

            if (isAttacking)
            {
                NPC.velocity = Vector2.Zero;
            }

            switch ((int)NPC.ai[StateSlot])
            {
                case 0:
                    WaveAttack();
                    break;
                case 1:
                    HoverAroundPlayerProj(player, 0, -400);
                    break;
                case 2:
                    ProjectileSpew();
                    break;

                case 3:
                    SpiralBurst(3,10f);
                    break;
                case 4:
                    GridBurst(player, 8);
                    break;

            }

            NPC.ai[StateTimerSlot]++; // increment timer
        }
        private void HoverAroundPlayerProj(Player player, float hoverX = 0, float hoverY = -250, int damage = 200)
        {
            
            Vector2 hoverOffset = new Vector2(hoverX, hoverY);
            Vector2 targetPos = player.Center + hoverOffset;

            //movement speed
            float speed = 10f;
            float inertia = 40f;

            //direction toward the hover position
            Vector2 move = targetPos - NPC.Center;

            if (move.Length() > speed)
                move = move.SafeNormalize(Vector2.Zero) * speed;

            //smooth movement (inertia) thanks co pilot im a dumbass
            NPC.velocity = (NPC.velocity * (inertia - 1) + move) / inertia;

            if (NPC.ai[StateTimerSlot] % 30 == 0) // every 30 ticks
            {
                Vector2 dir = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);
                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    dir * 10f,
                    ModContent.ProjectileType<BossBullet1>(),
                    damage,
                    0f
                );
            }

            if (NPC.ai[StateTimerSlot] > 300)
                ChangeState(Main.rand.Next(5));

        }

        //ai generated cause when i tried it myself for over an hour i lowkey started crashing out
        private void WaveAttack(int count = 2, int damage = 200)
        {
            if (NPC.ai[StateTimerSlot] % 17 != 0) return;
            NPC.velocity = new Vector2(0, -0.3f);

            float speed = 12f;
            Vector2 bossPos = NPC.Center;
            int projType = ModContent.ProjectileType<BossBullet1>();

            // Progress of attack (0 → 1)
            float t = NPC.ai[StateTimerSlot] / 300f;
            t = MathHelper.Clamp(t, 0f, 1f);

            // Offsets shrink over time
            float horizontalOffset = MathHelper.Lerp(2000f, 0f, t);
            float verticalRange = MathHelper.Lerp(1300f, 0f, t);

            // LEFT WAVES
            for (int j = 0; j < count; j++)
            {
                float y = MathHelper.Lerp(bossPos.Y - verticalRange, bossPos.Y + verticalRange, j / (float)(count - 1));
                Vector2 spawnPos = new Vector2(bossPos.X - horizontalOffset, y);

                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0f, speed), projType, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0f, -speed), projType, damage, 0f, Main.myPlayer);
            }

            // RIGHT WAVES
            for (int j = 0; j < count; j++)
            {
                float y = MathHelper.Lerp(bossPos.Y - verticalRange, bossPos.Y + verticalRange, j / (float)(count - 1));
                Vector2 spawnPos = new Vector2(bossPos.X + horizontalOffset, y);

                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0f, speed), projType, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0f, -speed), projType, damage, 0f, Main.myPlayer);
            }

            if (NPC.ai[StateTimerSlot] > 300)
                ChangeState(Main.rand.Next(5));
        }



        private void ProjectileSpew(int damage = 200)
        {
            // Fire every 10 ticks
            if (NPC.ai[StateTimerSlot] % 10 == 0)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player p = Main.player[i];

                    if (!p.active || p.dead)
                        continue;

                    //base direction toward player
                    Vector2 baseDir = (p.Center - NPC.Center).SafeNormalize(Vector2.Zero);

                    //random angle offset inside +- X degrees
                    float coneDegrees = 15.5f;
                    float coneRadians = MathHelper.ToRadians(coneDegrees);
                    float randomAngle = Main.rand.NextFloat(-coneRadians, coneRadians);

                    Vector2 finalDir = baseDir.RotatedBy(randomAngle);

                    //random speed between 8 and 14
                    float speed = Main.rand.NextFloat(8f, 14f);

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        finalDir * speed,
                        ModContent.ProjectileType<BossBullet1>(),
                        damage,
                        0f,
                        Main.myPlayer
                    );
                }
            }

            //end attack after 180 ticks
            if (NPC.ai[StateTimerSlot] > 180)
                ChangeState(Main.rand.Next(5));
        }


        private void SpiralBurst(int ProjectileCount = 1, float baseSpeed = 12f, int damage = 200)
        {
            NPC.velocity = new Vector2(0, -0.8f);
            NPC.ai[SubStateSlot] += 0.15f; // rotation speed

            for (int i = 0; i < ProjectileCount; i++)
            {
                //1 degree random offset
                float randomOffset = MathHelper.ToRadians(Main.rand.NextFloat(-1f, 1f));

                float angle = NPC.ai[SubStateSlot]
                              + MathHelper.ToRadians(360f / ProjectileCount * i)
                              + randomOffset;

                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * baseSpeed;

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    velocity,
                    ModContent.ProjectileType<BossBullet1>(),
                    damage,
                    0f,
                    Main.myPlayer
                );
            }

            if (NPC.ai[StateTimerSlot] > 300)
                ChangeState(Main.rand.Next(5));
        }

        private void GridBurst(Player player,int count = 12, int damage = 200)
        {
            int projType = ModContent.ProjectileType<BossBullet1>();
            float speed = 14f;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];

                if (!p.active || p.dead)
                    continue;

                float left = p.Center.X - 800f;
                float right = p.Center.X + 800f;
                float top = p.Center.Y - 800f;
                float bottom = p.Center.Y + 800f;

                if (NPC.ai[StateTimerSlot] % 35 == 0)
                {



                    // top → bottom warning
                    for (int j = 0; j < count; j++)
                    {
                        float x = MathHelper.Lerp(left, right, j/ (float)(count - 1));
                        Vector2 spawnPos = new Vector2(x, top);
                        Vector2 velocity = new Vector2(0f, speed);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity, projType, 0, 0f);
                    }

                    // bottom → top warning
                    for (int j = 0; j < count; j++)
                    {
                        float x = MathHelper.Lerp(left, right, j / (float)(count - 1));
                        Vector2 spawnPos = new Vector2(x, bottom);
                        Vector2 velocity = new Vector2(0f, -speed);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity, projType, 0, 0f);
                    }

                    // left → right warning
                    for (int j = 0; j < count; j++)
                    {
                        float y = MathHelper.Lerp(top, bottom, j / (float)(count - 1));
                        Vector2 spawnPos = new Vector2(left, y);
                        Vector2 velocity = new Vector2(speed, 0f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity, projType, 0, 0f);
                    }

                    // right → left warning
                    for (int j = 0; j < count; j++)
                    {
                        float y = MathHelper.Lerp(top, bottom, j / (float)(count - 1));
                        Vector2 spawnPos = new Vector2(right, y);
                        Vector2 velocity = new Vector2(-speed, 0f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity, projType, 0, 0f);
                    }

                }
            }
            // End attack after 4 seconds
            if (NPC.ai[StateTimerSlot] > 240)
                ChangeState(Main.rand.Next(5));
        }
        
        










    }


}
