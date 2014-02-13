using System;
using Northwind.Components;

namespace Northwind.Entities
{
    public class Order : Entity<int>
    {
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DateTime OrderedOn { get; set; }
        public virtual DateTime? ShippedOn { get; set; }
        public virtual Address DeliveryAddress { get; set; }

        protected Order() {}

        public Order(Customer customer, Employee employee) : this(customer, employee, DateTime.Now) {}

        public Order(Customer customer, Employee employee, DateTime orderedOn)
        {
            Customer = customer;
            Employee = employee;
            OrderedOn = orderedOn;
        }
    }
}