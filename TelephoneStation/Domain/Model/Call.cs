
using System.Xml.Linq;

namespace TelephoneStation.Domain.Model
{
    public class Call
    {
        public Guid Id { get; set; }
        public int DurationInSec { get; set; }

        public override string ToString() => Id.ToString();
    }
}
