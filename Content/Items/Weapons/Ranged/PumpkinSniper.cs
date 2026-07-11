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
    public class PumpkinSniper : ModItem
    {
        public override void SetDefaults()
        {
            Item.reuseDelay = 55;
            Item.damage = 167;
            Item.rare = ItemRarityID.Yellow;
            Item.useAmmo = ModContent.ItemType<PumpkinAmmo>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 7f;

            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.shootSpeed = 35f;

            Item.ArmorPenetration = 167;

            Item.crit = 31;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item122 with { PitchVariance = 0.1f, Pitch = 0.3f };

        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        { 
            if (player.altFunctionUse == 2)
            { 
                return false;
            }
            return base.CanUseItem(player);
        }

        public override void HoldItem(Player player)
        {
            // If right-click is NOT held, disable zoom
            if (Main.mouseRight)
                player.scope = true;
            else
                player.scope = false;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[proj].scale = 0.4f;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SoulofFright, 15).AddIngredient(ItemID.SoulofMight,15).AddIngredient(ItemID.SoulofSight,15).AddIngredient(ItemID.HallowedBar, 15).AddIngredient(ItemID.AdamantiteBar, 10).AddIngredient(ItemID.Wood, 10).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe().AddIngredient(ItemID.SoulofFright, 15).AddIngredient(ItemID.SoulofMight, 15).AddIngredient(ItemID.SoulofSight, 15).AddIngredient(ItemID.HallowedBar, 15).AddIngredient(ItemID.AdamantiteBar, 10).AddIngredient(ItemID.Wood, 10).AddTile(TileID.MythrilAnvil).Register();
        }

    }
}
