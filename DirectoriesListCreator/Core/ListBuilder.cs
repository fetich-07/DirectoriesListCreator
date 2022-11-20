using DirectoriesListBuilder.Helpers;
using Microsoft.AspNetCore.StaticFiles;

namespace DirectoriesListBuilder.Core
{
    public class ListBuilder
    {
        public static List<List<Tuple<string, long, string>>> GetDirectories(string path, string searchPattern)
        {
            try
            {
                var result = new List<List<Tuple<string, long, string>>>();
                var dirs = Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories)
                    .ToList();

                foreach (var dir in dirs)
                {
                    result.Add(GetElementsSizeAndMimeType(dir));
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Для того чтобы иметь доступ к некоторым директорям, необходимо перезапустить приложение на правах администратора!");
                return new List<List<Tuple<string, long, string>>>();
            }
        }


        private static List<Tuple<string, long, string>> GetElementsSizeAndMimeType(string currentDir)
        {
            var result = new List<Tuple<string, long, string>>();
            var currentDirectoryDirs = Directory.GetDirectories(currentDir, "*");
            var currentDirFiles = Directory.GetFiles(currentDir, "*");

            foreach (var dir in currentDirectoryDirs)
            {
                var size = DirSize(new DirectoryInfo(dir)) / 1024;
                result.Add(Tuple.Create($"-директория- {new DirectoryInfo(dir).Name}", size, ""));
            }

            foreach (var file in currentDirFiles)
            {
                var name = new FileInfo(file).Name;
                long size = new FileInfo(file).Length / 1024;
                var provider = new FileExtensionContentTypeProvider();
                string? mimeType;
                if (provider.TryGetContentType(file, out mimeType))
                {
                    MimeTypesCountHelper.DirectoryFiles?.Add(new Core.File()
                    {
                        MimeTypeName = mimeType,
                        SizeInKB = size
                    });
                }

                result.Add(Tuple.Create($"-файл- {name}", size, mimeType));
            }

            return result;
        }

        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }
    }
}
