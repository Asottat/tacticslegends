using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class GameMath
    {
        public static float GetBaseAttackAccuracy(CharacterGameplay attacker, CharacterGameplay opponent, bool isBackAttack = false, bool isMainHand = true)
        {
            //Adjust attacker dextery based on level difference
            var dexAtt = attacker.Dextery + (((opponent.BaseInfo.Level - attacker.BaseInfo.Level) / 50f) * attacker.Dextery);

            //Apply the accuracy formula
            var acc = dexAtt / (dexAtt + opponent.Evasion) * 190f;

            //Apply weapon skill multiplier
            //TODO: se for offhand, aplicar multiplicador de 0.8 do resultado final, a não ser que alguma skill especifique o contrário
            float weaponSkill = isMainHand ? attacker.MainHandWeaponSkill : attacker.OffHandWeaponSkill;
            acc = Mathf.Clamp(acc, 1f, 99f) * weaponSkill;

            //Bonus to attack from behind (2x)
            if (isBackAttack)
            {
                acc = Mathf.Clamp(acc, 1f, 99f) * 2f;
            }

            return (float)System.Math.Round(Mathf.Clamp(acc, 1f, 99f), 2);
        }
    }
}
