using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Events;

namespace Domain
{
    public class Vessel : AggregateRoot
    {
        static float _previousPower;

        public Vessel(EventSourceId eventSourceId) : base(eventSourceId) {}

        public void ChangePower(float newValue)
        {
            if( newValue > _previousPower ) Apply(new PowerIncreased { From = _previousPower, To = newValue });
            else Apply(new PowerIncreased { From = _previousPower, To = newValue });
        }
    }
}
