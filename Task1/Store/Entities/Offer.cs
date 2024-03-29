﻿using System;

namespace Store.Entities
{
  public class Offer
  {
    public Guid Id { get; set; }
    public Product Product { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public int Count { get; set; }
    public override string ToString()
    {
      return $"Id: {Id} | Product: {Product.Id} | Price: {Price} | Tax: {Tax} | Count: {Count}";
    }
  }
}
