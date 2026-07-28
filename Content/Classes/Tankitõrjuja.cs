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
                damageInheritance: 0.2f,
                critChanceInheritance: 0.2f,
                attackSpeedInheritance: 0.2f,
                armorPenInheritance: 0.2f,
                knockbackInheritance: 0.2f);
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
