using Microsoft.Xna.Framework;
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
    public class XPlusN : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shootSpeed = 20f;
            Item.damage = 167;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 60;
            Item.useAnimation = 60;

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
                    ModContent.ProjectileType<RecursiveAddBolt>(),
                    damage,
                    0f,
                    player.whoAmI
                );

            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.IronBar, 15).AddIngredient(ItemID.ManaCrystal, 3).AddTile(TileID.Anvils);
            CreateRecipe().AddIngredient(ItemID.LeadBar, 15).AddIngredient(ItemID.ManaCrystal, 3).AddTile(TileID.Anvils);
        }

        
    }
}
