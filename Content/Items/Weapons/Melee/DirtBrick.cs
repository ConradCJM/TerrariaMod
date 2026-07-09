using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.crit = -10;
            Item.rare = ItemRarityID.Green;
            Item.value = 0;
            Item.useTurn = true;

            Item.ArmorPenetration = 1;

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
                SoundEngine.PlaySound(SoundID.Dig, target.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust d2 = Dust.NewDustPerfect(
                        target.Center,
                        DustID.Dirt,
                        Main.rand.NextVector2Circular(10f, 10f),
                        150,
                        default,
                        1.4f
                    );
                    d2.noGravity = true;
                }
            }
        }


    }
}
