using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Request
    {
        public Request()
        {
            ShelterAnimals = new HashSet<ShelterAnimal>();
        }

        public int Id { get; set; }
        public int RequestTypeId { get; set; }
        public DateOnly RegDate { get; set; }
        public int UserId { get; set; }
        public string ContactTelephone { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public int AddressId { get; set; }
        public int RequestStatusId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual RequestStatus RequestStatus { get; set; } = null!;
        public virtual RequestType RequestType { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ShelterAnimal> ShelterAnimals { get; set; }
    }
}
