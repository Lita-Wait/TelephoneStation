
namespace TelephoneStation.Domain.Model
{
    public class Agent
    {
        public required string Name { get; set; }

        public bool isBusy { get; set; } = false;

        public override string ToString() => Name;

    }
}
