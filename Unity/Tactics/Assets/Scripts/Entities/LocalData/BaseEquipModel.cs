using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.LocalData
{
    public class BaseEquipModel
    {
        public BaseEquipModel() { }
        public BaseEquipModel(int id, int slot, params string[] compositions)
        {
            Id = id;
            Slot = slot;
            Composition = new List<int[]>();
            foreach (var compItem in compositions)
                Composition.Add(compItem.Split(':').Select(s => int.Parse(s)).ToArray());
        }
        public BaseEquipModel(int id, int slot, int classType, params string[] compositions)
        {
            Id = id;
            Slot = slot;
            ClassType = classType;
            Composition = new List<int[]>();
            foreach (var compItem in compositions)
                Composition.Add(compItem.Split(':').Select(s => int.Parse(s)).ToArray());
        }

        /// <summary>
        /// Unique identifier of the model
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Enum: ItemSlotType
        /// </summary>
        public int Slot { get; set; }
        /// <summary>
        /// Enum: ItemClassType
        /// </summary>
        public int? ClassType { get; set; }
        /// <summary>
        /// Map for ModularCharacter prefab
        /// </summary>
        public List<int[]> Composition { get; set; }
    }
}
