namespace Assets.Scripts.Entities.Utility
{
    public class TurnGauge
    {
        public long CharacterId { get; set; }
        public long PlayerId { get; set; }
        public float Speed { get; set; }
        public int Movement { get; set; }
        public float Gauge { get; set; }

        public TurnGauge Clone()
        {
            return new TurnGauge()
            {
                CharacterId = this.CharacterId,
                PlayerId = this.PlayerId,
                Speed = this.Speed,
                Movement = this.Movement,
                Gauge = this.Gauge
            };
        }
    }
}
