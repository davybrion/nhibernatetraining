using System.Collections.Generic;
using Northwind.Builders;
using Northwind.Dtos;
using Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Linq;

namespace NHibernateWorkshop.Querying
{
    [TestFixture]
    public class Exercises : AutoRollbackFixture
    {
        [Test]
        public void get_employee_with_highest_salary()
        {
            // the maximum salary to be generated for the testdata is set at 3500
            var topPaidEmployee = new EmployeeBuilder().WithSalary(3600).Build();
            Session.Save(topPaidEmployee);
            FlushAndClear();

            Employee retrievedEmployee = null;

            Assert.AreEqual(topPaidEmployee, retrievedEmployee);
        }

        [Test]
        public void get_overview_of_product_suppliers_ordered_by_product_name_and_purchase_cost()
        {
            Session.Query<Product>().ToList();
            Session.Query<Supplier>().ToList();
            var allSources = Session.Query<ProductSource>().ToList();

            // this is strictly in memory
            var productSources = allSources
                .Select(s => new ProductSourcesOverview(s.Product.Id, s.Product.Name, s.Supplier.Name, s.Cost))
                .OrderBy(o => o.ProductName)
                .ThenBy(o => o.Cost)
                .ToList();

            IList<ProductSourcesOverview> retrievedProductSources = null;

            Assert.AreEqual(productSources, retrievedProductSources);
        }

        [Test]
        public void get_cheapest_supplier_for_product()
        {
            var product = new ProductBuilder().Build();
            var supplier1 = new SupplierBuilder().Build();
            var supplier2 = new SupplierBuilder().Build();
            var supplier3 = new SupplierBuilder().Build();
            product.AddSource(supplier1, 50);
            product.AddSource(supplier2, 48);
            product.AddSource(supplier3, 52);
            Session.Save(product);
            Session.Flush();

            Supplier cheapestSupplier = null;

            Assert.AreEqual(supplier2, cheapestSupplier);
        }

        [Test]
        public void get_top_10_selling_products()
        {
            Session.QueryOver<Product>().List(); // makes sure that we already have the product references in the session cache
            var allOrderItems = Session.QueryOver<OrderItem>().List();

            // this is strictly in memory... it might contain hints for the real query, but then again it might not :P
            var topProducts = allOrderItems
                .GroupBy(o => o.Product)
                .Select(g => new { g.Key, Total = g.Sum(o => o.Quantity) })
                .OrderByDescending(g => g.Total)
                .Take(10)
                .Select(g => g.Key)
                .ToList();

            IList<Product> retrievedTopProducts = null;

            Assert.AreEqual(topProducts, retrievedTopProducts);
        }
    }
}