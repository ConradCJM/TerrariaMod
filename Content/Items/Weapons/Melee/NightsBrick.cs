using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.NightsBrick;
using System.Security.Cryptography.Xml;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class NightsBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 7;

            Item.useAnimation = 35;
            Item.useTime = 35;

            Item.scale = 2.25f;
            Item.crit = -15;
            Item.rare = ItemRarityID.Purple;
            Item.value = 67;
            Item.shootSpeed = 30f;
            Item.shoot = ModContent.ProjectileType<NightsBrickProjectile>();

        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<EnchantedBrick>(), 1).AddIngredient(ItemID.DemoniteBar, 15).AddIngredient(ItemID.ShadowScale, 10).AddTile(TileID.DemonAltar).Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<EnchantedBrick>(), 1).AddIngredient(ItemID.CrimtaneBar, 15).AddIngredient(ItemID.TissueSample, 10).AddTile(TileID.DemonAltar).Register();
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
            Vector2 direction = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            velocity = direction * Item.shootSpeed;

            Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.DD2_BetsysWrathShot, player.Center);

            
            return false;
        }





        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {

                int extraDamage = damageDone * 9;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
                SoundEngine.PlaySound(SoundID.Dig, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.Wraith,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.GemAmethyst,
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


    }
}
