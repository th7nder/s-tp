using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.ConsoleInterface
{
  class DataService
  {
    private DataRepository _dataRepository;
    public DataService(DataRepository dataRepository)
    {
      this._dataRepository = dataRepository;
    }
  }
}
