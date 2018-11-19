using System.Collections.Generic;

namespace DataLoaderLibrary.Services
{
    public interface ILoaderService<T> where T : class
    {
        IEnumerable<T> GetQueryResults(string sqlExpression);
    }
}
