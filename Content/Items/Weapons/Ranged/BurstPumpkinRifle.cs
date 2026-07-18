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
    public class BurstPumpkinRifle : ModItem
    {
        private int burstShotsLeft = 0;
        private int burstTimer = 0;
        private EntitySource_ItemUse_WithAmmo currentShot = null;

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.rare = ItemRarityID.Yellow;
            Item.useAmmo = ModContent.ItemType<PumpkinAmmo>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.scale = 1.3f;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;

            Item.noMelee = true;
            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<PumpkinProjectile>();
            Item.shootSpeed = 19f;

            Item.ArmorPenetration = 15;
            Item.crit = 8;
            Item.DamageType = DamageClass.Ranged;
            

            Item.UseSound = SoundID.Item108 with { PitchVariance = 0.1f, Pitch = 0f };
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            burstShotsLeft = 4;   
            burstTimer = 4;
            currentShot = source;

            FireOneShot(player, position, velocity);

            return false;
        }

        public override void HoldItem(Player player)
        {
            if (burstShotsLeft > 0)
            {
                burstTimer--;

                if (burstTimer <= 0)
                {
                    burstTimer = 4;
                    burstShotsLeft--;

                    Vector2 position = player.Center;
                    Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed;

                    FireOneShot(player, position, velocity);
                }
            }
        }

        private void FireOneShot(Player player, Vector2 position, Vector2 velocity)
        {
            //get correct ammo projectile + modifiers
            player.PickAmmo(Item,
                out int ammoProj,
                out float speed,
                out int ammoDamage,
                out float ammoKnockback,
                out int usedAmmoItemId,
                true);

            int finalDamage =(int) (ammoDamage * 0.8f);
            float finalKnockback = ammoKnockback;
            Vector2 finalVelocity = velocity.SafeNormalize(Vector2.UnitX) * (Item.shootSpeed);

            int proj = Projectile.NewProjectile(currentShot, position, finalVelocity, ammoProj, finalDamage, finalKnockback, player.whoAmI);
            Main.projectile[proj].scale = 0.4f;

            SoundEngine.PlaySound(
                SoundID.Item108 with { PitchVariance = 0.2f, Pitch = 0.2f },
                position
            );
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 5).AddIngredient(ItemID.CrystalShard, 15).AddIngredient(ItemID.CobaltBar, 10).AddIngredient(ItemID.Wood, 15).AddIngredient(ItemID.IllegalGunParts).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 5).AddIngredient(ItemID.CrystalShard, 15).AddIngredient(ItemID.PalladiumBar, 10).AddIngredient(ItemID.Wood, 15).AddIngredient(ItemID.IllegalGunParts).AddTile(TileID.Anvils).Register();
        }
    }
}
