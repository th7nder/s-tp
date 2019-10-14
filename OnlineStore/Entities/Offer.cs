﻿namespace OnlineStore.Entities
{
  public class Offer
  {
    public Product Product { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public int Count { get; set; }
  }
}
