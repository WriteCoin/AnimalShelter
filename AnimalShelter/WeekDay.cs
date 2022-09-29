using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class WeekDay
    {
        public WeekDay()
        {
            WorkHours = new HashSet<WorkHour>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<WorkHour> WorkHours { get; set; }
    }
}
