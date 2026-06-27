using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class EnchantedBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 10;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 2;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.scale = 3f;
            Item.crit = -10;
            Item.rare = ItemRarityID.Green;
            Item.value = 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ModContent.ItemType<DirtBrick>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

        }





        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {

                int extraDamage = damageDone * 7;
                target.SimpleStrikeNPC(extraDamage, 0, false, 0, DamageClass.Melee, false, 0, false);
            }
        }


    }
}
