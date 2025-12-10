using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Algorithms;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
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

            builder.RegisterType<BasicTagCloudAlgorithm>().As<ITagCloudAlgorithm>();

            builder.RegisterType<LinearFontSizeCalculator>()
               .As<IFontSizeCalculator>()
               .WithParameter("minFontSize", 12f)
               .WithParameter("maxFontSize", 72f);

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
