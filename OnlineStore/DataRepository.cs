namespace OnlineStore
{
  public class DataRepository
  {
    private readonly DataContext _dataContext = new DataContext();
    private readonly IDataFiller _dataFiller;
    internal DataRepository(IDataFiller dataFiller)
    {
      _dataFiller = dataFiller;
      _dataFiller.Fill(_dataContext);
    }

    public static DataRepository CreatePredefinedDataRepository => new DataRepository(new PredefinedDataFiller());
  }
}
