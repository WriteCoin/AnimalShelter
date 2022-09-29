using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class ShelterAnimal
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public int ShelterId { get; set; }
        public DateOnly AdoptionDate { get; set; }
        public int RequestId { get; set; }
        public DateOnly TermShelter { get; set; }

        public virtual Animal Animal { get; set; } = null!;
        public virtual Request Request { get; set; } = null!;
        public virtual Shelter Shelter { get; set; } = null!;
    }
}
