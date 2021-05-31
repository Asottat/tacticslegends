using Assets.Scripts.Entities.LocalData;

namespace Assets.Scripts.Entities
{
    public class Weapon
    {
        public long Id { get; set; }
        //public long? PlayerId { get; set; } //Apenas para o banco
        public string Name { get; set; }
        public int? BaseModelId { get; set; }
        public int? Skin { get; set; }
        public BaseWeaponModel BaseModel { get; set; }
    }
}
