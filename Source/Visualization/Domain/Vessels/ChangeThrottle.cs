using Dolittle.Commands;

namespace Domain.Vessels
{
    public class ChangeThrottle : ICommand
    {
        public int Engine {  get; set; }
        public float Target {  get; set; }

    }
}