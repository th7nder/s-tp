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

    public void DisplayAllClients()
    {
      IEnumerable<Client> clients = _dataRepository.GetAllClients();
      Console.WriteLine($"- All Clients | Count: {clients.Count()}"); 
      foreach (Client client in clients)
      {
        Console.WriteLine($"\t{client}");
      }
    }

    public void DisplayAllInvoices()
    {
      IEnumerable<Invoice> invoices = _dataRepository.GetAllInvoices();
      Console.WriteLine($"- All Invoices | Count: {invoices.Count()}");
      foreach (Invoice invoice in invoices)
      {
        Console.WriteLine($"\t{invoice}");
      }
    }

    public void DisplayAllOffers()
    {
      IEnumerable<Offer> offers = _dataRepository.GetAllOffers();
      Console.WriteLine($"- All Offers | Count: {offers.Count()}");
      foreach (Offer offer in offers)
      {
        Console.WriteLine($"\t{offer}");
      }
    }

    public void DisplayAllProducts()
    {
      IEnumerable<Product> products = _dataRepository.GetAllProducts();
      Console.WriteLine($"- All Products | Count: {products.Count()}");
      foreach (Product product in products)
      {
        Console.WriteLine($"\t{product}");
      }
    }
  }
}
