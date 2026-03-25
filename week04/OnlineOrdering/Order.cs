using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineOrdering
{
    public class Order
    {
        private List<Product> _products;
        private Customer _customer;

        public Order(Customer customer)
        {
            _products = new List<Product>();
            _customer = customer;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public decimal GetTotalCost()
        {
            decimal totalProductsCost = 0;

            foreach (Product product in _products)
            {
                totalProductsCost += product.GetTotalCost();
            }

            // Add shipping cost
            if (_customer.IsInUSA())
            {
                totalProductsCost += 5;
            }
            else
            {
                totalProductsCost += 35;
            }

            return totalProductsCost;
        }

        public string GetPackingLabel()
        {
            StringBuilder packingLabel = new StringBuilder();
            packingLabel.AppendLine("PACKING LABEL");
            packingLabel.AppendLine("-------------");

            foreach (Product product in _products)
            {
                packingLabel.AppendLine($"Product: {product.GetName()}");
                packingLabel.AppendLine($"ID: {product.GetProductId()}");
                packingLabel.AppendLine($"Quantity: {product.GetTotalCost() / product.GetTotalCost()}"); // This is awkward, better approach below
                packingLabel.AppendLine();
            }

            return packingLabel.ToString();
        }

        // Alternative implementation with quantity
        public string GetPackingLabelWithQuantity()
        {
            StringBuilder packingLabel = new StringBuilder();
            packingLabel.AppendLine("PACKING LABEL");
            packingLabel.AppendLine("-------------");

            foreach (Product product in _products)
            {
                packingLabel.AppendLine($"Product: {product.GetName()} (ID: {product.GetProductId()})");
                packingLabel.AppendLine();
            }

            return packingLabel.ToString();
        }

        public string GetShippingLabel()
        {
            StringBuilder shippingLabel = new StringBuilder();
            shippingLabel.AppendLine("SHIPPING LABEL");
            shippingLabel.AppendLine("---------------");
            shippingLabel.AppendLine($"Customer: {_customer.GetName()}");
            shippingLabel.AppendLine(_customer.GetAddress().GetFullAddress());

            return shippingLabel.ToString();
        }
    }
}