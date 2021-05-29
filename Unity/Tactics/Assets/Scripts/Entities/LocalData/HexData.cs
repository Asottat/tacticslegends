namespace Assets.Scripts.Entities.LocalData
{
    public class HexData
    {
        public HexData(int coordX, int coordY, int height, int hexType, int? startTeam = null, int? blockLevel = null, int? prop = null, int? propRotation = null)
        {
            CoordX = coordX;
            CoordY = coordY;
            Height = height;
            HexType = hexType;
            StartTeam = startTeam;
            BlockLevel = blockLevel;
            Prop = prop;
            PropRotation = propRotation;
        }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int Height { get; set; }
        public int HexType { get; set; } //Enum: HexType
        public int? StartTeam { get; set; }
        public int? BlockLevel { get; set; } //Heigth above hex
        //public int? BlockPercent { get; set; } //Width block
        public int? Prop { get; set; }
        public int? PropRotation { get; set; }
        public CharacterGameplay Character { get; set; }
    }
}
