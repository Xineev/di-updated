using Autofac;
using TagCloudGenerator.Algorithms;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Services;
using TagCloudGenerator.Infrastructure.Analyzers;
using TagCloudGenerator.Infrastructure.Calculators;
using TagCloudGenerator.Infrastructure.Filters;
using TagCloudGenerator.Infrastructure.Measurers;
using TagCloudGenerator.Infrastructure.Reader;
using TagCloudGenerator.Infrastructure.Renderers;

namespace TagCloudGenerator.DI
{
    public class TagCloudModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LineTextReader>().As<IReader>();

            builder.RegisterType<BoringWordsFilter>().As<IFilter>();
            builder.RegisterType<ToLowerCaseFilter>().As<IFilter>();

            builder.RegisterType<BasicTagCloudAlgorithm>().As<ITagCloudAlgorithm>();

            builder.RegisterType<LinearFontSizeCalculator>()
               .As<IFontSizeCalculator>();

            builder.RegisterType<GraphicsTextMeasurer>()
               .As<ITextMeasurer>();

            builder.RegisterType<WordsFrequencyAnalyzer>()
               .As<IAnalyzer>();

            builder.RegisterType<ConsoleClient>().As<IClient>();

            builder.RegisterType<PngRenderer>().As<IRenderer>();

            builder.RegisterType<CloudGenerator>().As<ITagCloudGenerator>();
        }
    }
}
