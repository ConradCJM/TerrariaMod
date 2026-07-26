using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using SomethingCreative.Content.Projectiles.Tankitõrjuja;

namespace SomethingCreative.Content.Items.Weapons.Tankitõrjuja
{
    public class Igla : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<Classes.TankitõrjujaDamage>();
            Item.damage = 367;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<IglaHeldProj>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.crit = 6;
            Item.shootSpeed = 0f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var p = player.GetModPlayer<IglaPlayer>();

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

    public class IglaHeldProj : ModProjectile
    {
        int chargeTimer = 0;
        bool readyToFire = false;

        public override void SetDefaults()
        {
            Projectile.width = 302;
            Projectile.height = 128;
            Projectile.scale = 0.30f;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles,
            List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
            List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        NPC FindStrongestNPCAbove(Player player)
        {
            NPC strongest = null;
            int highestLife = -1;
            float maxDist = 2000f;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.life <= 0)
                    continue;

                // must be above the player
                if (npc.Center.Y >= player.Center.Y)
                    continue;

                float dist = Vector2.Distance(player.Center, npc.Center);
                if (dist > maxDist)
                    continue;

                if (npc.lifeMax > highestLife)
                {
                    highestLife = npc.lifeMax;
                    strongest = npc;
                }
            }

            return strongest;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Lock item slot
            player.selectedItem = player.FindItem(ModContent.ItemType<Igla>());
            player.itemTime = 2;
            player.itemAnimation = 2;

            var p = player.GetModPlayer<IglaPlayer>();

            Projectile.timeLeft = 2;

            NPC targetNPC = FindStrongestNPCAbove(player);

            Vector2 aim;
            if (targetNPC != null)
                aim = player.DirectionTo(targetNPC.Center);
            else
                aim = new Vector2(0, -1); // aim straight up if no target

            Projectile.rotation = aim.ToRotation();

            Vector2 offset = new(-107, -10);
            Projectile.Center = player.MountedCenter + offset;

            Projectile.spriteDirection = aim.X < 0 ? -1 : 1;

            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            player.direction = aim.X >= 0 ? 1 : -1;
            player.itemRotation = aim.ToRotation();

            bool holdingM1 = player.channel;

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
                    SoundEngine.PlaySound(SoundID.DrumClosedHiHat with
                    {
                        Pitch = -1.2f + (chargeTimer / 60f) * 0.6f,
                        Volume = 0.2f
                    }, player.Center);
                }
            }
            else
            {
                if (readyToFire)
                    TryFire(player, p);

                chargeTimer = 0;
                readyToFire = false;

                Projectile.Kill();
            }
        }

        void TryFire(Player player, IglaPlayer p)
        {
            if (p.isReloading)
                return;

            p.ammoCount--;
            p.loadingTimer = 240;

            SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with
            {
                Pitch = -0.3f,
                PitchVariance = 0.2f
            }, player.Center);

            if (p.ammoCount <= 0)
            {
                p.isReloading = true;
                p.reloadTimer = 60*15; //15 seconds
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Restocking! Ammo: {p.ammoCount}");
            }
            else
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Loading Ammo! Ammo: {p.ammoCount}");
            }
            NPC targetNPC = FindStrongestNPCAbove(player);

            Vector2 velocity;
            if (targetNPC != null)
                velocity = player.DirectionTo(targetNPC.Center) * 10f;
            else
                velocity = new Vector2(0, -10f);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                velocity,
                ModContent.ProjectileType<IglaProj>(),
                player.GetWeaponDamage(player.HeldItem),
                player.GetWeaponKnockback(player.HeldItem),
                player.whoAmI
            );

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                -velocity,
                ModContent.ProjectileType<ExhaustFlame>(),
                (int)(player.GetWeaponDamage(player.HeldItem) * 0.4f),
                0f,
                player.whoAmI
            );
        }
    }

    public class IglaProj : ModProjectile
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
            Projectile.extraUpdates = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
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
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.GemAmber,
                Main.rand.NextVector2Circular(1f, 1f),
                150,
                default,
                Main.rand.NextFloat(0.5f, 2f)
            );
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.Flare,
                Main.rand.NextVector2Circular(1f, 1f),
                150,
                default,
                Main.rand.NextFloat(0.5f, 2f)
            );
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //summon custom explosion
        }
    }

    public class IglaPlayer : ModPlayer
    {
        public int ammoCount = 4;
        public int loadingTimer = 0;
        public int reloadTimer = 0;
        public bool isReloading = false;

        public override void PostUpdate()
        {
            Player player = Player;

            if (loadingTimer >= 0 && !isReloading)
                loadingTimer--;

            if (loadingTimer == 0)
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Ammo Loaded! Ammo: {ammoCount}");

            if (isReloading)
            {
                reloadTimer--;

                if (reloadTimer <= 0)
                {
                    isReloading = false;
                    ammoCount = 4;

                    CombatText.NewText(player.Hitbox, Color.LightYellow, $"Restocked! Now Loading! Ammo: {ammoCount}");
                }
            }
        }
    }
}
