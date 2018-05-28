using System.Numerics;
using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Events.Vessels;

namespace Domain.Vessels
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

        public void ChangeGravity(Vector3 vector)
        {
            Apply(new GravityChanged { X = vector.X, Y = vector.Y, Z = vector.Z });
        }
    }
}
