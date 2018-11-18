using System.Collections.Generic;

namespace DataLoaderLibrary.Services
{
    public interface ILoaderService<T> where T : class
    {
        IEnumerable<dynamic> GetQueryResults(string sqlExpression);

        IEnumerable<T> GetQueryResultsForType(string sqlExpression);
    }
}
