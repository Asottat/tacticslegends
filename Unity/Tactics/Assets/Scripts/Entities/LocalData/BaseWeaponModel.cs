namespace Assets.Scripts.Entities.LocalData
{
    public class BaseWeaponModel
    {
        public BaseWeaponModel() { }
        public BaseWeaponModel(int id, int weaponType)
        {
            Id = id;
            WeaponType = weaponType;
        }

        public int Id { get; set; }
        public int WeaponType { get; set; }
    }
}
