using OnlineStore.Entities;
using System;
using System.Collections.Generic;
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
      return _dataContext.Clients.Where(client => !client.Deleted);
    }

    public void UpdateClient(int id, Client client)
    {
      _dataContext.Clients[id] = client;
    }

    public void DeleteClient(Client client)
    {
      Client c = GetClient(client.Email);
      if (c == null)
      {
        throw new ArgumentException($"Client '${client.Email}'does not exist in the repository");
      }

      c.Deleted = true;
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
      } catch
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
      if (offers.Any(offer => offer.Product.Id == product.Id)) {
        throw new ArgumentException($"A offer uses this product");
      }

      _dataContext.Products.Remove(key);
    }

    public IEnumerable<Offer> GetAllOffers()
    {
      return _dataContext.Offers;
    }
  } 
}
