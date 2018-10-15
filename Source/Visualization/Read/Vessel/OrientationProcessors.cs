using System;
using Dolittle.Events.Processing;
using Events.Devices.Vessels;

namespace Read.Vessel
{
    public class OrientationProcessors : ICanProcessEvents
    {

        [EventProcessor("7230393e-1ba5-4fb8-b337-d85ddb3158f1")]
        public void Process(GravityChanged @event)
        {
            
        }
    }
}