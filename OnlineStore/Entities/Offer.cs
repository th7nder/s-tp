using System;

namespace OnlineStore.Entities
{
  public class Offer
  {
    public Offer()
    {
      Id = Guid.NewGuid();
    }
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
