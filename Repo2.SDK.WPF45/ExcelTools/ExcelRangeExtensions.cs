using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Repo2.SDK.WPF45.ExcelTools
{
    public static class ExcelRangeExtensions
    {
        public static float H1FontSize = 13;
        public static float H2FontSize = 10;


        public static ExcelRange AlignCenter(this ExcelRange rnge)
        {
            rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return rnge;
        }
        public static ExcelRange AlignLeft(this ExcelRange rnge)
        {
            rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            return rnge;
        }
        public static ExcelRange AlignRight(this ExcelRange rnge)
        {
            rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            return rnge;
        }
        public static ExcelRange AlignMiddle(this ExcelRange rnge)
        {
            rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            return rnge;
        }



        public static ExcelRange WriteText(this ExcelRange rnge, string text)
        {
            rnge.Value = text;
            return rnge;
        }
        public static ExcelRange WriteDate(this ExcelRange rnge, DateTime date, string format = "d MMM yyyy")
        {
            rnge.Formula = $"=DATE({date.Year},{date.Month},{date.Day})";
            return rnge.FormatNumber(format)
                       .AlignCenter();
        }
        public static ExcelRange WriteNumber(this ExcelRange rnge, double? number, string format = "#,##0.00  ")
        {
            rnge.Value = number;
            return rnge.FormatNumber(format)
                       .AlignRight();
        }
        public static ExcelRange WriteFormula(this ExcelRange rnge, string formula, string format = "#,##0.00  ")
        {
            rnge.Formula = formula;
            return rnge.FormatNumber(format)
                       .AlignRight();
        }
        public static ExcelRange WriteSumFormula(this ExcelRange rnge, string startAddress, string endAddress, string format = "#,##0.00  ")
        {
            var formula = $"=SUM({startAddress}:{endAddress})";
            return rnge.WriteFormula(formula, format);
        }



        public static ExcelRange SetBold(this ExcelRange rnge, bool isBold = true)
        {
            rnge.Style.Font.Bold = isBold;
            return rnge;
        }
        public static ExcelRange SetItalic(this ExcelRange rnge, bool isItalic = true)
        {
            rnge.Style.Font.Italic = isItalic;
            return rnge;
        }
        public static ExcelRange WrapText(this ExcelRange rnge, bool isWrapText = true)
        {
            rnge.Style.WrapText = isWrapText;
            return rnge;
        }
        public static ExcelRange FontSize(this ExcelRange rnge, float fontSize)
        {
            rnge.Style.Font.Size = fontSize;
            return rnge;
        }



        public static ExcelRange FormatNumber(this ExcelRange rnge, string numberFormat)
        {
            rnge.Style.Numberformat.Format = numberFormat;
            return rnge;
        }
        public static ExcelRange FormatH1(this ExcelRange rnge)
            => rnge.WrapText().AlignCenter()
                   .SetBold().FontSize(H1FontSize);

        public static ExcelRange FormatH2(this ExcelRange rnge)
            => rnge.WrapText().AlignCenter()
                   .SetBold().FontSize(H2FontSize)
                   .SetItalic();

    }
}
