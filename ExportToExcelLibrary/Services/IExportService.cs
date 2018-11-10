using System.Collections.Generic;

namespace ExportToExcelLibrary.Services
{
    public interface IExportService
    {
        void CreateExcelFile(IEnumerable<dynamic> exportedData, string path, string sheetName);
    }
}
