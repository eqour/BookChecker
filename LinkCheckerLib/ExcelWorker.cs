using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LinkCheckerLib
{
    public class ExcelWorker : IDisposable
    {
        private Excel.Application _application;
        private Excel.Workbooks _workbooks;
        private Excel.Workbook _workbook;
        private Excel.Sheets _worksheets;
        private Excel.Worksheet _worksheet;

        public static List<string[,]> ReadDataInRangeFromWorkbook(string path, int rowIndex, int columnIndex, int width, int height)
        {
            List<string[,]> worksheetsData = new List<string[,]>();
            ExcelWorker excelWorker = new ExcelWorker();

            if (!excelWorker.SetWorkbook(path))
            {
                excelWorker.Dispose();
                return null;
            }

            int currentWidth = width;
            int currentHeight = height;

            for (int i = 0; i < excelWorker.GetNumberOfWorksheets(); i++)
            {
                if (width == -1)
                    currentWidth = excelWorker.GetMaxBusyRowInWorksheet(i + 1);

                if (height == -1)
                    currentHeight = excelWorker.GetMaxBusyColumnInWorksheet(i + 1);

                worksheetsData.Add(excelWorker.ReadDataFromRange(rowIndex, columnIndex, currentWidth, currentHeight, i + 1));
            }

            excelWorker.Dispose();

            return worksheetsData;
        }

        public static bool WriteDataInRangeFromWorkbook(string path, List<string[,]> worksheetsData, int rowIndex, int columnIndex, string saveAsPath = null)
        {
            ExcelWorker excelWorker = new ExcelWorker();

            excelWorker.SetWorkbook(path);

            for (int i = 0; i < excelWorker.GetNumberOfWorksheets(); i++)
                worksheetsData.Add(excelWorker.WriteDataFromRange(worksheetsData[i], rowIndex, columnIndex, i + 1));

            if (saveAsPath != null)
            {
                if (excelWorker.SaveAs(saveAsPath) == false)
                {
                    excelWorker.Dispose();
                    return false;
                }
            }

            excelWorker.Dispose();
            return true;
        }

        public ExcelWorker()
        {
            _application = new Excel.Application();
            _application.Visible = false;
            _application.DisplayAlerts = false;

            _workbooks = _application.Workbooks;
        }

        /// <summary>
        /// Выбирает рабочую книгу (открывеает для работы с ней)
        /// </summary>
        /// <param name="path">Путь к файлу рабочей книги</param>
        /// <returns>Удалось ли открыть рабочую книгу</returns>
        public bool SetWorkbook(string path)
        {
            try
            {
                if (_workbook == null)
                {
                    _workbook = _workbooks.Open(path);
                    _worksheets = _workbook.Worksheets;
                    return true;
                }
                else
                {
                    Marshal.FinalReleaseComObject(_worksheets);
                    _worksheets = null;
                    _workbook.Save();
                    _workbook.Close();
                    Marshal.FinalReleaseComObject(_workbook);
                    _workbook = null;

                    _workbook = _workbooks.Open(path);
                    _worksheets = _workbook.Worksheets;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Получает количество листов в рабочей книге
        /// </summary>
        /// <returns>Количество листов в рабочей книге</returns>
        public int GetNumberOfWorksheets()
        {
            if (_workbook != null)
            {
                Excel.Sheets worksheets = _workbook.Worksheets;
                int count = worksheets.Count;

                Marshal.FinalReleaseComObject(worksheets);

                return count;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Получает длину самой длинной строки на рабочем листе
        /// </summary>
        /// <param name="worksheetNumber">Номер рабочего листа</param>
        /// <returns>Длина самой длинной строки</returns>
        public int GetMaxBusyRowInWorksheet(int worksheetNumber)
        {
            SetWorksheet(worksheetNumber);

            if (_workbook != null)
            {
                return _worksheet.UsedRange.Columns.Count;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Получает длину самого длинного столбца на рабочем листе
        /// </summary>
        /// <param name="worksheetNumber">Номер рабочего листа</param>
        /// <returns>Длина самого длинного столбца</returns>
        public int GetMaxBusyColumnInWorksheet(int worksheetNumber)
        {
            SetWorksheet(worksheetNumber);

            if (_workbook != null)
            {
                return _worksheet.UsedRange.Rows.Count;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Получает двумерный массив строк из диапазона в листе
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер столбца</param>
        /// <param name="width">Ширина диапазона</param>
        /// <param name="height">Высота диапазона</param>
        /// <param name="worksheet">Рабочий лист</param>
        /// <returns>Двумерный массив строк</returns>
        public string[,] ReadDataFromRange(int rowIndex, int columnIndex, int width, int height, int worksheetNumber)
        {
            SetWorksheet(worksheetNumber);

            string[,] data = new string[height, width];

            for (int i = rowIndex; i < rowIndex + height; i++)
                for (int j = columnIndex; j < columnIndex + width; j++)
                    data[i - rowIndex, j - columnIndex] = GetCellValue(i, j);

            return data;
        }

        /// <summary>
        /// Записывает двумерный массив строк диапазон на листе
        /// </summary>
        /// <param name="data">Двумерный массив строк</param>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер столбца</param>
        /// <param name="worksheet">Рабочий лист</param>
        /// <returns></returns>
        public string[,] WriteDataFromRange(string[,] data, int rowIndex, int columnIndex, int worksheetNumber)
        {
            SetWorksheet(worksheetNumber);

            for (int i = rowIndex; i < rowIndex + data.GetLength(0); i++)
                for (int j = columnIndex; j < columnIndex + data.GetLength(1); j++)
                    SetCellValue(data[i - rowIndex, j - columnIndex], i, j);

            return data;
        }

        public bool SaveAs(string path)
        {
            if (_workbook == null)
                return false;

            try
            {
                _workbook.SaveAs(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Открывает рабочий лист
        /// </summary>
        /// <param name="number">Номер рабочего листа (начиная с 1)</param>
        /// <returns>Удалось ли открыть рабочий лист</returns>
        private bool SetWorksheet(int number)
        {
            try
            {
                if (_worksheet == null)
                {
                    _worksheet = _worksheets[number];
                    return true;
                }
                else
                {
                    Marshal.FinalReleaseComObject(_worksheet);
                    _worksheet = null;

                    _worksheet = _worksheets[number];
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Получает значение ячейки листа
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер столбца</param>
        /// <param name="worksheet">Рабочий лист</param>
        /// <returns></returns>
        private string GetCellValue(int rowIndex, int columnIndex)
        {
            string cellValue = "";

            Excel.Range range = _worksheet.Cells;
            Excel.Range cellRange = range[rowIndex, columnIndex];

            if (cellRange.Value != null)
                cellValue = cellRange.Value.ToString();

            Marshal.FinalReleaseComObject(range);
            Marshal.FinalReleaseComObject(cellRange);

            return cellValue;
        }

        /// <summary>
        /// Устанавливает значение ячейки листа
        /// </summary>
        /// <param name="value">Значение, которое необходимо установить</param>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер столбца</param>
        /// <param name="worksheet">Рабочий лист</param>
        private void SetCellValue(string value, int rowIndex, int columnIndex)
        {
            Excel.Range range = _worksheet.Cells;
            Excel.Range cellRange = range[rowIndex, columnIndex];

            cellRange.Value = value;

            Marshal.FinalReleaseComObject(range);
            Marshal.FinalReleaseComObject(cellRange);
        }

        public void Dispose()
        {
            if (_worksheet != null)
            {
                Marshal.FinalReleaseComObject(_worksheet);
            }

            if (_workbook != null)
            {
                Marshal.FinalReleaseComObject(_worksheets);
                _workbook.Save();
                _workbook.Close(0);
                Marshal.FinalReleaseComObject(_workbook);
                _workbook = null;
            }

            _workbooks.Close();
            Marshal.FinalReleaseComObject(_workbooks);

            _application.Application.Quit();
            _application.Quit();
            Marshal.FinalReleaseComObject(_application);
            _application = null;
        }

        ~ExcelWorker()
        {
            if (_workbook != null)
                Dispose();
        }
    }
}
