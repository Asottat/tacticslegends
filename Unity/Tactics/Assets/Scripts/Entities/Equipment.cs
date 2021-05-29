using Assets.Scripts.Entities.LocalData;

namespace Assets.Scripts.Entities
{
    public class Equipment
    {
        public long Id { get; set; }
        //public long? PlayerId { get; set; } //Apenas para o banco
        public string Name { get; set; }
        public int? BaseModelId { get; set; }
        public int? Skin { get; set; }
        public BaseEquipModel BaseModel { get; set; }
    }
}
