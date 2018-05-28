using System;
using Dolittle.Events.Processing;
using ExternalEvents;

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
        public void Process(GravityChanged @event)
        {
            Console.WriteLine($"Gravity : {@event.X}, {@event.Y}, {@event.Z}");
            var hub = _hubs.Get<VesselOrientationHub>();
            hub.GravityChanged(@event.X, @event.Y, @event.Z);
        }
    }
}