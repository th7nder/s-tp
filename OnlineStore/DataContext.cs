using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnlineStore
{
  class DataContext
  {
    public List<Client> Clients;
    public Dictionary<uint, Product> Products;
    public List<Offer> Offers;
    public ObservableCollection<Invoice> Invoices;
  }
}
