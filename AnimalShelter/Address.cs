using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Address
    {
        public Address()
        {
            Requests = new HashSet<Request>();
            Shelters = new HashSet<Shelter>();
        }

        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int House { get; set; }
        public int? UserId { get; set; }
        public string FullAddr { get; set; } = null!;

        public virtual User? User { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
    }
}
