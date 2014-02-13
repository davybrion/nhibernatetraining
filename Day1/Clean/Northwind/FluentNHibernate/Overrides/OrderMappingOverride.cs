using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using Northwind.Entities;

namespace Northwind.FluentNHibernate.Overrides
{
    public class OrderMappingOverride : IAutoMappingOverride<Order>
    {
        public void Override(AutoMapping<Order> mapping)
        {
            mapping.References(o => o.Customer)
                .Not.Nullable();

            mapping.References(o => o.Employee)
                .Not.Nullable();

            mapping.Map(o => o.OrderedOn)
                .Not.Nullable();
        }
    }
}