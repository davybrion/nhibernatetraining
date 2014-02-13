using System;

namespace Northwind.Dtos
{
    public class ProductSourcesOverview : IEquatable<ProductSourcesOverview>
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string SupplierName { get; private set; }
        public double Cost { get; private set; }

        protected ProductSourcesOverview() {}

        public ProductSourcesOverview(int productId, string productName, string supplierName, double cost)
        {
            ProductId = productId;
            ProductName = productName;
            SupplierName = supplierName;
            Cost = cost;
        }

        public bool Equals(ProductSourcesOverview other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ProductId == ProductId && Equals(other.ProductName, ProductName) && Equals(other.SupplierName, SupplierName) && other.Cost.Equals(Cost);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ProductSourcesOverview)) return false;
            return Equals((ProductSourcesOverview)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = ProductId;
                result = (result * 397) ^ (ProductName != null ? ProductName.GetHashCode() : 0);
                result = (result * 397) ^ (SupplierName != null ? SupplierName.GetHashCode() : 0);
                result = (result * 397) ^ Cost.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(ProductSourcesOverview left, ProductSourcesOverview right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductSourcesOverview left, ProductSourcesOverview right)
        {
            return !Equals(left, right);
        }
    }
}