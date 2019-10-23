using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnlineStore
{
  public class DataContext
  {
    public List<Client> Clients { get; } = new List<Client>();
    public Dictionary<Guid, Product> Products { get; } = new Dictionary<Guid, Product>();
    public List<Offer> Offers { get; } = new List<Offer>();
    public ObservableCollection<Invoice> Invoices { get; } = new ObservableCollection<Invoice>();
  }
}
