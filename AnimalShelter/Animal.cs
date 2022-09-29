using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Animal
    {
        public Animal()
        {
            ShelterAnimals = new HashSet<ShelterAnimal>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Age { get; set; }
        public bool Vaccinated { get; set; }

        public virtual ICollection<ShelterAnimal> ShelterAnimals { get; set; }
    }
}
