using System.ComponentModel.DataAnnotations.Schema;

namespace FabrikamResidences_Activities.Models
{
    public class Attendee
    {
        public int Id { get; set; }

        public int PortalActivityId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}