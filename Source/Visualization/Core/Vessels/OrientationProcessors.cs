using System;
using Dolittle.Events.Processing;
using Events.Devices.Vessels;

namespace Web.Vessel
{
    /// <summary>
    /// 
    /// </summary>
    public class OrientationProcessors : ICanProcessEvents
    {
        readonly IHubs _hubs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubs"></param>
        public OrientationProcessors(IHubs hubs)
        {
            _hubs = hubs;
        }

        /// <summary>
        /// 
        /// </summary>
        [EventProcessor("c13f3e8d-865c-4f36-b9b9-63ac33d9a033")]
        public void Process(GravityChanged @event)
        {
            Console.WriteLine($"Gravity : {@event.X}, {@event.Y}, {@event.Z}");
            var hub = _hubs.Get<VesselOrientationHub>();
            hub.GravityChanged(@event.X, @event.Y, @event.Z);
        }
    }
}
