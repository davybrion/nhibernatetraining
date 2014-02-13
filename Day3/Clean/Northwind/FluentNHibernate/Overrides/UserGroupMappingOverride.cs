using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Northwind.Entities;

namespace Northwind.FluentNHibernate.Overrides
{
    public class UserGroupMappingOverride : IAutoMappingOverride<UserGroup>
    {
        public void Override(AutoMapping<UserGroup> mapping)
        {
            mapping.Cache.ReadWrite();

            mapping.Id(o => o.Id).GeneratedBy.HiLo("100");

            mapping.Map(u => u.Name)
                .Not.Nullable()
                .Length(20);
        }
    }
}