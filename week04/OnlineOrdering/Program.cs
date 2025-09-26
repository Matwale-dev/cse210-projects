using System;
using System.Collections.Generic;

// ==================== Product Class ====================
public class Product
{
    private string _name;
    private string _productId;
    private double _price;
    private int _quantity;

    public Product(string name, string productId, double price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetProductId()
    {
        return _productId;
    }

    public double GetTotalCost()
    {
        return _price * _quantity;
    }
}

// ==================== Address Class ====================
public class Address
{
    private string _street;
    private string _city;
    private string _county;
    private string _country;

    public Address(string street, string city, string county, string country)
    {
        _street = street;
        _city = city;
        _county = county;
        _country = country;
    }

    public bool IsInUSA()
    {
        return _country.Trim().ToUpper() == "USA";
    }

    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_county}\n{_country}";
    }
}

// ==================== Customer Class ====================
public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string GetName()
    {
        return _name;
    }

    public Address GetAddress()
    {
        return _address;
    }

    public bool LivesInUSA()
    {
        return _address.IsInUSA();
    }
}

// ==================== Order Class ====================
public class Order
{
    private List<Product> _products = new List<Product>();
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public double GetTotalCost()
    {
        double total = 0;
        foreach (Product product in _products)
        {
            total += product.GetTotalCost();
        }

        // Add shipping cost
        total += _customer.LivesInUSA() ? 5 : 35;
        return total;
    }

    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (Product product in _products)
        {
            label += $"- {product.GetName()} (ID: {product.GetProductId()})\n";
        }
        return label;
    }

    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{_customer.GetName()}\n{_customer.GetAddress().GetFullAddress()}";
    }
}

// ==================== Program Class ====================
public class Program
{
    public static void Main()
    {
        // --------- Order 1 (Customer in Nairobi) ----------
        Address addr1 = new Address("00100 Kenyatta Ave", "Nairobi", "Nairobi County", "Kenya");
        Customer cust1 = new Customer("Peter Mwangi", addr1);
        Order order1 = new Order(cust1);
        order1.AddProduct(new Product("Maize Flour", "M123", 120.0, 5));
        order1.AddProduct(new Product("Cooking Oil", "C456", 350.0, 2));

        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: KES {order1.GetTotalCost():0.00}\n");

        // --------- Order 2 (Customer in Mombasa) ----------
        Address addr2 = new Address("Ocean Road 102", "Mombasa", "Mombasa County", "Kenya");
        Customer cust2 = new Customer("Aisha Hassan", addr2);
        Order order2 = new Order(cust2);
        order2.AddProduct(new Product("Laptop", "L001", 75000.0, 1));
        order2.AddProduct(new Product("USB Flash Drive", "U002", 1500.0, 3));

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: KES {order2.GetTotalCost():0.00}");
    }
}
