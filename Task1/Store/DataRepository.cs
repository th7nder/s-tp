using Store.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Store
{
  public class DataRepository : IDataRepository
  {
    private readonly DataContext _dataContext = new DataContext();
    private readonly IDataFiller _dataFiller;

    public event EventHandler InvoiceAdded;
    public event EventHandler InvoiceDeleted;

    public DataRepository(IDataFiller dataFiller)
    {
      _dataFiller = dataFiller;
      _dataFiller.Fill(_dataContext);

      _dataContext.Invoices.CollectionChanged += InvoicesCollectionChanged;
    }

    private void InvoicesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        InvoiceAdded?.Invoke(this, new EventArgs());
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        InvoiceDeleted?.Invoke(this, new EventArgs());
      }
    }

    public void AddClient(Client client)
    {
      if (_dataContext.Clients.Any(c => c.Email == client.Email))
      {
        throw new ArgumentException($"Client with email {client.Email} already exists");
      }

      _dataContext.Clients.Add(client);
    }

    public Client GetClient(string email)
    {
      return _dataContext.Clients.FirstOrDefault(client => client.Email == email);
    }

    public IEnumerable<Client> GetAllClients()
    {
      return _dataContext.Clients;
    }

    public void UpdateClient(string email, Client client)
    {
      if (email != client.Email)
      {
        throw new ArgumentException("Can't change client's email.");
      }

      int id = _dataContext.Clients.FindIndex(c => c.Email == email);
      if (id == -1)
      {
        throw new ArgumentException($"Client with email {email} does not exist");
      }

      _dataContext.Clients[id] = client;
    }

    public void DeleteClient(Client client)
    {
      if (!_dataContext.Clients.Remove(client))
      {
        throw new ArgumentException("Client does not exist");
      }
    }

    public IEnumerable<Product> GetAllProducts()
    {
      return _dataContext.Products.Values;
    }

    public Product GetProduct(Guid productId)
    {
      try
      {
        return _dataContext.Products[productId];
      }
      catch
      {
        return null;
      }
    }

    public void AddProduct(Product product)
    {
      if (_dataContext.Products.ContainsKey(product.Id))
      {
        throw new ArgumentException($"Product with key '${product.Id}' already exists");
      }

      _dataContext.Products.Add(product.Id, product);
    }

    public void UpdateProduct(Guid productId, Product product)
    {
      if (productId != product.Id)
      {
        throw new ArgumentException($"Can't change products Guid");
      }

      if (!_dataContext.Products.ContainsKey(productId))
      {
        throw new ArgumentException($"Product with key '${productId}' does not exist");
      }

      _dataContext.Products[productId] = product;
    }

    public void DeleteProduct(Product product)
    {
      if (!_dataContext.Products.ContainsKey(product.Id))
      {
        throw new ArgumentException($"Product does not exist");
      }

      _dataContext.Products.Remove(product.Id);
    }

    public IEnumerable<Offer> GetAllOffers()
    {
      return _dataContext.Offers;
    }

    public Offer GetOffer(Guid id)
    {
      return _dataContext.Offers.FirstOrDefault(offer => offer.Id == id);
    }

    public void AddOffer(Offer offer)
    {
      if (GetOffer(offer.Id) != null)
      {
        throw new ArgumentException("Offer with this guid already exists");
      }
      _dataContext.Offers.Add(offer);
    }

    public void UpdateOffer(Guid offerId, Offer offer)
    {
      int id = _dataContext.Offers.FindIndex(o => o.Id == offerId);
      if (id == -1)
      {
        throw new ArgumentException($"Offer with id {offerId} does not exist");
      }

      _dataContext.Offers[id] = offer;
    }

    public void DeleteOffer(Offer offer)
    {
      if (!_dataContext.Offers.Remove(offer))
      {
        throw new ArgumentException("Offer does not exist");
      }
    }

    public IEnumerable<Invoice> GetAllInvoices()
    {
      return _dataContext.Invoices;
    }

    public Invoice GetInvoice(Guid id)
    {
      return _dataContext.Invoices.FirstOrDefault(invoice => invoice.Id == id);
    }

    public void AddInvoice(Invoice invoice)
    {
      if (GetInvoice(invoice.Id) != null)
      {
        throw new ArgumentException("Invoice with this guid already exists");
      }

      _dataContext.Invoices.Add(invoice);
    }

    public void DeleteInvoice(Invoice invoice)
    {
      if (!_dataContext.Invoices.Remove(invoice))
      {
        throw new ArgumentException("Invoice does not exist");
      }
    }

    public void UpdateInvoice(Guid invoiceId, Invoice invoice)
    {
      Invoice invoiceInCollection = _dataContext.Invoices.FirstOrDefault(i => i.Id == invoiceId);
      if (invoiceInCollection == null)
      {
        throw new ArgumentException("Invoice does not exist");
      }

      int id = _dataContext.Invoices.IndexOf(invoiceInCollection);
      _dataContext.Invoices[id] = invoice;
    }
  }
}
