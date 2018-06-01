using System;

namespace MQTT
{
    public class Telemetry
    {
        public string type => "telemetry";
        public float angle_wind_relative { get; set; } // degrees
        public float relative_windspeed  { get; set; } // meters/second
        public float depth               { get; set; } // metres

        public float power               { get; set; } // Megawatt
        public float sog                 { get; set; } // Speed over ground, Knots
        public float stw                 { get; set; } // Speed through water, Knots

        public float list                { get; set; } // degrees
        public float trim                { get; set; } // metres

        public Guid vessel_id            { get; set; }
        public long time_stamp           { get; set; } // Unix Time in Millisecond


    }
}
