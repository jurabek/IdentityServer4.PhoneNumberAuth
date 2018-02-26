using IdentityServer4.PhoneNumberAuth.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IdentityServer4.PhoneNumberAuth
{
	public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
	            .MigrateDbContext<ApplicationDbContext>((_, __) => { })
				.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
