﻿using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace OnlineStore
{
  public class DataRepository
  {
    private readonly DataContext _dataContext = new DataContext();
    private readonly IDataFiller _dataFiller;
    public DataRepository(IDataFiller dataFiller)
    {
      _dataFiller = dataFiller;
      _dataFiller.Fill(_dataContext);
    }

    public void AddClient(Client client)
    {
      if (_dataContext.Clients.Any(c => c.Email == client.Email))
      {
        throw new ArgumentException($"Client with email {client.Email} already exists");
      }

      _dataContext.Clients.Add(client);
    }

    public Client GetClient(int id)
    {
      return _dataContext.Clients[id];
    }

    public Client GetClient(string email)
    {
      return _dataContext.Clients.FirstOrDefault(client => client.Email == email);
    }

    public IEnumerable<Client> GetAllClients()
    {
      return _dataContext.Clients;
    }

    public void UpdateClient(int id, Client client)
    {
      _dataContext.Clients[id] = client;
    }

    public void DeleteClient(Client client)
    {
      IEnumerable<Invoice> invoices = GetAllInvoices();
      if (invoices.Any(invoice => invoice.Client.Email == client.Email))
      {
        throw new ArgumentException($"Client is used in an invoice");
      }

      _dataContext.Clients.Remove(client);
    }

    public IEnumerable<Product> GetAllProducts()
    {
      return _dataContext.Products.Values;
    }

    public Product GetProduct(string key)
    {
      try
      {
        return _dataContext.Products[key];
      }
      catch
      {
        return null;
      }
    }

    public void AddProduct(string key, Product product)
    {
      if (_dataContext.Products.ContainsKey(key))
      {
        throw new ArgumentException($"Product with key '${key}' already exists");
      }

      _dataContext.Products.Add(key, product);
    }

    public void UpdateProduct(string key, Product product)
    {
      if (!_dataContext.Products.ContainsKey(key))
      {
        throw new ArgumentException($"Product with key '${key}' does not exist");
      }

      _dataContext.Products[key] = product;
    }

    public void DeleteProduct(string key)
    {
      if (!_dataContext.Products.ContainsKey(key))
      {
        throw new ArgumentException($"Product does not exist");
      }

      Product product = GetProduct(key);
      IEnumerable<Offer> offers = GetAllOffers();
      if (offers.Any(offer => offer.Product.Id == product.Id))
      {
        throw new ArgumentException($"A offer uses this product");
      }

      _dataContext.Products.Remove(key);
    }

    public IEnumerable<Offer> GetAllOffers()
    {
      return _dataContext.Offers;
    }

    public Offer GetOffer(int id)
    {
      return _dataContext.Offers[id];
    }

    public Offer GetOffer(Guid id)
    {
      return _dataContext.Offers.FirstOrDefault(offer => offer.Id == id);
    }

    public void AddOffer(Offer offer)
    {
      if (offer.Product == null)
      {
        throw new ArgumentException("Offer must reference a product");
      }

      _dataContext.Offers.Add(offer);
    }

    public void UpdateOffer(int id, Offer offer)
    {
      _dataContext.Offers[id] = offer;
    }

    public void DeleteOffer(Offer offer)
    {
      IEnumerable<Invoice> invoices = GetAllInvoices();

      if (invoices.Any(invoice => invoice.Items.Any(item => item.Offer.Id == offer.Id)))
      {
        throw new ArgumentException("Offer is used in an invoice");
      }

      _dataContext.Offers.Remove(offer);
    }

    public IEnumerable<Invoice> GetAllInvoices()
    {
      return _dataContext.Invoices;
    }

    public Invoice GetInvoice(int id)
    {
      try
      {
        return _dataContext.Invoices[id];
      } catch
      {
        return null;
      }
    }

    public void AddInvoice(Invoice invoice)
    {
      if (invoice.Client == null)
      {
        throw new ArgumentException("Client cannot be null");
      }

      if (invoice.Items == null || invoice.Items.Count == 0)
      {
        throw new ArgumentException("Invoice needs items");
      }

      if (GetInvoice(invoice.Id) != null)
      {
        throw new ArgumentException($"Invoice with Id '${invoice.Id}' already exists");
      }

      foreach (Invoice.Item item in invoice.Items)
      {
        Offer offer = GetOffer(item.Offer.Id);
        if (offer.Count < item.Count)
        {
          throw new ArgumentException("Cannot create an invoice, there is not enough items in Offer");
        }
        offer.Count -= item.Count;
      }

      _dataContext.Invoices.Add(invoice);
    }

    public void RemoveInvoice(Invoice invoice)
    {
      if (invoice == null)
      {
        throw new ArgumentException("Cannot remove null invoice");
      }

      if (GetInvoice(invoice.Id) == null)
      {
        throw new ArgumentException("Invoice does not exist");
      }

      foreach (Invoice.Item item in invoice.Items)
      {
        Offer offer = GetOffer(item.Offer.Id);
        offer.Count += item.Count;
      }

      _dataContext.Invoices.Remove(invoice);
    }

    public void UpdateInvoice(int id, Invoice invoice)
    {
      _dataContext.Invoices[id] = invoice;
    }
  }
}
