using SomethingCreative.Content.NPCS.Hostile;
using SomethingCreative.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SomethingCreative.Systems
{
    public class AFKBossSpawner : ModSystem
    {
        public override void PostUpdatePlayers()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];

                if (!p.active || p.dead)
                    continue;

                var afk = p.GetModPlayer<AFKTracker>();

                if (afk.afkTimer >= 60 * 60 * 10) // 10 minutes
                {
                    // Prevent multiple spawns
                    afk.afkTimer = 0;

                    // Spawn the boss near the player
                    NPC.NewNPC(
                        Entity.GetSource_NaturalSpawn(),
                        (int)p.Center.X,
                        (int)p.Center.Y - 300,
                        ModContent.NPCType<AntiLegoAFKBoss>()
                    );
                }
            }
        }
    }

}
