﻿using System;
using System.Collections.Generic;

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
    }
  }
}
