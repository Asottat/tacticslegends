namespace Assets.Scripts.Utils
{
    //TODO: RENOMEAR ESSA PORRA PRA AppConsts
    public class AppConts
    {
        public class PrefabPath
        {
            public const string MODULAR_HERO = "ModularCharacter";
            public const string WEAPONS = "Weapons/Wep";
        }

        public class AnimatorControllerPath
        {
            public const string UNARMED = "Animators/CharUnarmed";
            public const string TWO_HANDED_SWORD = "Animators/CharTwoHandedSword";
            public const string TWO_HANDED_HAMMER = "Animators/CharTwoHandedHammer";
            public const string TWO_HANDED_SPEAR = "Animators/CharTwoHandedSpear";
            public const string POLEARM = "Animators/CharPolearm";
            public const string ONE_HAND = "Animators/CharOneHand";
            public const string DUAL_WIELD = "Animators/CharDualWield";
            public const string BOW = "Animators/CharBow";
        }

        public class PopupPath
        {
            public const string CHAR_SELECTION = "UI/Popups/PopupCharSelection";
        }

        public class UIComponentsPath
        {
            public const string CHARACTER_SELECTION = "UI/UICharacterSelection";
            public const string CONFIRM = "UI/UIConfirm";
            public const string TOAST = "UI/UIToast";
        }

        public class HexMarkPath
        {
            public const string SET_POSITION = "Marks/HexSetPosition";
            public const string SET_DIRECTION = "Marks/HexDirection";
            public const string MOVE_OPTION = "Marks/HexMoveOption";
            public const string PLAYER_TURN = "Marks/HexPlayerTurn";
            public const string ALLY = "Marks/HexAlly";
            public const string ENEMY = "Marks/HexEnemy";
            public const string ATTACK_OPTION = "Marks/HexAttackOption";
        }

        public class MaterialPath
        {
            public const string WEAPON = "Materials/BaseWeapon";
        }

        //public class HexMapConfig
        //{
        //    public const int MAX_MAP_MATRIX_X = 10;
        //    public const int MAX_MAP_MATRIX_Y = 10;
        //    public const float STEP_HEX_POS_X = 4.33f;
        //    public const float STEP_HEX_POS_Y = 5f;
        //    public const float ELEVATION_MDF = 1f;
        //    public int MAP_MATRIX_HALF_X = 5;
        //    public int MAP_MATRIX_HALF_Y = 5;
        //}
    }
}
