using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using SomethingCreative.Content.Projectiles.EnchantedBrick;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class EnchantedBrick : ModItem
    {
        
        
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 10;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 7;

            Item.useAnimation = 35;
            Item.useTime = 35;

            Item.scale = 1.5f;
            Item.crit = -10;
            Item.rare = ItemRarityID.Green;
            Item.value = 0;

            Item.shoot = ModContent.ProjectileType<EnchantedBrickProjectile>(); 
            Item.shootSpeed = 24f;

            Item.noUseGraphic = false;
            Item.noMelee = false;

            Item.ArmorPenetration = 5;
        }

        //make the swing face the cursor
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 direction = Main.MouseWorld - player.Center;
            //player.itemRotation = direction.ToRotation();

            //flip player based on mouse direction
            player.direction = direction.X >= 0 ? 1 : -1;
        }

        //shoot projectile toward cursor
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<BrickModPlayer>();
            if (modPlayer.projectileCooldown > 0)
            {
                return false;
            }
            Vector2 direction = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            velocity = direction * Item.shootSpeed;

            Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.MaxMana, player.Center);

            modPlayer.projectileCooldown = 60;
            return false; 
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ModContent.ItemType<DirtBrick>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                int extraDamage = damageDone * 7;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);

                SoundEngine.PlaySound(SoundID.AbigailAttack, target.Center);

                for (int i = 0; i < 20; i++)
                {
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.MagicMirror,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    d2.noGravity = true;
                }
            }
        }
    }
    public class BrickModPlayer : ModPlayer
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
