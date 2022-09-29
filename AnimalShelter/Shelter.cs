using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Shelter
    {
        public Shelter()
        {
            ShelterAnimals = new HashSet<ShelterAnimal>();
            ShelterWorkHours = new HashSet<ShelterWorkHour>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public int AddressId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual ICollection<ShelterAnimal> ShelterAnimals { get; set; }
        public virtual ICollection<ShelterWorkHour> ShelterWorkHours { get; set; }
    }
}
