using Microsoft.Xna.Framework;
using SomethingCreative.Content.Projectiles.XTimesN;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Magic
{
    public class XTimesN : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shootSpeed = 20f;
            Item.damage = 200;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.scale = 3f;
            Item.mana = 20;
            Item.alpha = 100;
            Item.crit = 16;
            Item.UseSound = SoundID.DD2_DarkMageHealImpact with { PitchVariance = 0.2f, Pitch = 0.3f };
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind<ModRarity>("CalamityRed", out ModRarity CalamityRed))
                {
                    Item.rare = CalamityRed.Type;

                }
                else
                {
                    Item.rare = ItemRarityID.Master;
                }
            }
            else
            {
                Item.rare = ItemRarityID.Master;
            }
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
                    ModContent.ProjectileType<RecursiveXTimesNBolt>(),
                    damage,
                    0f,
                    player.whoAmI
                );

            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe().AddIngredient(ModContent.ItemType<X2>()).AddIngredient(ItemID.LastPrism).AddIngredient(ItemID.LunarOre,67);

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && calamity.TryFind<ModTile>("CosmicAnvil", out ModTile cosmicAnvil))
            {
                if (calamity.TryFind<ModItem>("CosmiliteBar", out ModItem cosmiliteBar))
                {
                    r.AddIngredient(cosmiliteBar.Type, 25).AddTile(cosmicAnvil.Type);

                }
            }
            else
            {
                r.AddTile(TileID.LunarCraftingStation);
            }
            r.Register();
        }


    }
}
