namespace Ingestion
{
    public class NavigationTelemetry
    {
        public float Trim { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public float Yaw { get; set; }
        public float Surge { get; set; }
        public float Sway { get; set; } // Artist formerly known as...
        public float Heave { get; set; }
        public float Depth { get; set; }
        public float STW { get; set; } // Speed Through Water
        public float SOG { get; set; } // Speed Over Ground
    }
}
