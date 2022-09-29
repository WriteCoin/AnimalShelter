using System;
using System.Collections.Generic;

namespace AnimalShelter
{
    public partial class Person
    {
        public Person()
        {
            Admins = new HashSet<Admin>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Fio { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string Telephone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly RegDate { get; set; }

        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
