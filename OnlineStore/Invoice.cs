using System;
using System.Collections.Generic;

namespace OnlineStore
{
  public class Invoice
  {
    public Client Client { get; set; }
    public DateTime PurchaseTime { get; set; }
    public uint Count { get; set; }
    public List<Item> Items { get; set; }

    public class Item
    {
      public Offer Offer { get; set; }
      public uint Count { get; set; }
    }
  }
}
