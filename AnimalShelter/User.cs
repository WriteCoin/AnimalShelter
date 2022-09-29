using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
