using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStore.Entities;
using System;
using System.Linq;
using static System.Linq.Enumerable;


namespace OnlineStore.UnitTest
{
  [TestClass]
  public class RandomDataFillerTest
  {

    [TestMethod]
    public void RandomDataFiller_FillsData()
    {
      RandomDataFiller filler = new RandomDataFiller(10, 20, 30);
      DataRepository dataRepository = new DataRepository(filler);

      Assert.AreEqual(10, dataRepository.GetAllClients().Count());
      Assert.AreEqual(20, dataRepository.GetAllProducts().Count());
      Assert.AreEqual(20, dataRepository.GetAllOffers().Count());
      Assert.AreEqual(30, dataRepository.GetAllInvoices().Count());
    }
  }

  class RandomDataFiller : IDataFiller
  {
    private int _numClients;
    private int _numProducts;
    private int _numInvoices;
    public RandomDataFiller(int clients, int products, int invoices)
    {
      _numClients = clients;
      _numProducts = products;
      _numInvoices = invoices;
    }

    public void Fill(DataContext context)
    {
      Random random = new Random();
      foreach (int index in Range(1, _numClients))
      {
        context.Clients.Add(new Client
        {
          Email = $"test-{index}@xd.com",
          Name = $"Jan{index}",
          Surname = $"Kowalski{index}",
          Address = new Address
          {
            City = $"Kalisz{index}",
            PostalCode = $"{index % 100}-{index % 1000}",
            Street = $"Sezamkowa {index % 123}"
          }
        });
      }

      foreach (int index in Range(1, _numProducts))
      {
        Guid productGuid = Guid.NewGuid();
        Product product = new Product
        {
          Id = productGuid,
          Category = $"Crypto Index {index}",
          Description = $"Nice description {index}",
          Name = $"Name {index}"
        };
        context.Products.Add(productGuid, product);

        Guid offerGuid = Guid.NewGuid();
        context.Offers.Add(new Offer
        {
          Id = offerGuid,
          Count = random.Next(1, 100),
          Price = new decimal(random.NextDouble()) * 100.0M,
          Product = product,
          Tax = new decimal(random.NextDouble())
        });
      }

      foreach (int index in Range(1, _numInvoices))
      {
        Guid invoiceGuid = Guid.NewGuid();
        context.Invoices.Add(new Invoice
        {
          Id = invoiceGuid,
          Client = context.Clients[random.Next(0, context.Clients.Count)],
          Offer = context.Offers[random.Next(0, context.Offers.Count)],
          PurchaseTime = new DateTimeOffset(1990, 12, 30, 0, 0, 0, new TimeSpan()).AddDays(random.Next(1, 6000))
        });
      }
    }
  }
}
