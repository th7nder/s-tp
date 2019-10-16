using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.ConsoleInterface
{
  class DataService
  {
    private DataRepository _dataRepository;
    public DataService(DataRepository dataRepository)
    {
      _dataRepository = dataRepository;
    }

    public IEnumerable<Client> GetAllClients() => _dataRepository.GetAllClients();
    public IEnumerable<Product> GetAllProducts() => _dataRepository.GetAllProducts();
    public IEnumerable<Offer> GetAllOffers() => _dataRepository.GetAllOffers();
    public IEnumerable<Invoice> GetAllInvoices() => _dataRepository.GetAllInvoices();

    public void DisplayBoughtProductsForClient(Client client)
    {
      Console.WriteLine($"## Display products for client: {client}");
      IEnumerable<Invoice> invoices = GetInvoicesForClient(client);
      foreach (Invoice invoice in invoices)
      {
        Console.WriteLine($"\t Invoice: {invoice}");
        foreach (Invoice.Item item in invoice.Items)
        {
          Console.WriteLine($"\t\t\t Product: {item.Offer.Product}");
        }
      }
    }

    public IEnumerable<Invoice> GetInvoicesForClient(Client client)
    {
      return _dataRepository.GetAllInvoices().Where(invoice => invoice.Client.Email == client.Email);
    }

    public IEnumerable<Offer> GetOffersFromInvoices(IEnumerable<Invoice> invoices)
    {
      Dictionary<Guid, Offer> offers = new Dictionary<Guid, Offer>();
      foreach (Invoice invoice in invoices)
      {
        foreach (Invoice.Item item in invoice.Items)
        {
          offers[item.Offer.Id] = item.Offer;
        }
      }

      return offers.Values;
    }

    public IEnumerable<Product> GetProductsFromOffers(IEnumerable<Offer> offers)
    {
      Dictionary<Guid, Product> products = new Dictionary<Guid, Product>();
      foreach (Offer offer in offers)
      {
        products[offer.Product.Id] = offer.Product;
      }

      return products.Values;
    }

    public IEnumerable<Product> GetBoughtProductsForClient(Client client)
    {
      IEnumerable<Invoice> invoices = GetInvoicesForClient(client);
      IEnumerable<Offer> offers = GetOffersFromInvoices(invoices);
      IEnumerable<Product> products = GetProductsFromOffers(offers);

      return products;
    }

    public void DisplayClients(IEnumerable<Client> clients)
    {
      Console.WriteLine($"-- Clients | Count: {clients.Count()}");
      foreach (Client client in clients)
      {
        Console.WriteLine($"\t{client}");
      }
    }

    public void DisplayInvoices(IEnumerable<Invoice> invoices)
    {
      Console.WriteLine($"-- Invoices | Count: {invoices.Count()}");
      foreach (Invoice invoice in invoices)
      {
        Console.WriteLine($"\t{invoice}");
      }
    }

    public void DisplayOffers(IEnumerable<Offer> offers)
    {
      Console.WriteLine($"-- Offers | Count: {offers.Count()}");
      foreach (Offer offer in offers)
      {
        Console.WriteLine($"\t{offer}");
      }
    }

    public void DisplayProducts(IEnumerable<Product> products)
    {
      Console.WriteLine($"-- Products | Count: {products.Count()}");
      foreach (Product product in products)
      {
        Console.WriteLine($"\t{product}");
      }
    }

    public Invoice CreateInvoice(Client client, Offer offer)
    {
      Invoice invoice = new Invoice
      {
        Client = client,
        PurchaseTime = DateTime.Now,
        Items =
        {
          new Invoice.Item
          {
            Count = 1, 
            Offer = offer
          }
        }
      };

      _dataRepository.AddInvoice(invoice);

      return invoice;
    }

    public Offer CreateOffer(Product product, decimal price, int count, decimal tax = 0.23M)
    {
      Offer offer = new Offer
      {
        Product = product,
        Count = count,
        Price = price,
        Tax = tax
      };

      _dataRepository.AddOffer(offer);
      return offer;
    }

    public Product CreateProduct(string name, string description, string category)
    {
      Product product = new Product
      {
        Category = category,
        Description = description,
        Name = name
      };

      _dataRepository.AddProduct(product.Id.ToString(), product);
      return product;
    }

    public Client CreateClient(string email, string name, string surname, Address address)
    {
      Client client = new Client
      {
        Email = email,
        Name = name,
        Surname = surname,
        Address = address
      };

      _dataRepository.AddClient(client);

      return client;
    }

    public Client GetClientByEmail(string email)
    {
      return _dataRepository.GetClient(email);
    }

    public void DeleteClient(Client client)
    {
      _dataRepository.DeleteClient(client);
    }
  }
}
