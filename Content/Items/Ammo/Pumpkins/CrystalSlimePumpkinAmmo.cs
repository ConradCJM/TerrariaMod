using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class CrystalSlimePumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 999;
            Item.scale = 0.8f;
            Item.rare = ItemRarityID.Pink;

            Item.value = Item.buyPrice(0);
            Item.damage = 20;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2f;
            Item.ammo = ModContent.ItemType<PumpkinAmmo>();
            Item.shoot = ModContent.ProjectileType<CrystalSlimePumpkinProjectile>();
            Item.consumable = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.GelBalloon, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 5);
            recipe.AddIngredient(ModContent.ItemType<PumpkinAmmo>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
