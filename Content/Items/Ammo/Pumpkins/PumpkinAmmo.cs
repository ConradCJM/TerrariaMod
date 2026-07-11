using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class PumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 9999;
            Item.scale = 0.8f;

            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.White;
            Item.damage = 7;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2f;
            Item.ammo = Item.type;
            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.consumable = true;

        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe(10);
            r.AddIngredient(ItemID.Pumpkin, 5);
            r.AddTile(TileID.WorkBenches);
            r.Register();
        }
    }

        
}
