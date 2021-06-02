using Assets.Scripts.Entities.LocalData;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Services.LocalData
{
    public class BaseWeaponService
    {
        private List<BaseWeaponModel> _allWep;

        public BaseWeaponService()
        {
            _allWep = new List<BaseWeaponModel>();

            #region Dagger (1)
            _allWep.Add(new BaseWeaponModel(22,1));
            _allWep.Add(new BaseWeaponModel(23,1));
            _allWep.Add(new BaseWeaponModel(76,1));
            _allWep.Add(new BaseWeaponModel(70,1));
            _allWep.Add(new BaseWeaponModel(71,1));
            #endregion

            #region Sword (2)
            _allWep.Add(new BaseWeaponModel(5,2));
            _allWep.Add(new BaseWeaponModel(10,2));
            _allWep.Add(new BaseWeaponModel(11,2));
            _allWep.Add(new BaseWeaponModel(20,2));
            _allWep.Add(new BaseWeaponModel(21,2));
            _allWep.Add(new BaseWeaponModel(50,2));
            _allWep.Add(new BaseWeaponModel(51,2));
            _allWep.Add(new BaseWeaponModel(67,2));
            _allWep.Add(new BaseWeaponModel(74,2));
            _allWep.Add(new BaseWeaponModel(75,2));
            _allWep.Add(new BaseWeaponModel(79,2));
            #endregion

            #region Axe (3)
            _allWep.Add(new BaseWeaponModel(1,3));
            _allWep.Add(new BaseWeaponModel(3,3));
            _allWep.Add(new BaseWeaponModel(12,3));
            _allWep.Add(new BaseWeaponModel(14,3));
            _allWep.Add(new BaseWeaponModel(30,3));
            _allWep.Add(new BaseWeaponModel(40,3));
            _allWep.Add(new BaseWeaponModel(44,3));
            _allWep.Add(new BaseWeaponModel(45,3));
            _allWep.Add(new BaseWeaponModel(46,3));
            #endregion

            #region Mace (4)
            _allWep.Add(new BaseWeaponModel(4,4));
            _allWep.Add(new BaseWeaponModel(16,4));
            _allWep.Add(new BaseWeaponModel(19,4));
            _allWep.Add(new BaseWeaponModel(35,4));
            _allWep.Add(new BaseWeaponModel(36,4));
            _allWep.Add(new BaseWeaponModel(37,4));
            _allWep.Add(new BaseWeaponModel(41,4));
            _allWep.Add(new BaseWeaponModel(42,4));
            _allWep.Add(new BaseWeaponModel(48,4));
            _allWep.Add(new BaseWeaponModel(80,4));
            #endregion

            #region Hammer (5)
            _allWep.Add(new BaseWeaponModel(17,5));
            _allWep.Add(new BaseWeaponModel(38,5));
            _allWep.Add(new BaseWeaponModel(39,5));
            #endregion

            #region TwoHandSword (6)
            _allWep.Add(new BaseWeaponModel(9,6));
            _allWep.Add(new BaseWeaponModel(26,6));
            _allWep.Add(new BaseWeaponModel(27,6));
            _allWep.Add(new BaseWeaponModel(28,6));
            _allWep.Add(new BaseWeaponModel(29,6));
            #endregion

            //TODO: Editar os modelos dos Machados de Duas Mãos pra pegada ficar um pouco mais acima
            #region TwoHandAxe (7)
            _allWep.Add(new BaseWeaponModel(2,7));
            _allWep.Add(new BaseWeaponModel(7,7));
            _allWep.Add(new BaseWeaponModel(13,7));
            _allWep.Add(new BaseWeaponModel(15,7));
            #endregion

            #region TwoHandMace (8)
            _allWep.Add(new BaseWeaponModel(43,8));
            #endregion

            #region TwoHandHammer (9)
            _allWep.Add(new BaseWeaponModel(31,9));
            _allWep.Add(new BaseWeaponModel(32,9));
            _allWep.Add(new BaseWeaponModel(33,9));
            _allWep.Add(new BaseWeaponModel(34,9));
            #endregion

            #region Spear (10)
            _allWep.Add(new BaseWeaponModel(24,10));
            _allWep.Add(new BaseWeaponModel(47,10));
            _allWep.Add(new BaseWeaponModel(63,10));
            _allWep.Add(new BaseWeaponModel(64,10));
            _allWep.Add(new BaseWeaponModel(69,10));
            #endregion

            #region Staff (11)
            _allWep.Add(new BaseWeaponModel(25,11));
            _allWep.Add(new BaseWeaponModel(65,11));
            _allWep.Add(new BaseWeaponModel(66,11));
            _allWep.Add(new BaseWeaponModel(68,11));
            #endregion

            #region Polearm (12)
            _allWep.Add(new BaseWeaponModel(6,12));
            _allWep.Add(new BaseWeaponModel(8,12));
            _allWep.Add(new BaseWeaponModel(18,12));
            _allWep.Add(new BaseWeaponModel(49,12));
            _allWep.Add(new BaseWeaponModel(72,12));
            _allWep.Add(new BaseWeaponModel(73,12));
            #endregion

            #region ShortBow (13)
            #endregion

            #region LongBow (14)
            _allWep.Add(new BaseWeaponModel(81, 14));
            _allWep.Add(new BaseWeaponModel(82, 14));
            #endregion

            #region Crossbow (15)
            #endregion

            #region Wand (16)
            #endregion

            #region Shield (17)
            _allWep.Add(new BaseWeaponModel(52,17));
            _allWep.Add(new BaseWeaponModel(53,17));
            _allWep.Add(new BaseWeaponModel(54,17));
            _allWep.Add(new BaseWeaponModel(55,17));
            _allWep.Add(new BaseWeaponModel(56,17));
            _allWep.Add(new BaseWeaponModel(57,17));
            _allWep.Add(new BaseWeaponModel(58,17));
            _allWep.Add(new BaseWeaponModel(59,17));
            _allWep.Add(new BaseWeaponModel(60,17));
            _allWep.Add(new BaseWeaponModel(61,17));
            _allWep.Add(new BaseWeaponModel(62,17));
            #endregion

            #region Throw (18)
            _allWep.Add(new BaseWeaponModel(77,18));
            _allWep.Add(new BaseWeaponModel(78,18));
            #endregion

            #region Pistol (19)
            #endregion

            #region Rifle (20)
            #endregion

            #region Relic (21)
            #endregion
        }

        public BaseWeaponModel GetById(int id)
        {
            return _allWep.FirstOrDefault(f => f.Id == id);
        }

        //public List<BaseEquipModel> GetBaseEquipBySlot(ItemSlotType slotType)
        //{
        //    return _allEquip.Where(w => w.Slot == (int)slotType).ToList();
        //}

        //public List<BaseEquipModel> GetBaseEquipBySlotClass(ItemSlotType slotType, ItemClassType classType)
        //{
        //    int st = (int)slotType;
        //    int ct = (int)classType;
        //    return _allEquip.Where(w => w.Slot == st && w.ClassType.HasValue && w.ClassType == ct).ToList();
        //}
    }
}
