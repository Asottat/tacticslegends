namespace Assets.Scripts.Entities
{
    public class CharacterGameplay
    {
        public CharacterGameplay(Character baseCharacter)
        {
            BaseInfo = baseCharacter;
            HealthMax = baseCharacter.BaseHealth;
            HealthCurrent = HealthMax;
            Vitality = baseCharacter.BaseVitality;
            Strength = baseCharacter.BaseStrength;
            Dextery = baseCharacter.BaseDextery;
            Intelligence = baseCharacter.BaseIntelligence;
            Evasion = baseCharacter.BaseEvasion;
            Movement = baseCharacter.BaseMovement;
            Jump = baseCharacter.BaseMovement;
            Speed = baseCharacter.BaseSpeed;

            //TODO: Implementar os modificadores de atributos com base nos itens
            //TODO: Utilizar o GameMath

            MainHandWeaponSkill = 1f; //TODO: recuperar da arma x dados do personagem (detalhes na planilha excel)

            Power = HealthMax + Movement + Jump + Speed; //TODO: Implementar lógica de cálculo de Power
            BaseAttackRange = 1; //TODO: Controlar pela arma
        }

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
