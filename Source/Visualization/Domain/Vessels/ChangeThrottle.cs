using Dolittle.Commands;

namespace Domain.Vessels
{
    public class ChangeThrottle : ICommand
    {
        public int[] Engines {  get; set; }
        public double Target {  get; set; }
    }
}