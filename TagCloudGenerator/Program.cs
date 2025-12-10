using System;
using Autofac;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.DI;

namespace TagCloudGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<TagCloudModule>();

            var container = builder.Build();

            using var scope = container.BeginLifetimeScope();
            var client = scope.Resolve<IClient>();
            client.Run(args);
        }
    }
}