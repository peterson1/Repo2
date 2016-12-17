using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Repo2.SDK.WPF45.ExcelTools
{
    public class BorderWriter1
    {
        private ExcelWorksheet _ws;

        public BorderWriter1(ExcelWorksheet worksheet)
        {
            _ws = worksheet;
        }


        public ExcelBorderStyle this[int startRow, int startCol, int endRow, int endCol]
        {
            set
            {
                _ws.Cells[startRow, startCol, endRow, endCol].Style.Border.BorderAround(value);
            }
        }
    }
}
