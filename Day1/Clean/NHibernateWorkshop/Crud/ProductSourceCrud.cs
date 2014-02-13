using System;
using Northwind.Builders;
using Northwind.Entities;
using NUnit.Framework;

namespace NHibernateWorkshop.Crud
{
    [TestFixture]
    public class ProductSourceCrud : CrudFixture<ProductSource, int> 
    {
        protected override ProductSource BuildEntity()
        {
            var productSource = new ProductSourceBuilder().Build();
            // TODO: remove the following line (saving productSource.Supplier) and make sure the tests still work
            Session.Save(productSource.Supplier);
            Session.Save(productSource.Product);
            return productSource;
        }

        protected override void ModifyEntity(ProductSource entity)
        {
            entity.Cost = entity.Cost * 2;
        }

        protected override void AssertAreEqual(ProductSource expectedEntity, ProductSource actualEntity)
        {
            Assert.AreEqual(expectedEntity.Cost, actualEntity.Cost);
            Assert.AreEqual(expectedEntity.Product, actualEntity.Product);
            Assert.AreEqual(expectedEntity.Supplier, actualEntity.Supplier);
        }

        protected override void AssertValidId(ProductSource entity)
        {
            Assert.Greater(entity.Id, 0);
        }
    }
}