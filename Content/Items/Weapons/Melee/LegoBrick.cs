using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class LegoBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 500;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 20;

            Item.useAnimation = 10;
            Item.useTime = 10;

            Item.scale = 1f;
            Item.crit = -30;
            Item.rare = ItemRarityID.Master;
            Item.value = 670000*5;
            Item.shootSpeed = 25f;

        }

        public override void AddRecipes()
        {
            //calamity mod recipe


            //no calamity mod recipe
            CreateRecipe().AddIngredient(ModContent.ItemType<BricksWrath>(), 1).AddIngredient(ModContent.ItemType<TerraBrick>(), 1).AddIngredient(ModContent.ItemType<NightsBrick>(), 1).AddIngredient(ModContent.ItemType<EnchantedBrick>(), 1).AddIngredient(ModContent.ItemType<DirtBrick>(), 1).AddTile(TileID.LunarCraftingStation).Register();

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
            //Idea: after 12 shots shoots a large projectile that does 10x damage and has a large aoe. towards cursor. projectile explodes into smaller bricks
            //Idea: in a circle that is 200 pixels radius around the cursor, spawn projectiles that move towards the cursor. spawn location is random but on the circles perimeter. spawn 3 projectiles everytime

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
        public override void ResetEffects()
        {
            if (projectileCooldown > 0)
            {
                projectileCooldown--;
            }
        }
    }
}
