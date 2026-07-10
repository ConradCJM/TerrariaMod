using Microsoft.Xna.Framework;
using SomethingCreative.Content.Items.Ammo.Pumpkins;
using SomethingCreative.Content.Projectiles.Pumpkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Ranged
{
    public class PumpkinShotgun : ModItem
    {
        public override void SetDefaults()
        {
            Item.reuseDelay = 15;
            Item.damage = 3;
            Item.rare = ItemRarityID.Green;
            Item.useAmmo = ModContent.ItemType<PumpkinAmmo>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.shootSpeed = 20f;

            Item.ArmorPenetration = 10;

            Item.crit = 7;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item108 with { PitchVariance = 0.1f, Pitch = -0.8f };

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int pellets = Main.rand.Next(6,7);

            for (int i = 0; i < pellets; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(16));
                float speedMod = 1f - (Main.rand.NextFloat() * 0.3f);
                int proj = Projectile.NewProjectile(source, position, newVelocity * speedMod, type, damage, knockback, player.whoAmI);

                Main.projectile[proj].scale = 0.4f;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Boomstick).AddIngredient(ItemID.DemoniteBar, 10).AddIngredient(ItemID.ShadowScale, 10).AddIngredient(ItemID.Wood, 10).AddTile(TileID.DemonAltar).Register();
            CreateRecipe().AddIngredient(ItemID.Boomstick).AddIngredient(ItemID.CrimtaneBar, 10).AddIngredient(ItemID.TissueSample, 10).AddIngredient(ItemID.Wood, 10).AddTile(TileID.DemonAltar).Register();


        }

    }
}
