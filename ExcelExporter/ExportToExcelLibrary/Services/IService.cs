using System.Collections.Generic;

namespace ExportToExcelLibrary.Services
{
    public interface IService
    {
        void CreateExcelFile(IEnumerable<dynamic> exportedData, string path, string sheetName);
    }
}
