using Microsoft.Xna.Framework;
using SomethingCreative.Content.Items.Ammo.Pumpkins;
using SomethingCreative.Content.Projectiles.Pumpkins;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Ranged
{
    public class PumpkinAssaultRifle : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.rare = ItemRarityID.Purple;
            Item.useAmmo = ModContent.ItemType<PumpkinAmmo>();
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.reuseDelay = 0;

            Item.noMelee = true;
            Item.knockBack = 5f;

            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.shootSpeed = 21f;

            Item.ArmorPenetration = 25;
            Item.crit = 11;
            Item.DamageType = DamageClass.Ranged;


            Item.UseSound = SoundID.Item108 with { PitchVariance = 0.2f, Pitch = 0.2f };
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 15).AddIngredient(ItemID.ShroomiteBar, 15).AddIngredient(ItemID.FragmentVortex, 15).AddIngredient(ItemID.IllegalGunParts).AddTile(TileID.LunarCraftingStation).Register();
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 15).AddIngredient(ItemID.ShroomiteBar, 15).AddIngredient(ItemID.FragmentVortex, 15).AddIngredient(ItemID.IllegalGunParts).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}
