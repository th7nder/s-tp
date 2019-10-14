namespace OnlineStore.Entities
{
  public class Client
  {
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
  }

  public class Address
  {
      public string Street { get; set; }
      public string City { get; set; }
      public string PostalCode { get; set; }
  }
}
