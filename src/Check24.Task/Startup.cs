using System;
using Microsoft.Extensions.DependencyInjection;

namespace Check24.Task
{
    /// <summary>
    /// Bootstrap app configuration
    /// </summary>
    public static class Statup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Add IoC configuration
            serviceCollection.AddScoped<IGameAlgorithm, GameAlgorithm>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
