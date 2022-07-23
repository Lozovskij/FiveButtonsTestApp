using OfficeOpenXml;

namespace ReadersAndWriters.Writers;

public static class ExcelWriter
{
    public static async Task WriteAsync(Article[] articles, string path)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
        }

        using var package = new ExcelPackage(file);
        ExcelWorksheet ws = package.Workbook.Worksheets.Add("Articles");
        ExcelRangeBase range = ws.Cells["A1"].LoadFromCollection(articles, true);

        //style a bit
        ws.Row(1).Style.Font.Bold = true;
        ws.Column(1).AutoFit();
        ws.Column(4).AutoFit();
        ws.Column(5).AutoFit();

        await package.SaveAsync();
    }

}
