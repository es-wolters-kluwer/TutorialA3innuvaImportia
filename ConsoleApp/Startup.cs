namespace ConsoleApp
{
    using System.IO;
    using a3innuva.Tutorial.Implementations;
    using a3innuva.Tutorial.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {

        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationRoot>(this.Configuration);

            
            services.AddTransient<IUserDataEntity, UserDataEntity>();
            services.AddTransient<IDataGenerator, DataGenerator>();
            services.AddTransient<IZipUtil, ZipUtil>();

            services.AddTransient<App>();

        }
    }


}
