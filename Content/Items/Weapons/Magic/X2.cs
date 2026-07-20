using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.X2;
using SomethingCreative.Content.Projectiles.XDiv2;
using SomethingCreative.Content.Projectiles.XMinN;
using SomethingCreative.Content.Projectiles.XPlusN;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Magic
{
    public class X2 : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shootSpeed = 20f;
            Item.damage = 169;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.scale = 0.35f;
            Item.mana = 20;
            Item.alpha = 100;
            Item.crit = 12;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.DD2_DarkMageHealImpact with { PitchVariance = 0.2f, Pitch = 0.3f };
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 direction = Main.MouseWorld - player.Center;
            //player.itemRotation = direction.ToRotation();

            //flip player based on mouse direction
            player.direction = direction.X >= 0 ? 1 : -1;
        }

        public override bool? UseItem(Player player)
        {

            int manaUsed = player.statMana;
            if (manaUsed >= 0)
            {
                player.statMana = -10;

                int damage = manaUsed + Item.damage;
                damage = damage / 4;

                Vector2 direction = Main.MouseWorld - player.Center;
                direction.Normalize();

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    direction * Item.shootSpeed,
                    ModContent.ProjectileType<RecursiveDoubleBolt>(),
                    damage,
                    0f,
                    player.whoAmI
                );

            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<XPlusN>()).AddIngredient(ItemID.AvengerEmblem).AddIngredient(ItemID.FragmentNebula, 15).AddIngredient(ItemID.SpectreBar, 30).AddTile(TileID.LunarCraftingStation).Register();
        }

        
    }
}
