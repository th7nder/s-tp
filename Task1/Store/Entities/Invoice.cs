using System;

namespace Store.Entities
{
  public class Invoice
  {
    public Guid Id { get; set; }
    public Client Client { get; set; }
    public DateTimeOffset PurchaseTime { get; set; }
    public Offer Offer { get; set; }

    public override string ToString() => $"Id: {Id} | Client: {Client.Email} | PurchaseTime: {PurchaseTime} | Offer: {Offer}";
  }
}
