using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class WorkHour
    {
        public WorkHour()
        {
            ShelterWorkHours = new HashSet<ShelterWorkHour>();
        }

        public int Id { get; set; }
        public int WeekDayId { get; set; }
        public TimeOnly HourMin { get; set; }
        public TimeOnly MinuteMin { get; set; }
        public TimeOnly HourMax { get; set; }
        public TimeOnly MinuteMax { get; set; }

        public virtual WeekDay WeekDay { get; set; } = null!;
        public virtual ICollection<ShelterWorkHour> ShelterWorkHours { get; set; }
    }
}
