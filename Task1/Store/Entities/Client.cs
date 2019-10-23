namespace Store.Entities
{
  public class Client
  {
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Address Address { get; set; }

    public override string ToString()
    {
      return $"Email: {Email} | Name: {Name} | Surname: {Surname} | Address: {Address}";
    }
  }

  public class Address
  {
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }

    public override string ToString()
    {
      return $"Street: {Street} | City: {City} | PostalCode: {PostalCode}";
    }
  }
}
