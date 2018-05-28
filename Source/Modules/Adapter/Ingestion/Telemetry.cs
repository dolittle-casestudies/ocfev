namespace Ingestion
{
    public class Telemetry
    {
        public float angle_wind_relative { get; set; } // degrees
        public float depth               { get; set; } // metres
        public float list                { get; set; } // degrees
        public float power               { get; set; } // Megawatt
        public float sog                 { get; set; } // Knots
        public float relative_windspeed  { get; set; } // meters/second
        public float stw                 { get; set; } // Knots
        public float trim                { get; set; } // metres
    }
}
