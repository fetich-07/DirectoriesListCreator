using DirectoriesListBuilder.Core;

namespace DirectoriesListBuilder.Helpers
{
    public static class MimeTypesCountHelper
    {
        public static List<Core.File>? DirectoryFiles= new();

        public static List<Tuple<string?, int, double>> GetQuantitativeAndPercentageRatio()
        {
            var result = new List<Tuple<string?, int, double>>();

            var fileGroups = DirectoryFiles.GroupBy(f => f.MimeTypeName);

            foreach (var group in fileGroups)
            {
                var count = group.Count();
                double percentage = (double)count * 100 / DirectoryFiles.Count();
                result.Add(Tuple.Create(item1: group.Key, item2: count, item3: percentage));
            }

            return result;
        }

        public static List<Tuple<string?, long>> GetAverageFileSize()
        {
            var result = new List<Tuple<string?, long>>();
            var fileGroups = DirectoryFiles.GroupBy(f => f.MimeTypeName);

            foreach (var group in fileGroups)
            {
                var sum = group.Select(c => c.SizeInKB).Sum();
                var averageSize = sum / group.Count();
                result.Add(Tuple.Create(group.Key, averageSize));
            }

            return result;
        }
    }
}
