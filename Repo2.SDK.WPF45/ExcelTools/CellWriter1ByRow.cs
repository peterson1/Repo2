using System;
using System.IO;
using OfficeOpenXml;

namespace Repo2.SDK.WPF45.ExcelTools
{
    public class CellWriter1ByRow
    {
        private ExcelPackage   _pkg;
        private ExcelWorksheet _ws;
        private string         _path;
        private int            _currentRow;
        private int            _currentCol;
        private BorderWriter1  _bordr;


        public CellWriter1ByRow(string filenameSuffix, string extension = "xlsx")
        {
            _path = ComposeFilePath(filenameSuffix, extension);
        }


        //public float H1FontSize { get; set; } = 13;
        //public float H2FontSize { get; set; } = 10;
        public BorderWriter1 Border => _bordr;



        //public bool this[bool 


        public ExcelRange WriteMergedText(string text, int rowSpan, int colSpan)
        {
            var rnge = _ws.Cells[_currentRow, 
                                 _currentCol, 
                                 _currentRow + (rowSpan - 1), 
                                 _currentCol + (colSpan - 1)];
            rnge.Merge = true;
            WriteText(text);

            for (int i = 0; i < rowSpan - 1; i++)
                MoveToNextRow();

            return rnge.AlignCenter();
        }

        public void WriteMergedH1(string headerText, int rowSpan, int colSpan)
            => WriteMergedText(headerText, rowSpan, colSpan).FormatH1();

        public void WriteMergedH2(string headerText, int rowSpan, int colSpan)
            => WriteMergedText(headerText, rowSpan, colSpan).FormatH2();


        #region Cell Value Writers

        public ExcelRange WriteH1(string headerText)
            => WriteText(headerText).FormatH1();

        public ExcelRange WriteH2(string headerText)
            => WriteText(headerText).FormatH2();


        public ExcelRange WriteText(string text)
        {
            var cell = CurrentCell.WriteText(text);
            MoveToNextRow();
            return cell;
        }
        public void WriteDate(DateTime date, string format = "d MMM yyyy")
        {
            CurrentCell.WriteDate(date, format);
            MoveToNextRow();
        }
        public void WriteNumber(double? number, string format = "#,##0.00  ")
        {
            CurrentCell.WriteNumber(number, format);
            MoveToNextRow();
        }

        public ExcelRange WriteSumFormulaForColumn(int startRow, int endRow, string format = "#,##0.00  ")
        {
            var startAddr = _ws.Cells[startRow, _currentCol].Address;
            var endAddr   = _ws.Cells[endRow, _currentCol].Address;
            var cell = CurrentCell.WriteSumFormula(startAddr, endAddr, format);
            MoveToNextRow();
            return cell;
        }

        public ExcelRange WriteSumFormulaForRow(int startCol, int endCol, string format = "#,##0.00  ")
        {
            var startAddr = _ws.Cells[_currentRow, startCol].Address;
            var endAddr   = _ws.Cells[_currentRow, endCol].Address;
            var cell      = CurrentCell.WriteSumFormula(startAddr, endAddr, format);
            MoveToNextRow();
            return cell;
        }

        public ExcelRange WritePercentFormula(string numeratorAddress, string denominatorAddress, string format = "#,##0.00 %  ")
        {
            var fmla = $"={numeratorAddress} / {denominatorAddress}";
            var cell = CurrentCell.WriteFormula(fmla, format);
            MoveToNextRow();
            return cell;
        }

        public ExcelRange WriteDifferenceFormula(string minuendAddress, string subtrahendAddress, string format = "#,##0.00  ")
        {
            var fmla = $"={minuendAddress} - {subtrahendAddress}";
            var cell = CurrentCell.WriteFormula(fmla, format);
            MoveToNextRow();
            return cell;
        }

        #endregion



        #region Range Formatters

        protected virtual void ApplyGlobalCellFormat(ExcelRange rnge)
        {
            rnge.AlignMiddle();
        }

        #endregion



        #region File I/O

        private ExcelPackage CreateExcelPackage()
        {
            Directory.CreateDirectory(OutputDir);
            return new ExcelPackage(new FileInfo(_path));
        }


        public string SaveToFile()
        {
            _pkg.Save();
            return _path;
        }


        private string ComposeFilePath(string filenameSuffix, string extension)
        {
            var nme = GetUniqueFilename(filenameSuffix, extension);
            return Path.Combine(OutputDir, nme);
        }


        private string GetUniqueFilename(string suffix, string extension)
        {
            var d8 = DateTime.Now;
            return $"{d8:yyyy-MM-dd_hhmmss}_{suffix}.{extension}";
        }

        public string OutputDir
            => Path.Combine(Path.GetTempPath(), GetType().Namespace);

        #endregion



        #region Workbook / Worksheet

        public void AddWorksheet(string worksheetName)
        {
            if (_pkg == null)
                _pkg = CreateExcelPackage();

            _ws = _pkg.Workbook.Worksheets.Add(worksheetName);
            ApplyGlobalCellFormat(_ws.Cells);
            _bordr = new BorderWriter1(_ws);
        }

        public void FreezePane(int row, int col)
            => _ws.View.FreezePanes(row, col);

        #endregion



        #region Cursor Methods

        public void MoveTo(int? rowNumber, int? colNumber)
        {
            _currentRow = rowNumber ?? _currentRow;
            _currentCol = colNumber ?? _currentCol;
        }

        public void        MoveToNextRow     () => _currentRow++;
        public void        MoveToNextCol     () => _currentCol++;
        public void        MoveToPreviousRow () => _currentRow--;
        public void        MoveToPreviousCol () => _currentCol--;

        public ExcelRange  CurrentCell       => _ws.Cells [_currentRow, _currentCol];
        public ExcelRow    CurrentRow        => _ws.Row   (_currentRow);
        public ExcelColumn CurrentCol        => _ws.Column(_currentCol);
        public int         CurrentRowNumber  => _currentRow;
        public int         CurrentColNumber  => _currentCol;

        #endregion
    }
}
