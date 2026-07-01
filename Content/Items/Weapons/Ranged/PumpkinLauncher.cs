using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SomethingCreative.Content.Items.Ammo.Pumpkins;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Ranged
{
    public class PumpkinLauncher : ModItem
    {

        public override void SetDefaults()
        { 
            Item.reuseDelay = 5;
            Item.damage = 15;
            Item.rare = ItemRarityID.Green;
            Item.useAmmo = ModContent.ItemType<PumpkinAmmo>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ItemID.IronBar, 10);
            r.AddIngredient(ItemID.Wood, 5);
            r.AddTile(TileID.Anvils);
            r.Register();

            CreateRecipe().AddIngredient(ItemID.LeadBar, 10).AddIngredient(ItemID.Wood, 5).AddTile(TileID.Anvils).Register();

        }
    }
}
