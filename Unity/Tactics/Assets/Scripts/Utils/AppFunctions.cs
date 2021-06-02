using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class AppFunctions
    {
        public static int GetDirectionNum(int dirX, int dirY)
        {
            if (dirX == -1)
            {
                if (dirY == 0)
                    return 4;
                if (dirY == -1)
                    return 5;
            }
            else if (dirX == 0)
            {
                if (dirY == -1)
                    return 0;
                if (dirY == 1)
                    return 3;
            }
            else if (dirX == 1)
            {
                if (dirY == 0)
                    return 1;
                if (dirY == 1)
                    return 2;
            }
            return 0;
        }

        public static Quaternion GetRotation(int num)
        {
            switch (num)
            {
                case 0: return new Quaternion(0, 1, 0, 0);
                case 1: return new Quaternion(0, 0.9f, 0, -0.5f);
                case 2: return new Quaternion(0, 0.5f, 0, -0.9f);
                case 3: return new Quaternion(0, 0, 0, 1);
                case 4: return new Quaternion(0, 0.5f, 0, 0.9f);
                case 5: return new Quaternion(0, 0.9f, 0, 0.5f);
            }
            return new Quaternion(0, 1, 0, 0);
        }

        public static Quaternion GetRotation(int dirX, int dirY)
        {
            return GetRotation(GetDirectionNum(dirX, dirY));
        }

        public static int GetWeaponRangeByType(int? weaponType)
        {
            if (!weaponType.HasValue)
                return 1;

            switch (weaponType.Value)
            {
                case (int)WeaponType.ShortBow:
                case (int)WeaponType.LongBow:
                case (int)WeaponType.Pistol:
                case (int)WeaponType.Rifle:
                    return 3;
                case (int)WeaponType.Spear:
                    return 2;
                default:
                    return 1;
            }
        }

        public static int GetWeaponRangeByType(int? mainWeapon, int? offWeapon)
        {
            var main = GetWeaponRangeByType(mainWeapon);
            var off = GetWeaponRangeByType(offWeapon);

            return main >= off ? main : off;
        }

        //public static Vector3 GetHexPosition(float coordX, float coordY, float? height = null)
        //{
        //    var newCoordX = coordX - AppConts.HexMapConfig.MAX_MAP_MATRIX_X;
        //    var newCoordY = coordY - AppConts.HexMapConfig.MAX_MAP_MATRIX_Y;
        //    var posX = AppConts.HexMapConfig.STEP_HEX_POS_X * newCoordX * -1f;
        //    var posY = height.HasValue ? AppConts.HexMapConfig.ELEVATION_MDF * height.Value : 0f;
        //    var posZ = (AppConts.HexMapConfig.STEP_HEX_POS_Y * newCoordY) - (newCoordX * AppConts.HexMapConfig.STEP_HEX_POS_Y / 2f);

        //    return new Vector3(posX, posY, posZ);
        //}
    }
}
