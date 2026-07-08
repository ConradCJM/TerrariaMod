using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class FlamingPumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 999;
            Item.scale = 0.8f;

            Item.value = Item.buyPrice(0);
            Item.rare = ItemRarityID.LightRed;
            Item.damage = 12;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 10f;
            Item.ammo = ModContent.ItemType<PumpkinAmmo>();
            Item.shoot = ModContent.ProjectileType<FlamingPumpkinProjectile>();

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(ModContent.ItemType<PumpkinAmmo>(), 10);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }


    }
    
}
