using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Ammo.Pumpkins
{
    public class VoidPumpkinAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 54;
            Item.maxStack = 9999;
            Item.scale = 0.8f;

            Item.value = Item.buyPrice(0);
            Item.rare = ItemRarityID.Red;
            Item.damage = 5;

            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0f;
            Item.ammo = ModContent.ItemType<PumpkinAmmo>();
            Item.shoot = ModContent.ProjectileType<VoidPumpkinProjectile>();
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(40);
            recipe.AddIngredient(ItemID.LunarOre, 10);
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);

            //calamity mod recipe
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && calamity.TryFind<ModItem>("MeldBlob", out ModItem meldblob))
            {
                recipe.AddIngredient(meldblob.Type, 10);
            }

            recipe.AddIngredient(ModContent.ItemType<PumpkinAmmo>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
