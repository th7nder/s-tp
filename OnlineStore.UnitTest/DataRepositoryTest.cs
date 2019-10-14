using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Entities;

namespace OnlineStore.UnitTest
{
  [TestClass]
  public class DataRepositoryTest
  {
    [TestMethod]
    public void GetAllClients_ReturnsAllClients()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Assert.AreEqual(1, dataRepository.GetAllClients().Count());
    }

    [TestMethod]
    public void GetClient_ReturnsClient()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Assert.AreEqual("jkowalski@ff.pl", dataRepository.GetClient(0).Email);
    }

    [TestMethod]
    public void GetClientByEmail_ExistingMail_ReturnsClient()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Assert.IsNull(dataRepository.GetClient("test@xd.com"));
      Assert.IsNotNull(dataRepository.GetClient("jkowalski@ff.pl"));
    }

    [TestMethod]
    public void GetClientByEmail_NonExistingMail_ReturnsNull()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Assert.IsNull(dataRepository.GetClient("test@xd.com"));
    }

    [TestMethod]
    public void AddClient_ValidClient_AddsClient()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client newClient = new Client
      {
        Name = "Konrad",
        Surname = "Stepniak",
        Email = "vsph@mail.com",
        Address = new Address
        {
          City = "Kalisz",
          PostalCode = "62-800",
          Street = "Kaliska 1"
        }
      };

      dataRepository.AddClient(newClient);
      Assert.AreEqual(2, dataRepository.GetAllClients().Count());
      Assert.AreEqual("vsph@mail.com", dataRepository.GetClient(1).Email);
    }

    [TestMethod]
    public void AddClient_InvalidClient_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client newClient = new Client
      {
        Name = "Konrad",
        Surname = "Stepniak",
        Email = "vsph@mail.com",
        Address = new Address
        {
          City = "Kalisz",
          PostalCode = "62-800",
          Street = "Kaliska 1"
        }
      };
      dataRepository.AddClient(newClient);

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddClient(newClient));
    }

    [TestMethod]
    public void UpdateClient_ExistingClient_Updates()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client newClient = new Client
      {
        Name = "Konrad",
        Surname = "Stepniak",
        Email = "vsph@mail.com",
        Address = new Address
        {
          City = "Kalisz",
          PostalCode = "62-800",
          Street = "Kaliska 1"
        }
      };

      dataRepository.UpdateClient(0, newClient);
      Assert.AreEqual("vsph@mail.com", dataRepository.GetClient(0).Email);
    }

    [TestMethod]
    public void DeleteClient_ExistingClient_SetsDeleted()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client newClient = new Client
      {
        Name = "Konrad",
        Surname = "Stepniak",
        Email = "vsph@mail.com",
        Address = new Address
        {
          City = "Kalisz",
          PostalCode = "62-800",
          Street = "Kaliska 1"
        }
      };
      dataRepository.AddClient(newClient);
      dataRepository.DeleteClient(newClient);

      Assert.AreEqual(1, dataRepository.GetAllClients().Count());
    }

    [TestMethod]
    public void DeleteClient_ExistingClientAssignedToInvoice_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client client = dataRepository.GetClient(0);

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteClient(client));
    }

    [TestMethod]
    public void GetAllProducts_ReturnsAllProducts()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      IEnumerable<Product> products = dataRepository.GetAllProducts();
      Assert.AreEqual(1, products.Count());
    }

    [TestMethod]
    public void GetProduct_NonExistentKey_ReturnsNull()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.IsNull(dataRepository.GetProduct("non-existent key"));
    }

    [TestMethod]
    public void GetProduct_ValidKey_ReturnsProduct()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Assert.AreEqual("Bitcoin", dataRepository.GetProduct("btc").Name);
    }


    [TestMethod]
    public void AddProduct_ExistingKey_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddProduct("btc", new Product { }));
    }

    [TestMethod]
    public void AddProduct_NonExistingKey_AddsProduct()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      dataRepository.AddProduct("ltc", new Product
      {
        Name = "Litecoin",
        Category = "Virtual Currency",
        Description = "LTC"
      });

      Assert.AreEqual(2, dataRepository.GetAllProducts().Count());
    }


    [TestMethod]
    public void UpdateProduct_NonExistingKey_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateProduct("invalid-key", new Product { }));
    }

    [TestMethod]
    public void UpdateProduct_ExistingKey_Updates()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Product product = dataRepository.GetProduct("btc");
      product.Description = "Changed Description";

      dataRepository.UpdateProduct("btc", product);

      Assert.AreEqual("Changed Description", dataRepository.GetProduct("btc").Description);
    }

    [TestMethod]
    public void DeleteProduct_UsedByOffers_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteProduct("btc"));
    }

    [TestMethod]
    public void DeleteProduct_NonExistent_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteProduct("non-existent-key"));
    }

    [TestMethod]
    public void DeleteProduct_NotUsed_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      dataRepository.AddProduct("test", new Product
      {
      });

      dataRepository.DeleteProduct("test");

      Assert.AreEqual(1, dataRepository.GetAllProducts().Count());
    }

    [TestMethod]
    public void GetAllOffers_ReturnsAllOffers()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.AreEqual(1, dataRepository.GetAllOffers().Count());
    }

    [TestMethod]
    public void GetOffer_ExistingOffer_ReturnsOffer()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Offer offer = dataRepository.GetOffer(0);
      Assert.IsNotNull(offer);
    }

    [TestMethod]
    public void AddOffer_ValidData_CreatesOffer()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      dataRepository.AddOffer(new Offer
      {
        Product = dataRepository.GetProduct("btc"),
        Count = 10,
        Price = 3000.0M,
        Tax = 0.23M
      });

      Assert.AreEqual(2, dataRepository.GetAllOffers().Count());
    }

    [TestMethod]
    public void AddOffer_InvalidData_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddOffer(new Offer()));
    }

    [TestMethod]
    public void UpdateOffer_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Offer offer = dataRepository.GetOffer(0);
      offer.Count = 66;

      dataRepository.UpdateOffer(0, offer);

      Assert.AreEqual(66, dataRepository.GetOffer(0).Count);
    }

    [TestMethod]
    public void DeleteOffer_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Offer offer = new Offer
      {
        Product = dataRepository.GetProduct("btc"),
        Count = 10,
        Price = 3000.0M,
        Tax = 0.23M
      };
      dataRepository.AddOffer(offer);
      dataRepository.DeleteOffer(offer);

      Assert.AreEqual(1, dataRepository.GetAllOffers().Count());
    }
  }

  public class TestDataFiller : IDataFiller
  {
    public void Fill(DataContext context)
    {
      context.Clients.Add(
        new Client
        {
          Name = "Jan",
          Surname = "Kowalski",
          Email = "jkowalski@ff.pl",
          Address = new Address
          {
            Street = "Klonowa 10",
            City = "Pabianice",
            PostalCode = "12-345"
          }
        });

      context.Products.Add("btc", new Product { Name = "Bitcoin", Category = "Virtual Currency", Description = "The most expensive virtual currency" });

      context.Offers.Add(new Offer
      {
        Price = 33869.72M,
        Count = 10,
        Product = context.Products["btc"],
        Tax = 0.10M
      });

      context.Invoices.Add(new Invoice
      {
        Client = context.Clients[0],
        Items = new List<Invoice.Item>
        {
          new Invoice.Item
          {
            Count = 1,
            Offer = context.Offers[0],
          }
        },
        PurchaseTime = new DateTime(2019, 10, 9, 23, 04, 38)
      });
    }
  }
}
