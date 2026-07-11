using SomethingCreative.Content.Items.Ammo.Pumpkins;
using SomethingCreative.Content.Items.Weapons.Melee;
using SomethingCreative.Content.Projectiles.Pumpkins;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Ranged
{
    public class PumpkinFlareGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.reuseDelay = 55;
            Item.damage = 1;

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind<ModRarity>("ExoticRainbow", out ModRarity exoticRainbow))
                {
                    Item.rare = exoticRainbow.Type;

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

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 0f;

            Item.shoot = ProjectileID.RainbowFlare;//make pumpkin flare projectile and put it here
            Item.shootSpeed = 18f;

            Item.ArmorPenetration = 300;

            Item.crit = 6;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item122 with { PitchVariance = 0.1f, Pitch = 0.3f };
        }

        public override bool? UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<SkibidiPlayer>();
            modPlayer.flareCoolDown = 60 * 30;//(60 ticks/1 second) * 30 seconds
            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<SkibidiPlayer>();
            if (modPlayer.flareCoolDown > 0)
            {
                return false;
            }
            return base.CanUseItem(player);
        }

    }
    public class SkibidiPlayer : ModPlayer
    {
        public int flareCoolDown = 0;
        

        public override void ResetEffects()
        {
            if (flareCoolDown >= 0)
            {
                flareCoolDown--;

            }
            if (flareCoolDown == 0)
            {
                SoundEngine.PlaySound(SoundID.Dolphin with { Volume = 5f }, Player.Center);
            }
        }

    }
}
