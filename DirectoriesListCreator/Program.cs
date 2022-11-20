using DirectoriesListBuilder.Core;
using DirectoriesListBuilder.Helpers;
using DirectoriesListBuilder.Services;
using System.Text;

var currentDir = Directory.GetCurrentDirectory();
var directories = ListBuilder.GetDirectories($"{currentDir}", "*");
var averageFileSize = MimeTypesCountHelper.GetAverageFileSize();
var ratio = MimeTypesCountHelper.GetQuantitativeAndPercentageRatio();

var htmlPage = HtmlTableCreator.GetHtmlPage(directories, ratio, averageFileSize);

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Файл \"DirectoriesList.htm\" создан в текущей директории");
System.IO.File.WriteAllText($"{currentDir}\\DirectoriesList.htm", htmlPage.ToString());
Console.ReadKey();