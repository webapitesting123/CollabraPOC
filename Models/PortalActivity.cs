using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FabrikamResidences_Activities.Models
{
    public class PortalActivity
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<Attendee> Attendees { get; set; } 

        [NotMapped]
        public string ModifyDate { get; set; }

    }
}
