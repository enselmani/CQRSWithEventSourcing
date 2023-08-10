using Microsoft.AspNetCore.Builder;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            WebApplication application = builder
                .ConfigureServices()
                .ConfigurePipeline();

            // Run the application
            application.Run();
        }
    }
}