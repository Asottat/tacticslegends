namespace Assets.Scripts.Entities
{
    public class CharacterGameplay
    {
        public Character BaseInfo;
        public int HealthMax;
        public int HealthCurrent;

        public int Vitality { get; set; }
        public int Strength { get; set; }
        public int Dextery { get; set; }
        public int Intelligence { get; set; }
        public int Evasion { get; set; }

        public int Movement;
        public int Jump;
        public int Speed;

        public int BaseAttackRange;
        public float MainHandWeaponSkill;
        public float OffHandWeaponSkill;

        public int Power;

        #region Gameplay

        public int PosX;
        public int PosY;
        public int FaceDirection;

        #endregion
    }
}
