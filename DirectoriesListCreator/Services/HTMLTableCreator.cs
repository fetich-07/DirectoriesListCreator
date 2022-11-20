using DirectoriesListBuilder.Core;
using System.Data;
using System.Drawing;
using System.Text;

namespace DirectoriesListBuilder.Services
{
    public class HtmlTableCreator
    {
        public static StringBuilder GetHtmlPage(
            List<List<Tuple<string, long, string>>> directories,
            List<Tuple<string?, int, double>> ratio,
            List<Tuple<string?, long>> averageFileSize)
        {
            StringBuilder htmlPage = new StringBuilder();

            DataTable table1 = new DataTable();
            table1.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Название", typeof(string)),
                    new DataColumn("Размер",typeof(string)),
                    new DataColumn("MimeType файла",typeof(string))});
            foreach (var directory in directories)
            {
                foreach (var dir in directory)
                {
                    table1.Rows.Add(dir.Item1, $"{dir.Item2}Кб", dir.Item3);
                }
            }
            htmlPage.AppendLine("Директории и файлы данного каталога");
            htmlPage.Append(CreateHtmlTable(table1));

            DataTable table2 = new DataTable();
            table2.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Название типа", typeof(string)),
                    new DataColumn("Количество среди всех типов",typeof(int)),
                    new DataColumn("Процент от всех типов",typeof(string))});
            foreach(var row in ratio)
            {
                table2.Rows.Add(row.Item1, row.Item2, $"{string.Format("{0:0.##}", row.Item3)}%");
            }
            htmlPage.AppendLine("Частота данного типа относительно всей собранной коллекции");
            htmlPage.Append(CreateHtmlTable(table2));
            
            DataTable table3 = new DataTable();
            table3.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("Название типа", typeof(string)),
                    new DataColumn("Средний размер файла",typeof(string))});
            foreach (var size in averageFileSize)
            {
                table3.Rows.Add(size.Item1, $"{size.Item2}Кб") ;
            }
            htmlPage.AppendLine("Cредний размер файла для каждого MimeType");
            htmlPage.Append(CreateHtmlTable(table3));

            return htmlPage;
        }

        private static string CreateHtmlTable(DataTable table)
        {
            var sb = new StringBuilder();

            sb.Append("<table cellpadding='3' cellspacing='3' style='border: 1px solid #800000;font-size: 12pt;font-family:Arial'>");
            //Add Table Header
            sb.Append("<tr>");
            foreach (DataColumn column in table.Columns)
            {
                sb.Append("<th style='background-color: #3366ff;border: 1px solid #000'>" + column.ColumnName + "</th>");
            }
            sb.Append("</tr>");
            //Add Table Rows
            foreach (DataRow row in table.Rows)
            {
                sb.Append("<tr>");
                //Add Table Columns
                foreach (DataColumn column in table.Columns)
                {
                    sb.Append("<td style='width:100px;border: 1px solid #ccc'>" + row[column.ColumnName].ToString() + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            return sb.ToString();

        }
    }
}
