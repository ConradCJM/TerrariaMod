using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SomethingCreative.Content.Projectiles.LegoBrick;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class LegoBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(40, 25);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 500;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 20;

            Item.useAnimation = 30;
            Item.useTime = 6;

            Item.scale = 1.3f;
            Item.crit = -30;
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind<ModRarity>("CosmicPurple", out ModRarity cosmicPurple))
                {
                    Item.rare = cosmicPurple.Type;

                }
                else
                {
                    Item.rare = ItemRarityID.Master;
                }
            }
            else
            {
                Item.rare = ItemRarityID.Master;
            }
            Item.value = 670000;
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<LegoProjectile>();


            Item.ArmorPenetration = 50;
        }

        public override void AddRecipes()
        {
            
            Recipe r = CreateRecipe();
            r.AddIngredient(ItemID.LunarBar, 25).AddIngredient(ModContent.ItemType<BricksWrath>(), 1);
            //calamity mod recipe
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind<ModItem>("CosmiliteBar", out ModItem cosmiliteBar) && calamity.TryFind<ModTile>("CosmicAnvil", out ModTile cosmicAnvil))
                {
                    r.AddIngredient(cosmiliteBar.Type, 25).AddTile(cosmicAnvil.Type);

                }
                else
                {
                    r.AddIngredient(ItemID.Cactus);//for testing and now as a joke item in the recipe if some weird reason the calamity mod is installed but the recipe is not found. this should never happen but just in case
                }
            }
            else
            {
                //no calamity mod recipe
                r.AddIngredient(ModContent.ItemType<TerraBrick>(), 1).AddIngredient(ModContent.ItemType<NightsBrick>(), 1).AddIngredient(ModContent.ItemType<EnchantedBrick>(), 1).AddIngredient(ModContent.ItemType<DirtBrick>(), 1).AddTile(TileID.LunarCraftingStation);
            }
            


            r.Register();
            
            //works in calamity and base game because of zenith recipe rework in calamity
            CreateRecipe().AddIngredient(ItemID.Zenith).AddIngredient(ModContent.ItemType<BricksWrath>()).AddTile(TileID.LunarCraftingStation).Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 direction = Main.MouseWorld - player.Center;

            //flip player based on mouse direction
            player.direction = direction.X >= 0 ? 1 : -1;
        }

        

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
           Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<LegoBrickPlayer>();
            //Idea: after 12 shots shoots a large projectile that does 10x damage and has a large aoe. towards cursor. projectile explodes into smaller bricks
            //Idea: in a circle that is 200 pixels radius around the cursor, spawn projectiles that move towards the cursor. spawn location is random but on the circles perimeter. spawn 3 projectiles everytime

            //spawn projectiles around cursor here
            Vector2 cursor = Main.MouseWorld;
            float radius = 400f;

            for (int i = 0; i < 1; i++)
            {
                
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);

                Vector2 spawnPos = cursor + new Vector2(
                    (float)System.Math.Cos(angle),
                    (float)System.Math.Sin(angle)
                ) * radius;
                SoundEngine.PlaySound(SoundID.MaxMana with { Volume = 1.0f, Pitch = -0.5f, MaxInstances = 25}, spawnPos);
                Vector2 vel = (cursor - spawnPos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;

                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    vel,
                    type,        
                    damage,
                    knockback,
                    player.whoAmI
                );

                for (int j = 0; j < 10; j++)
                {
                    Dust d = Dust.NewDustPerfect(
                        spawnPos,
                        DustID.GemRuby,
                        Main.rand.NextVector2Circular(20f, 20f),
                        150,
                        default,
                        14f
                    );
                    Dust d2 = Dust.NewDustPerfect(
                        spawnPos,
                        DustID.HeatRay,
                        Main.rand.NextVector2Circular(20f, 20f),
                        150,
                        default,
                        1.4f
                    );
                    Dust d3 = Dust.NewDustPerfect(
                        spawnPos,
                        DustID.GemTopaz,
                        Main.rand.NextVector2Circular(20f, 20f),
                        150,
                        default,
                        1.4f
                    );
                    d3.noGravity = true;
                    d2.noGravity = true;
                    d.noGravity = true;
                }
            }

            if (modPlayer.projectileCooldown > 0 && modPlayer.projectileTimerCooldown > 0)
            {
                modPlayer.projectileCooldown -= 1;
                return false;
            }
            SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen with { Volume = 2.0f, Pitch = -0.5f, MaxInstances = 12 }, player.Center);

            modPlayer.projectileCooldown = 13*6;
            modPlayer.projectileTimerCooldown = 60 * 20;

            Vector2 v = (cursor - player.Center).SafeNormalize(Vector2.Zero) * Item.shootSpeed * 0.9f;

            int projectileDamage = Item.damage * 5;

            ModProjectile modProjectile = ModContent.GetModProjectile(ModContent.ProjectileType<BigLegoProjectile>());

            Projectile.NewProjectile(
                    source,
                    player.Center,
                    v,
                    modProjectile.Type,        
                    projectileDamage,
                    knockback,
                    player.whoAmI
                );

            return false;
        }





        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            if (hit.Crit)
            {

                int extraDamage = damageDone * 10;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
                SoundEngine.PlaySound(SoundID.Dig, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.GemRuby,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.HeatRay,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    Dust d3 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.GemTopaz,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f

                 );
                    d3.noGravity = true;
                    d2.noGravity = true;
                    d.noGravity = true;
                }
            }
        }


    }
    public class LegoBrickPlayer : ModPlayer
    {
        public int projectileCooldown = 0;
        public int projectileTimerCooldown = 0;

        public override void ResetEffects()
        {
            if (projectileTimerCooldown > 0)
            {
                projectileTimerCooldown--;
            }
        }
    }
}
