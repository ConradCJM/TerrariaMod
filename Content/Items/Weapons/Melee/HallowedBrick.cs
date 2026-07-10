using Microsoft.Xna.Framework;

using SomethingCreative.Content.Projectiles.HallowedBrick;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class HallowedBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(40, 25);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 10;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 20;

            Item.useAnimation = 7;
            Item.useTime = 7;

            Item.scale = 3.22f;
            Item.crit = -18;
            
            Item.rare = ItemRarityID.Yellow;
            
            Item.value = 67;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<HallowedSlash>();
            Item.ArmorPenetration = 100;
        }

        public override void AddRecipes()
        { 
            CreateRecipe().AddIngredient(ItemID.HallowedBar, 15).AddTile(TileID.MythrilAnvil).Register();
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

            Vector2 cursor = Main.MouseWorld;

            Projectile.NewProjectile(
                source,
                cursor,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );


            return false;
        }





        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (hit.Crit)
            {

                int extraDamage = damageDone * 13;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
                SoundEngine.PlaySound(SoundID.DeerclopsIceAttack with { Pitch = 1.2f, PitchVariance = 0.2f }, target.Center);
                for (int i = 0; i < 20; i++)
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


    }
}
