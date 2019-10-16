namespace OnlineStore.ConsoleInterface
{
  class Program
  {
    static void Main(string[] args)
    {
      DataService dataService = new DataService(new DataRepository(new EmptyDataFiller()));
    }

    private class EmptyDataFiller : IDataFiller
    {
      public void Fill(DataContext context)
      {
      }
    }
  }
}
