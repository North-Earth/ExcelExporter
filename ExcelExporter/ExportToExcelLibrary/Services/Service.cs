using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExportToExcelLibrary.Services
{
    public class Service : IService
    {
        public void CreateExcelFile(IEnumerable<dynamic> exportedData, string path, string sheetName)
        {           
            // Вызывает исключение, если выгрузке неприсвоено значение.
            if (exportedData == null)
                throw new ArgumentNullException();

            // Вызывает исключение, если в выгрузке нет значений.
            if (exportedData.Count() < 1)
                throw new NullReferenceException(message: "Нет значений для выгрузки, возможно запрос возвращает 0 строк.");

            // Перегруженный метод перезаписывает отстутсвующие объекты в string.Empty(пустую строку).
            exportedData = exportedData.RemoveNullParams();
            
            // Создаёт Excel документ. 
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                Row workbookRow = new Row();

                // Определяет заголовки.
                foreach (var cellHead in exportedData.FirstOrDefault())
                {
                    workbookRow.Append(ConstructCell(value: cellHead.Key, dataType: CellValues.String));
                }

                // Записывает строку в файл.
                sheetData.AppendChild(workbookRow);

                // Наполняет таблицу.
                foreach (var row in exportedData)
                {
                    workbookRow = new Row();

                    foreach (var cell in (IDictionary<string, object>)row)
                    {
                        CellValues cellValues;

                        // Определяет тип объекта.
                        switch (cell.Value)
                        {
                            case DateTime dt:
                                cellValues = CellValues.Date;
                                break;
                            case string s:
                                cellValues = CellValues.String;
                                break;
                            case int i:
                                cellValues = CellValues.Number;
                                break;
                            case bool b:
                                cellValues = CellValues.Boolean;
                                break;
                            default:
                                cellValues = default;
                                break;
                        }

                        // Записывает объект с присвоеным ему типом.
                        workbookRow.Append(ConstructCell(value: cell.Value.ToString(), dataType: cellValues));
                    }

                    // Записывает строку в файл.
                    sheetData.AppendChild(workbookRow);
                }

                // Сохраняет результат.
                worksheetPart.Worksheet.Save();
            }
        }

        private Cell ConstructCell(string value, CellValues dataType)
            => new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
    }
}
