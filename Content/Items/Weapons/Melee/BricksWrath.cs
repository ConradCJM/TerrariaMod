using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.BricksWrath;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class BricksWrath : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 100;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 20;

            Item.useAnimation = 23;
            Item.useTime = 23;

            Item.scale = 4f;
            Item.crit = -28;
            Item.rare = ItemRarityID.Master;
            Item.value = 67;
            Item.shootSpeed = 30f;
            Item.shoot = ModContent.ProjectileType<StarBrick>(); //star brick

        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<TerraBrick>(), 1).AddIngredient(ItemID.FragmentVortex, 20).AddIngredient(ItemID.FragmentStardust, 20).AddIngredient(ItemID.FragmentSolar, 20).AddIngredient(ItemID.FragmentNebula, 20).AddTile(TileID.LunarCraftingStation).Register();
            
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 direction = Main.MouseWorld - player.Center;
            //player.itemRotation = direction.ToRotation();

            //flip player based on mouse direction
            player.direction = direction.X >= 0 ? 1 : -1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
           Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<BricksWrathPlayer>();
            
            float randomX = Main.rand.NextFloat(-200f, 200f);

            Vector2 spawnLocation = player.Center + new Vector2(randomX, -600f);

            Vector2 direction = (Main.MouseWorld - spawnLocation).SafeNormalize(Vector2.UnitX);

            velocity = direction * Item.shootSpeed;

            Projectile.NewProjectile(source, spawnLocation, velocity, type, damage, knockback, player.whoAmI); // star brick
            SoundEngine.PlaySound(SoundID.DD2_BetsysWrathShot, player.Center);
            
            
            
            randomX = Main.rand.NextFloat(-200f, 200f);

            spawnLocation = player.Center + new Vector2(randomX, -800f);

            direction = (Main.MouseWorld - spawnLocation).SafeNormalize(Vector2.UnitX);

            velocity = direction * Item.shootSpeed;

            Projectile.NewProjectile(source, spawnLocation, velocity, type, damage, knockback, player.whoAmI); // star brick



            randomX = Main.rand.NextFloat(-200f, 200f);

            spawnLocation = player.Center + new Vector2(randomX, -700f);

            direction = (Main.MouseWorld - spawnLocation).SafeNormalize(Vector2.UnitX);

            velocity = direction * Item.shootSpeed;

            Projectile.NewProjectile(source, spawnLocation, velocity, type, damage, knockback, player.whoAmI); // star brick





            if (modPlayer.projectileCooldown <= 0)
            {
                int secondType = ModContent.ProjectileType<BigStarBrick>();


                randomX = Main.rand.NextFloat(-200f, 200f);
                spawnLocation = player.Center + new Vector2(randomX, -1000f);

                direction = (Main.MouseWorld - spawnLocation).SafeNormalize(Vector2.UnitX);

                Vector2 secondaryVelocity = direction * Item.shootSpeed * 0.3f;

                Projectile.NewProjectile(source, spawnLocation, secondaryVelocity, secondType, damage*5, knockback, player.whoAmI); // big star brick
                modPlayer.projectileCooldown = 460;
            }


            return false;
        }





        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {

                int extraDamage = damageDone * 10;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.TeleportationPotion,
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
                        DustID.Demonite,
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
    public class BricksWrathPlayer : ModPlayer
    {
        public int projectileCooldown = 0;
        public override void ResetEffects()
        {
            if (projectileCooldown > 0)
            {
                projectileCooldown--;
            }
        }
    }
}
