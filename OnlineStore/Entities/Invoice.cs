using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Entities
{
  public class Invoice
  {
    public int Id { get; set; }
    public Client Client { get; set; }
    public DateTime PurchaseTime { get; set; }
    public List<Item> Items { get; set; }

    public class Item
    {
      public Offer Offer { get; set; }
      public int Count { get; set; }
      public override string ToString()
      {
        return $"Offer: {Offer.Id} | Count: {Count}";
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder($"Id: {Id} | Client: {Client.Email} | PurchaseTime: {PurchaseTime}");
      foreach (Item item in Items)
      {
        stringBuilder.Append($"\n\t{item}");
      }

      return stringBuilder.ToString();
    }
  }
}
