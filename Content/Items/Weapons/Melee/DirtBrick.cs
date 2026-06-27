using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;

namespace SomethingCreative.Content.Items.Weapons.Melee
{
    public class DirtBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.DamageType = DamageClass.Melee;
            Item.damage = 5;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 7;

            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.scale = 1.25f;
            Item.crit = -5;
            Item.rare = ItemRarityID.Green;
            Item.value = 0;
            Item.useTurn = true;

        }

        public override void AddRecipes() { 
            CreateRecipe().AddIngredient(ItemID.DirtBlock, 67).AddTile(TileID.WorkBenches).Register();
        }



        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                
                int extraDamage = damageDone * 10;
                target.SimpleStrikeNPC(extraDamage,0,false,0,DamageClass.Melee,false,0,false);
            }
        }


    }
}
