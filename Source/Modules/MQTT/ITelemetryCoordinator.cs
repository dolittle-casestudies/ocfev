namespace MQTT
{
    public interface ITelemetryCoordinator
    {
        void OrientationChanged(VesselOrientation orientation);
    }
}