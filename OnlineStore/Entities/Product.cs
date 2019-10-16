﻿using System;

namespace OnlineStore.Entities
{
  public class Product
  {
    public Product()
    {
      Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public override string ToString()
    {
      return $"Id: {Id} | Name: {Name} | Description: {Description} | Category: {Category}";
    }
  }
}
