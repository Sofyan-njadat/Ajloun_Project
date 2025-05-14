using System.Collections.Generic;

namespace Ajloun_Project.Models
{
    public class EventsViewModel
    {
        public List<CulturalEvent> CulturalEvents { get; set; }
        public List<AssociationEvent> AssociationEvents { get; set; }
        public List<dynamic> Associations { get; set; } 
        public List<string> EventTypes { get; set; }
    }
} 