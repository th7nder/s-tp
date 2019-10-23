using Store.Entities;
using System;
using System.Collections.Generic;

namespace Store
{
  public interface IDataRepository
  {
    event EventHandler InvoiceAdded;
    event EventHandler InvoiceDeleted;
    void AddClient(Client client);
    void AddInvoice(Invoice invoice);
    void AddOffer(Offer offer);
    void AddProduct(Product product);
    void DeleteClient(Client client);
    void DeleteInvoice(Invoice invoice);
    void DeleteOffer(Offer offer);
    void DeleteProduct(Product product);
    IEnumerable<Client> GetAllClients();
    IEnumerable<Invoice> GetAllInvoices();
    IEnumerable<Offer> GetAllOffers();
    IEnumerable<Product> GetAllProducts();
    Client GetClient(string email);
    Invoice GetInvoice(Guid id);
    Offer GetOffer(Guid id);
    Product GetProduct(Guid productId);
    void UpdateClient(string email, Client client);
    void UpdateInvoice(Guid invoiceId, Invoice invoice);
    void UpdateOffer(Guid offerId, Offer offer);
    void UpdateProduct(Guid productId, Product product);
  }
}