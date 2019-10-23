using System;

namespace Store.Entities
{
  public class Product
  {
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
