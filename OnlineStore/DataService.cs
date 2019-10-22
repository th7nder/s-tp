using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore
{
  class DataService
  {
    private readonly IDataRepository _dataRepository;

    public DataService(IDataRepository dataRepository)
    {
      _dataRepository = dataRepository;
    }

    public IEnumerable<Client> GetClients() => _dataRepository.GetAllClients();
    public IEnumerable<Invoice> GetInvoices() => _dataRepository.GetAllInvoices();
    public IEnumerable<Offer> GetOffers() => _dataRepository.GetAllOffers();
    public IEnumerable<Product> GetProducts() => _dataRepository.GetAllProducts();

    public IEnumerable<Invoice> GetInvoicesForClient(Client client)
    {
      IEnumerable<Invoice> invoices = GetInvoices();

      return invoices.Where(invoice => invoice.Client.Email == client.Email);
    }

    public IEnumerable<Product> GetBoughtProducts(Client client)
    {
      IEnumerable<Invoice> invoices = GetInvoicesForClient(client);
      IEnumerable<Product> products = invoices.Select(invoice => invoice.Offer.Product);

      return products;
    }

    public IEnumerable<Invoice> GetInvoicesInPeriod(DateTimeOffset startTime, DateTimeOffset stopTime)
    {
      return GetInvoices().Where(invoice => invoice.PurchaseTime >= startTime && invoice.PurchaseTime <= stopTime);
    }

    public IEnumerable<Client> GetClientsFromCity(string city)
    {
      return GetClients().Where(client => client.Address.City.Equals(city));
    }

    public int GetProductSales(Product product)
    {
      return GetInvoices().Count(invoice => invoice.Offer.Product.Id.Equals(product.Id));
    }

    public Invoice BuyProduct(Client client, Offer offer)
    {
      Invoice invoice = new Invoice
      {
        Id = Guid.NewGuid(),
        Client = client,
        Offer = offer,
        PurchaseTime = DateTimeOffset.Now
      };

      _dataRepository.AddInvoice(invoice);

      return invoice;
    }
  }
}
