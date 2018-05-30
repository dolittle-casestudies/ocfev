using System;
using Dolittle.Commands.Handling;
using Dolittle.Domain;
using Dolittle.Runtime.Events;

namespace Domain.Vessels
{
    public class ThrottleCommandHandlers : ICanHandleCommands
    {
        static EventSourceId vesselId = (EventSourceId)Guid.Parse("5477464f-d3af-4526-abe0-e6eccab97bcb");
        
        readonly IAggregateRootRepositoryFor<Throttling> _repository;

        public ThrottleCommandHandlers(IAggregateRootRepositoryFor<Throttling> repository)
        {
            _repository = repository;
        }

        public void Handle(ChangeThrottle command)
        {
            var throttling = _repository.Get(vesselId);
            throttling.ChangeTo(command.Engine, command.Target);
        }
    }
}