using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SomethingCreative.Content.Items.Weapons.Tankitõrjuja
{
    public class Igla : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<Classes.TankitõrjujaDamage>();
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<IglaHeldProj>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;

            Item.shootSpeed = 0f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var p = player.GetModPlayer<IglaPlayer>();
            return !p.isReloading;
        }
    }

    public class IglaHeldProj : ModProjectile
    {
        int chargeTimer = 0;
        bool readyToFire = false;
        public override void SetDefaults()
        {
            Projectile.width = 302;
            Projectile.height = 128;
            Projectile.scale = 0.30f;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;


            Projectile.timeLeft = 2;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles,
    List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
    List<int> overWiresUI)
        {
            overPlayers.Add(index); // draw on top of player
        }




        public override void AI()
        {
            //more comments than usual since its my first time doing something like this
            //Main.NewText("Current Charge: " + chargeTimer);

            Player player = Main.player[Projectile.owner];
            var p = player.GetModPlayer<IglaPlayer>();

            //keep projectile alive
            Projectile.timeLeft = 2;

            Vector2 aim = player.DirectionTo(Main.MouseWorld);
            Projectile.rotation = aim.ToRotation();


            //draw projectile
            Vector2 offset = new(-100, -10);

            Projectile.Center = player.MountedCenter + offset;

            //flip sprite when aiming left
            Projectile.spriteDirection = aim.X < 0 ? -1 : 1;

            //counter‑rotate the sprite when flipped so it doesn't appear upside‑down
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            //make player face the aim direction
            player.direction = aim.X >= 0 ? 1 : -1;

            //make the player's arm rotate toward the mouse
            player.itemRotation = aim.ToRotation();

            bool holdingM1 = player.channel;

            //start charging
            if (holdingM1)
            {

                chargeTimer++;

                if (chargeTimer >= 60)
                    readyToFire = true;
            }
            else
            {
                //release
                if (readyToFire)
                {
                    TryFire(player, p);
                }

                //reset
                chargeTimer = 0;
                readyToFire = false;

                Projectile.Kill(); // stop holding projectile
            }
        }

        void TryFire(Player player, IglaPlayer p)
        {
            if (p.isReloading)
                return;

            //consume ammo
            p.ammoCount--;
            if (p.ammoCount <= 0)
            {
                p.isReloading = true;
                p.reloadTimer = 900;
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Reloading! Ammo: {p.ammoCount}");
            }
            else
            {
                CombatText.NewText(player.Hitbox, Color.LightYellow, $"Ammo: {p.ammoCount}");
            }

            //fire projectile
            Vector2 velocity = player.DirectionTo(Main.MouseWorld) * 10f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                velocity,
                ProjectileID.Bullet, // replace with custom ammo
                player.GetWeaponDamage(player.HeldItem),
                player.GetWeaponKnockback(player.HeldItem),
                player.whoAmI
            );
        }
    }



    public class IglaPlayer : ModPlayer
    {
        public int ammoCount = 10;
        public int reloadTimer = 0;
        public bool isReloading = false;

        public override void PostUpdate()
        {
            if (isReloading)
            {
                reloadTimer--;
                if (reloadTimer <= 0)
                {
                    isReloading = false;
                    ammoCount = 4;

                    Player player = Player;
                    CombatText.NewText(player.Hitbox, Color.LightYellow, $"Reloaded! Ammo: {ammoCount}");
                }
            }
        }
    }


}
