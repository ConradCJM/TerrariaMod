using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class DirtPumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 999;
            Item.scale = 0.8f;

            Item.value = Item.buyPrice(0);
            Item.rare = ItemRarityID.White;
            Item.damage = 5;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2f;
            Item.ammo = ModContent.ItemType<PumpkinAmmo>();
            Item.shoot = ModContent.ProjectileType<DirtPumpkinProjectile>();
            Item.consumable = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(7);
            recipe.AddIngredient(ItemID.DirtBlock, 6);
            recipe.Register();
        }

        
    }
}
