using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class ChlorophytePumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 999;
            Item.scale = 0.8f;

            Item.value = Item.buyPrice(0);
            Item.rare = ItemRarityID.Green;
            Item.damage = 20;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2f;
            Item.ammo = ModContent.ItemType<PumpkinAmmo>();
            Item.shoot = ModContent.ProjectileType<ChlorophytePumpkinProjectile>();

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.ChlorophyteOre, 10);
            recipe.AddIngredient(ModContent.ItemType<DirtPumpkinAmmo>(), 10);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
