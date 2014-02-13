using NHibernate.Criterion;
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

            var retrievedEmployee = Session.Query<Employee>()
                .OrderByDescending(e => e.Salary)
                .First();

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

//            var retrievedProductSources =
//                Session.CreateQuery(
//                    @"select new ProductSourcesOverview(pr.Id, pr.Name, s.Name, ps.Cost)
//                      from ProductSource ps join ps.Product pr join ps.Supplier s order by pr.Name, ps.Cost")
//                    .List<ProductSourcesOverview>();

            var retrievedProductSources = Session.Query<ProductSource>()
                .OrderBy(ps => ps.Product.Name)
                .ThenBy(ps => ps.Cost)
                .Select(ps => new ProductSourcesOverview(ps.Product.Id, ps.Product.Name, ps.Supplier.Name, ps.Cost))
                .ToList();

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

            var cheapestSupplier =
                Session.CreateQuery(
                    "select ps.Supplier from ProductSource ps where ps.Product.Id = :productId order by ps.Cost asc")
                    .SetParameter("productId", product.Id)
                    .SetMaxResults(1)
                    .UniqueResult<Supplier>();

            // the following LINQ approach also works, but uses an uneccessary join on Supplier in the subselect!
            //var cheapestSupplierId = Session.Query<ProductSource>()
            //    .Where(s => s.Product.Id == product.Id)
            //    .OrderBy(s => s.Cost)
            //    .Select(s => s.Supplier.Id)
            //    .Take(1);

            //var cheapestSupplier = Session.Query<Supplier>()
            //    .Where(s => s.Id == cheapestSupplierId.Single())
            //    .Single();

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
                .ThenBy(g => g.Key.Name)
                .Take(10)
                .Select(g => g.Key)
                .ToList();

            // this one only works on SQLite
            //var retrievedTopProducts =
            //    Session.CreateQuery(
            //        "select i.Product from OrderItem i group by i.Product order by sum(i.Quantity) desc, i.Product.Name asc")
            //        .SetMaxResults(10)
            //        .List<Product>();

            // this one only works on SQL Server... question: should it even work?!
            var retrievedTopProducts = Session.QueryOver<Product>()
                .WithSubquery.WhereProperty(p => p.Id).In(NHibernate.Criterion.QueryOver.Of<OrderItem>()
                    .Select(Projections.Group<OrderItem>(o => o.Product.Id))
                    .OrderBy(Projections.Sum<OrderItem>(o => o.Quantity)).Desc
                    .Take(10))
                .List();

            // straight SQL (works in both)
//            var retrievedTopProducts = Session.CreateSQLQuery(
//                @"select
//                    p.Id as 'Id',
//                    p.Version as 'Version',
//                    p.Name as 'Name',
//                    p.Category as 'Category',
//                    p.UnitPrice as 'UnitPrice',
//                    p.UnitsInStock as 'UnitsInStock',
//                    p.UnitsOnOrder as 'UnitsOnOrder',
//                    p.ReorderLevel as 'ReorderLevel',
//                    p.Discontinued as 'Discontinued' 
//                from
//                    OrderItem o 
//                inner join
//                    Product p 
//                        on o.ProductId=p.Id 
//                group by
//                    p.Id,
//                    p.Version,
//                    p.Name,
//                    p.Category,
//                    p.UnitPrice,
//                    p.UnitsInStock,
//                    p.UnitsOnOrder,
//                    p.ReorderLevel,
//                    p.Discontinued
//                order by
//                    sum(o.Quantity) desc,
//                    p.Name asc")
//                .AddEntity(typeof(Product))
//                .SetMaxResults(10)
//                .List<Product>();

            Assert.AreEqual(topProducts, retrievedTopProducts);
        }
    }
}