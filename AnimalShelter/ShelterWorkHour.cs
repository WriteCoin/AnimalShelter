using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class ShelterWorkHour
    {
        public int Id { get; set; }
        public int ShelterId { get; set; }
        public int WorkHoursId { get; set; }

        public virtual Shelter Shelter { get; set; } = null!;
        public virtual WorkHour WorkHours { get; set; } = null!;
    }
}
