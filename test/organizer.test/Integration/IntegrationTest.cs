using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Organizer.Context;
using Organizer.Repositories;

namespace Organizer.Test.Integration
{
    public class IntegrationTest
    {
          
        protected async void DoIntegrationTest(Func<HttpClient, DbContextOptions<OrganizerContext> , System.Threading.Tasks.Task>  action)
        {
             
             var path = PlatformServices.Default.Application.ApplicationBasePath;

             IServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var hostBuilder = new WebHostBuilder()
            .ConfigureServices( services =>
            {
                services.AddMvc();
                services.AddDbContext<OrganizerContext>(a =>
                {
                    a.UseInMemoryDatabase();
                    a.UseInternalServiceProvider(serviceProvider);
                    
                });
                services.AddScoped<IViewRepository, ViewRepository>();
                services.AddScoped<IModifyRepository, ModifyRepository>();

            })
            .Configure(app=>
            {
                app.UseMvc();
            })
            .ConfigureLogging(loggerFactory=>
            {
                 loggerFactory.AddDebug();
            })
            .UseContentRoot(path);
           
            TestServer server = new TestServer(hostBuilder);
            
            HttpClient client = server.CreateClient();
            
            DbContextOptionsBuilder<OrganizerContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<OrganizerContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase();
            dbContextOptionsBuilder.UseInternalServiceProvider(serviceProvider);  

            await action(client,dbContextOptionsBuilder.Options);
            
            client.Dispose();
            server.Dispose();
        }            
    }   
}