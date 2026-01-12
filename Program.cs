using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                System.IO.File.WriteAllText("startup_error.log", ex.ToString());
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:8000", "http://0.0.0.0:8001");
                });
    }
}

