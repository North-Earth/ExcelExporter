using System.Collections.Generic;
using System.Linq;

namespace ExportToExcelLibrary.Services
{
    public static class HelperParser
    {
        public static IEnumerable<dynamic> RemoveNullParams(this IEnumerable<dynamic> rows)
        {
            foreach (var row in rows)
            {
                var item = (IDictionary<string, object>)row;
                foreach (var key in item.Keys.ToList())
                {
                    if (item[key] == null)
                        item[key] = string.Empty;
                }
            }
            return rows;
        }
    }
}
