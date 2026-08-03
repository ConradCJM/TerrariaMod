using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace SomethingCreative.Content.Players
{
    public class AFKTracker : ModPlayer
    {
        public int afkTimer = 0;
        private Vector2 lastPosition;

        public override void Initialize()
        {
            lastPosition = Player.position;
        }

        public override void PostUpdate()
        {
            bool isAFK =
                Player.position == lastPosition &&   // not moving
                Player.velocity == Vector2.Zero &&   // not drifting
                !Player.controlLeft &&
                !Player.controlRight &&
                !Player.controlUp &&
                !Player.controlDown &&
                !Player.controlJump &&
                !Player.controlUseItem &&
                !Player.controlHook &&
                !Player.controlMount;

            if (isAFK)
            {
                afkTimer++;
            }
            else
            {
                afkTimer = 0;
            }

            lastPosition = Player.position;
        }
    }

}
