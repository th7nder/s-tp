using OnlineStore.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnlineStore
{
  public interface IDataFiller
  {
    void Fill(DataContext context);
  }

  public class PredefinedDataFiller : IDataFiller
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
        PurchaseTime = new System.DateTime(2019, 10, 9, 23, 04, 38)
      });
    }
  }
}