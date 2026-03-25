using System;
using System.Collections.Generic;

namespace OnlineOrdering
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store orders
            List<Order> orders = new List<Order>();

            // Create Addresses
            Address address1 = new Address("123 Main Street", "New York", "NY", "USA");
            Address address2 = new Address("456 Queen Street", "Toronto", "ON", "Canada");
            Address address3 = new Address("789 Oxford Street", "London", "England", "UK");

            // Create Customers
            Customer customer1 = new Customer("John Smith", address1);
            Customer customer2 = new Customer("Sarah Johnson", address2);
            Customer customer3 = new Customer("Michael Brown", address3);

            // Create Order 1 (USA Customer)
            Order order1 = new Order(customer1);
            order1.AddProduct(new Product("Laptop Computer", "LAP-001", 999.99m, 1));
            order1.AddProduct(new Product("Wireless Mouse", "MOU-002", 29.99m, 2));
            order1.AddProduct(new Product("Laptop Bag", "BAG-003", 49.99m, 1));
            orders.Add(order1);

            // Create Order 2 (Canadian Customer)
            Order order2 = new Order(customer2);
            order2.AddProduct(new Product("Smartphone", "PHN-001", 699.99m, 1));
            order2.AddProduct(new Product("Phone Case", "CAS-002", 19.99m, 3));
            order2.AddProduct(new Product("Screen Protector", "SCR-003", 12.99m, 2));
            orders.Add(order2);

            // Create Order 3 (UK Customer)
            Order order3 = new Order(customer3);
            order3.AddProduct(new Product("Headphones", "HDP-001", 199.99m, 1));
            order3.AddProduct(new Product("USB-C Cable", "USB-002", 14.99m, 4));
            orders.Add(order3);

            // Display all orders
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine($"ORDER {i + 1}");
                Console.WriteLine("=".PadRight(50, '='));
                Console.WriteLine();

                // Display packing label
                Console.WriteLine(orders[i].GetPackingLabelWithQuantity());
                Console.WriteLine();

                // Display shipping label
                Console.WriteLine(orders[i].GetShippingLabel());
                Console.WriteLine();

                // Display total price
                Console.WriteLine($"TOTAL PRICE: ${orders[i].GetTotalCost():F2}");
                Console.WriteLine();
                Console.WriteLine("-".PadRight(50, '-'));
                Console.WriteLine();
            }
        }
    }
}