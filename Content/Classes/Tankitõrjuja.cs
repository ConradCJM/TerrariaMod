using Terraria;
using Terraria.ModLoader;

//custom class for julian

namespace SomethingCreative.Content.Classes
{
    public class TankitõrjujaDamage : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;

            return new StatInheritanceData(
                damageInheritance: 0.1f,
                critChanceInheritance: 0.1f,
                attackSpeedInheritance: 0.1f,
                armorPenInheritance: 0.1f,
                knockbackInheritance: 0.1f);
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }

        public override void SetDefaultStats(Player player)
        {
            player.GetArmorPenetration<TankitõrjujaDamage>() += 9999;
        }

        public override bool UseStandardCritCalcs => true;

        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return base.ShowStatTooltipLine(player, lineName);
        }
    }
}
