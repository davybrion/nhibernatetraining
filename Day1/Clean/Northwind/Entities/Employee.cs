using System;
using Northwind.Components;

namespace Northwind.Entities
{
    public class Employee : Entity<int>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual DateTime HireDate { get; set; }
        public virtual Address Address { get; set; }
        public virtual string Phone { get; set; }

        protected Employee() {}

        public Employee(string firstName, string lastName, Address address, DateTime birthDate, DateTime hireDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            BirthDate = birthDate;
            HireDate = hireDate;
        }
    }
}