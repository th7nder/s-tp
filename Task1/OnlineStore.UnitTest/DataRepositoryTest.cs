using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public void GetClientByEmail_ExistingMail_ReturnsClient()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
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
      Assert.AreEqual("vsph@mail.com", dataRepository.GetClient("vsph@mail.com").Email);
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
        Name = "Jan",
        Surname = "Stepniak",
        Email = "jkowalski@ff.pl",
        Address = new Address
        {
          City = "Kalisz",
          PostalCode = "62-800",
          Street = "Kaliska 1"
        }
      };

      dataRepository.UpdateClient("jkowalski@ff.pl", newClient);
      Assert.AreEqual("Jan", dataRepository.GetClient("jkowalski@ff.pl").Name);
    }

    [TestMethod]
    public void UpdateClient_ChangeMail_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client newClient = new Client
      {
        Email = "jkowalski-changed@ff.pl"
      };

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateClient("jkowalski@ff.pl", newClient));
    }

    [TestMethod]
    public void UpdateClient_NonExistingMail_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateClient("fail", new Client { }));
    }

    [TestMethod]
    public void DeleteClient_ExistingClient_Deletes()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client client = dataRepository.GetClient("jkowalski@ff.pl");

      dataRepository.DeleteClient(client);
      Assert.AreEqual(0, dataRepository.GetAllClients().Count());
    }

    [TestMethod]
    public void DeleteClient_NonExistingClient_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteClient(new Client { }));
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

      Assert.IsNull(dataRepository.GetProduct(Guid.NewGuid()));
    }

    [TestMethod]
    public void GetProduct_ValidKey_ReturnsProduct()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Guid productGuid = dataRepository.GetAllProducts().First().Id;

      Assert.AreEqual("Bitcoin", dataRepository.GetProduct(productGuid).Name);
    }


    [TestMethod]
    public void AddProduct_ExistingKey_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Guid productGuid = dataRepository.GetAllProducts().First().Id;

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddProduct(new Product { Id = productGuid }));
    }

    [TestMethod]
    public void AddProduct_NonExistingKey_AddsProduct()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      dataRepository.AddProduct(new Product
      {
        Id = Guid.NewGuid(),
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

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateProduct(Guid.NewGuid(), new Product { }));
    }

    [TestMethod]
    public void UpdateProduct_ExistingKey_Updates()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Product product = dataRepository.GetAllProducts().First();
      product.Description = "Changed Description";

      dataRepository.UpdateProduct(product.Id, product);

      Assert.AreEqual("Changed Description", dataRepository.GetProduct(product.Id).Description);
    }


    [TestMethod]
    public void DeleteProduct_NonExistent_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteProduct(new Product { Id = Guid.NewGuid() }));
    }

    [TestMethod]
    public void DeleteProduct_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Product product = dataRepository.GetAllProducts().First();

      dataRepository.DeleteProduct(product);

      Assert.AreEqual(0, dataRepository.GetAllProducts().Count());
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
      Guid offerId = dataRepository.GetAllOffers().First().Id;

      Offer offer = dataRepository.GetOffer(offerId);

      Assert.IsNotNull(offer);
    }

    [TestMethod]
    public void AddOffer_ValidData_CreatesOffer()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      dataRepository.AddOffer(new Offer
      {
        Id = Guid.NewGuid(),
        Product = dataRepository.GetAllProducts().First(),
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

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddOffer(dataRepository.GetAllOffers().First()));
    }

    [TestMethod]
    public void UpdateOffer_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Offer offer = dataRepository.GetAllOffers().First();
      offer.Count = 66;

      dataRepository.UpdateOffer(offer.Id, offer);

      Assert.AreEqual(66, dataRepository.GetAllOffers().First().Count);
    }

    public void UpdateOffer_NonExisting_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateOffer(Guid.NewGuid(), new Offer { }));
    }


    [TestMethod]
    public void DeleteOffer_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Offer offer = dataRepository.GetAllOffers().First();

      dataRepository.DeleteOffer(offer);

      Assert.AreEqual(0, dataRepository.GetAllOffers().Count());
    }

    [TestMethod]
    public void DeleteOffer_InvalidData_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteOffer(new Offer { }));
    }

    [TestMethod]
    public void GetAllInvoices_ReturnsAllInvoices()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      IEnumerable<Invoice> invoices = dataRepository.GetAllInvoices();

      Assert.AreEqual(1, invoices.Count());
    }

    [TestMethod]
    public void GetInvoice_ExistingInvoice_ReturnsInvoice()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Guid invoiceId = dataRepository.GetAllInvoices().First().Id;

      Invoice invoice = dataRepository.GetInvoice(invoiceId);

      Assert.IsNotNull(invoice);
    }

    [TestMethod]
    public void GetInvoice_NotExistingInvoice_ReturnsNull()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Invoice invoice = dataRepository.GetInvoice(Guid.NewGuid());

      Assert.IsNull(invoice);
    }

    [TestMethod]
    public void AddInvoice_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client client = dataRepository.GetAllClients().First();
      Offer offer = dataRepository.GetAllOffers().First();

      dataRepository.AddInvoice(new Invoice
      {
        Id = Guid.NewGuid(),
        Client = client,
        PurchaseTime = DateTimeOffset.Now,
        Offer = offer
      });

      Assert.AreEqual(2, dataRepository.GetAllInvoices().Count());
    }


    [TestMethod]
    public void AddInvoice_InvalidData_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Guid invoiceGuid = dataRepository.GetAllInvoices().First().Id;

      Invoice invoice = new Invoice
      {
        Id = invoiceGuid
      };

      Assert.ThrowsException<ArgumentException>(() => dataRepository.AddInvoice(invoice));
      Assert.AreEqual(1, dataRepository.GetAllInvoices().Count());
    }

    [TestMethod]
    public void RemoveInvoice_InvalidData_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.DeleteInvoice(new Invoice { Id = Guid.NewGuid() }));
    }

    [TestMethod]
    public void RemoveInvoice_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      dataRepository.DeleteInvoice(dataRepository.GetAllInvoices().First());
      Assert.AreEqual(0, dataRepository.GetAllInvoices().Count());
    }

    [TestMethod]
    public void UpdateInvoice_ValidData_Succeeds()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Invoice invoice = dataRepository.GetAllInvoices().First();
      invoice.PurchaseTime = DateTimeOffset.MinValue;

      dataRepository.UpdateInvoice(invoice.Id, invoice);
      Assert.AreEqual(DateTimeOffset.MinValue, dataRepository.GetAllInvoices().First().PurchaseTime);
    }

    [TestMethod]
    public void UpdateInvoice_InvalidData_Throws()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());

      Assert.ThrowsException<ArgumentException>(() => dataRepository.UpdateInvoice(Guid.NewGuid(), new Invoice { }));
    }

    [TestMethod]
    public void InvoiceAdded_Event()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Client client = dataRepository.GetAllClients().First();
      Offer offer = dataRepository.GetAllOffers().First();

      bool called = false;
      dataRepository.InvoiceAdded += (object sender, EventArgs ags) => called = true;
      dataRepository.AddInvoice(new Invoice
      {
        Id = Guid.NewGuid(),
        Client = client,
        PurchaseTime = DateTimeOffset.Now,
        Offer = offer
      });

      Assert.AreEqual(true, called);
    }

    [TestMethod]
    public void InvoiceDeleted_Event()
    {
      DataRepository dataRepository = new DataRepository(new TestDataFiller());
      Invoice invoice = dataRepository.GetAllInvoices().First();
      bool called = false;
      dataRepository.InvoiceDeleted += (object sender, EventArgs ags) => called = true;

      dataRepository.DeleteInvoice(invoice);

      Assert.AreEqual(true, called);
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

      Guid productGuid = Guid.NewGuid();
      context.Products.Add(productGuid, new Product { Id = productGuid, Name = "Bitcoin", Category = "Virtual Currency", Description = "The most expensive virtual currency" });


      Guid offerGuid = Guid.NewGuid();
      context.Offers.Add(new Offer
      {
        Id = offerGuid,
        Price = 33869.72M,
        Count = 10,
        Product = context.Products[productGuid],
        Tax = 0.10M
      });


      Guid invoiceGuid = Guid.NewGuid();
      context.Invoices.Add(new Invoice
      {
        Id = invoiceGuid,
        Client = context.Clients[0],
        Offer = context.Offers[0],
        PurchaseTime = new DateTime(2019, 10, 9, 23, 04, 38)
      });
    }
  }
}
