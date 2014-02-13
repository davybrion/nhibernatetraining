using System.Collections.Generic;
using System.Linq;
using Northwind.Enums;

namespace Northwind.Entities
{
    public class Product : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual ProductCategory Category { get; set; }
        public virtual double? UnitPrice { get; set; }
        public virtual int? UnitsInStock { get; set; }
        public virtual int? UnitsOnOrder { get; set; }
        public virtual int? ReorderLevel { get; set; }
        public virtual bool Discontinued { get; set; }

        protected Product() {}

        public Product(string name, ProductCategory category)
        {
            Name = name;
            Category = category;
        }
    }
}