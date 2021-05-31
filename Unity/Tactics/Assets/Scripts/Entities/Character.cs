using System.Collections.Generic;

namespace Assets.Scripts.Entities
{
    public class Character
    {
        public long Id { get; set; }
        public long PlayerId { get; set; }
        public string Name { get; set; }
        public int Role { get; set; } //Class -> enum
        public int Gender { get; set; } //Enum: 
        public int Skin { get; set; } //Enum: 
        public int Head { get; set; }
        public int Eyebrows { get; set; }
        public int FacialHair { get; set; }
        public int Hair { get; set; }

        public int Level { get; set; }

        //Primary Stats
        public int BaseHealth { get; set; }
        public int BaseVitality { get; set; }
        public int BaseStrength { get; set; }
        public int BaseDextery { get; set; }
        public int BaseIntelligence { get; set; }
        public int BaseEvasion { get; set; }

        //Secondary Stats
        public int BaseMovement { get; set; }
        public int BaseJump { get; set; }
        public int BaseSpeed { get; set; }

        public List<Equipment> EquipSlots { get; set; }
        public Weapon MainHand { get; set; }
        public Weapon OffHand { get; set; }
    }
}
