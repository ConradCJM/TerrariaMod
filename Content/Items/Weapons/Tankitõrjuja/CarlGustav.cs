using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Audio;
using System.Runtime.CompilerServices;
using SomethingCreative.Content.Projectiles.Tankitõrjuja;

namespace SomethingCreative.Content.Items.Weapons.Tankitõrjuja
{
    public class CarlGustav : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<Classes.TankitõrjujaDamage>();
            Item.damage = 169;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<CarlGustavHeldProj>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.crit = 6;
            Item.shootSpeed = 0f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var p = player.GetModPlayer<CarlGustavPlayer>();

            if (p.isReloading)
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Restocking {p.reloadTimer / 60} seconds left");
                return false;
            }

            if (p.loadingTimer > 0)
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Loading {p.loadingTimer / 60} seconds left");
                return false;
            }
            
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("IronBar", 40);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    public class CarlGustavHeldProj : ModProjectile
    {
        int chargeTimer = 0;
        bool readyToFire = false;
        public override void SetDefaults()
        {
            Projectile.width = 670;
            Projectile.height = 207;
            Projectile.scale = 0.25f;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            
            

            Projectile.timeLeft = 2;
        }
        void DrawLaser(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            direction.Normalize();

            // spacing between dust particles
            float step = 4f;

            for (float i = 0; i < length; i += step)
            {
                Vector2 pos = start + direction * i;

                Dust d = Dust.NewDustPerfect(
                    pos,
                    DustID.GemRuby,
                    Vector2.Zero,
                    0,
                    Color.Red,
                    1.2f
                );

                d.noGravity = true;
                d.fadeIn = 0.4f;
            }
        }

        NPC FindClosestNPCToMouse(float maxDist = 67f)
        {
            NPC closest = null;
            float closestDist = maxDist;

            Vector2 mouseWorld = Main.MouseWorld;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.life <= 0)
                    continue;

                float dist = Vector2.Distance(mouseWorld, npc.Center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = npc;
                }
            }

            return closest;
        }


        public override void DrawBehind(int index, List<int> behindNPCsAndTiles,
    List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
    List<int> overWiresUI)
        {
            overPlayers.Add(index); // draw on top of player
        }




        public override void AI()
        {
            Projectile.ai[0]++;
            
            Player player = Main.player[Projectile.owner];

            //force the player to stay on the Carl Gustav item slot
            player.selectedItem = player.FindItem(ModContent.ItemType<CarlGustav>());

            player.itemTime = 2;
            player.itemAnimation = 2;

            var p = player.GetModPlayer<CarlGustavPlayer>();

            //keep projectile alive
            Projectile.timeLeft = 2;

            NPC target = FindClosestNPCToMouse();

            Vector2 aim;
            if (target != null)
            {
                aim = player.DirectionTo(target.Center);
                if (Projectile.ai[0] % 12 == 0)
                    DrawLaser(player.Center, target.Center);
            }
            else
                aim = player.DirectionTo(Main.MouseWorld);

            Projectile.rotation = aim.ToRotation();


            //draw projectile
            Vector2 offset = new (-252, -15);

            Projectile.Center = player.MountedCenter + offset;

            //flip sprite when aiming left
            Projectile.spriteDirection = aim.X < 0 ? -1 : 1;

            //counter‑rotate the sprite when flipped so it doesn't appear upside‑down
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            //make player face the aim direction
            player.direction = aim.X >= 0 ? 1 : -1;

            //make the player's arm rotate toward the mouse
            player.itemRotation = aim.ToRotation();

            bool holdingM1 = player.channel;

            //start charging
            if (holdingM1)
            {
                if (chargeTimer == 0)
                    
                CombatText.NewText(player.Hitbox, Color.LightYellow, "Preparing to Fire!");

                chargeTimer++;

                if (chargeTimer >= 60)
                {
                    if (!readyToFire)
                        CombatText.NewText(player.Hitbox, Color.LightYellow, "Ready to Fire!");

                    readyToFire = true;
                }
                else if (chargeTimer < 60 && chargeTimer % 6 == 0)
                {
                    SoundEngine.PlaySound(SoundID.DrumClosedHiHat with { Pitch = -1.2f + (chargeTimer / 60f) * 0.6f, Volume = 0.2f}, player.Center);
                }
            }
            else
            {
                //release
                if (readyToFire)
                {
                    TryFire(player, p);
                }

                //reset
                chargeTimer = 0;
                readyToFire = false;

                Projectile.Kill(); // stop holding projectile
            }
        }

        void TryFire(Player player, CarlGustavPlayer p)
        {
            if (p.isReloading)
                return;

            //consume ammo
            p.ammoCount--;
            p.loadingTimer = 120;
            SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { Pitch = -0.3f, PitchVariance = 0.2f }, player.Center);
            if (p.ammoCount <= 0)
            {
                p.isReloading = true;
                p.reloadTimer = 600;
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Restocking! Ammo: {p.ammoCount}");
            }
            else
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Loading Ammo! Ammo: {p.ammoCount}");
            }

            //fire projectile
            NPC target = FindClosestNPCToMouse();

            Vector2 velocity;
            if (target != null)
                velocity = player.DirectionTo(target.Center) * 10f;
            else
                velocity = player.DirectionTo(Main.MouseWorld) * 10f;



            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                velocity,
                ModContent.ProjectileType<CarlGustavProj>(),
                player.GetWeaponDamage(player.HeldItem),
                player.GetWeaponKnockback(player.HeldItem),
                player.whoAmI
            );

            //fire flame effect from back of rifle
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                -velocity,
                ModContent.ProjectileType<ExhaustFlame>(),
                (int)(player.GetWeaponDamage(player.HeldItem) * 0.2f),
                0f,
                player.whoAmI
            );
        }
    }

    public class CarlGustavProj : ModProjectile
    {
        private int damageLeft;
        private bool initialized = false;

        public override void SetDefaults()
        {
            Projectile.width = 237;
            Projectile.height = 26;
            Projectile.scale = 0.25f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
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

        public override void AI()
        {
            if (!initialized)
            {
                damageLeft = Projectile.damage;
                initialized = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.timeLeft < 300)
                Projectile.velocity.Y += 0.001f;

            if (Main.rand.NextBool())
            {
                var d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GemDiamond,
                    Main.rand.NextVector2Circular(5f, 5f),
                    150,
                    default,
                    Main.rand.NextFloat(0.1f, 2f)
                );
                d.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            damageLeft -= 40 + damageLeft/2;
            Projectile.damage = damageLeft;
            if (damageLeft <= 0)
                Projectile.Kill();
        }
    }
    public class CarlGustavPlayer : ModPlayer
    {
        public int ammoCount = 10;
        public int loadingTimer = 0;
        public int reloadTimer = 0;
        public bool isReloading = false;

        public override void PostUpdate()
        {
            Player player = Player;
            if (loadingTimer >= 0 && !isReloading) 
            {
                loadingTimer--;
            }
            if (loadingTimer == 0)
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Ammo Loaded! Ammo: {ammoCount}");
            }
            if (isReloading)
            {
                reloadTimer--;
                if (reloadTimer <= 0)
                {
                    isReloading = false;
                    ammoCount = 10;
                    
                    
                    CombatText.NewText(player.Hitbox, Color.LightYellow, $"Restocked! Now Loading! Ammo: {ammoCount}");
                }
            }
        }
    }


}
