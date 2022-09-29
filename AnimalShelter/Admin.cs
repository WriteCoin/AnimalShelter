using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Admin
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Password { get; set; } = null!;

        public virtual Person Person { get; set; } = null!;
    }
}
