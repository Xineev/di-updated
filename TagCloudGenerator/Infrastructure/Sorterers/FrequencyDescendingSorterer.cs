using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Sorterers
{
    /// <summary>
    /// Тут пожалуй отвечу насчет использования списка кортежей, насколько я выяснил в интернете, в целом словари можно сортировать,
    /// но вообще говоря Dictionary не гарантирует верный порядок элементов из-за особенностей своего устройства
    /// В большинстве случаев - да, будет сортировать, но гарантий нам никто не дает, особенно на больших словарях
    /// есть SortedDictionary, но он сортирует по ключу, поэтому я решил пойти таким путем - отделить сортировку от анализа частот
    /// </summary>
    public class FrequencyDescendingSorterer : ISorterer
    {
        public List<(string Word, int Frequency)> Sort(Dictionary<string, int> wordsWithFreqs)
        {
            return wordsWithFreqs
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .Select(kvpair => (kvpair.Key, kvpair.Value))
                .ToList();
        }
    }
}
