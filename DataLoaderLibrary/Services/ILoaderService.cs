using System.Collections.Generic;

namespace DataLoaderLibrary.Services
{
    public interface ILoaderService
    {
        IEnumerable<dynamic> GetQueryResults(string sqlExpression);
    }
}
