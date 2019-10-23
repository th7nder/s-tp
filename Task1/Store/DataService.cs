using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
  class DataService
  {
    private readonly IDataRepository _dataRepository;

    public DataService(IDataRepository dataRepository)
    {
      _dataRepository = dataRepository;
    }

    public event EventHandler InvoiceAdded
    {
      add => _dataRepository.InvoiceAdded += value;
      remove => _dataRepository.InvoiceAdded -= value;
    }
    public event EventHandler InvoiceRemoved
    {
      add => _dataRepository.InvoiceDeleted += value;
      remove => _dataRepository.InvoiceDeleted -= value;
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

    public IEnumerable<Invoice> GetInvoicesForProduct(Product product)
    {
      return GetInvoices().Where(invoice => invoice.Offer.Product.Id.Equals(product.Id));
    }

    public IEnumerable<Client> GetClientsForProduct(Product product)
    {
      IEnumerable<Invoice> invoices = GetInvoicesForProduct(product);
      Dictionary<string, Client> clients = new Dictionary<string, Client>();
      foreach (Invoice invoice in invoices)
      {
        clients[invoice.Client.Email] = invoice.Client;
      }

      return clients.Values;
    }

    public IEnumerable<Invoice> GetInvoicesInPeriod(DateTimeOffset startTime, DateTimeOffset stopTime)
    {
      return GetInvoices().Where(invoice => invoice.PurchaseTime >= startTime && invoice.PurchaseTime <= stopTime);
    }

    public IEnumerable<Client> GetClientsFromCity(string city)
    {
      return GetClients().Where(client => client.Address.City.Equals(city));
    }


    public ValueTuple<int, decimal> GetProductSales(Product product)
    {
      var productInvoices = GetInvoices()
        .Where(invoice => invoice.Offer.Product.Id.Equals(product.Id));

      int count = productInvoices.Count();
      decimal value = productInvoices.Sum(invoice => invoice.Offer.Price);
      return (count, value);
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
      if (offer.Count <= 1)
      {
        throw new InvalidOperationException("There are no products in stock");
      }

      offer.Count -= 1;
      _dataRepository.UpdateOffer(offer.Id, offer);
      _dataRepository.AddInvoice(invoice);

      return invoice;
    }

    public void UpdateProductStock(Product product, int count)
    {
      Offer offer = GetOffers().FirstOrDefault(o => o.Product.Id.Equals(product.Id));
      offer.Count = count;

      _dataRepository.UpdateOffer(offer.Id, offer);
    }

    public void AddClient(Client client) => _dataRepository.AddClient(client);
    public void AddOffer(Offer offer) => _dataRepository.AddOffer(offer);
    public void AddProduct(Product product) => _dataRepository.AddProduct(product);

    public void DeleteInvoice(Invoice invoice) => _dataRepository.DeleteInvoice(invoice);
    public void DeleteClient(Client client) => _dataRepository.DeleteClient(client);
    public void DeleteOffer(Offer offer) => _dataRepository.DeleteOffer(offer);
    public void DeleteProduct(Product product) => _dataRepository.DeleteProduct(product);

    public void UpdateInvoice(Guid invoiceId, Invoice invoice) => _dataRepository.UpdateInvoice(invoiceId, invoice);
    public void UpdateClient(string email, Client client) => _dataRepository.UpdateClient(email, client);
    public void UpdateOffer(Guid offerId, Offer offer) => _dataRepository.UpdateOffer(offerId, offer);
    public void UpdateProduct(Guid productId, Product product) => _dataRepository.UpdateProduct(productId, product);
  }
}
