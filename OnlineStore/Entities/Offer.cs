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
  }
}
