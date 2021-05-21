using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftServe_BackEnd;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Models;


namespace SoftServe_BackEndTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        
        protected IntegrationTest(string mode="InMemory")
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<DatabaseContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.RemoveAll(typeof(DatabaseContext));
                    
                    if (mode == "InMemory")
                    {
                        services.AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDB");
                        });
                    }
                    else
                    {
                        services.AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseNpgsql(
                                "User ID=postgres;" +
                                "Password=passwordpassword;" +
                                "Server=localhost;Port=5432;" +
                                "Database=SSDatabase;" +
                                "Integrated Security=true;" +
                                " Pooling=true;"
                            );
                        });
                    }
                });
            });
            TestClient = appFactory.CreateClient();
        }

        protected async Task<JObject> GetPageResponse(Task<HttpResponseMessage> response)
        {
            var pageResponse =  JsonConvert.DeserializeObject(await response.Result.Content.ReadAsStringAsync());
            var jsonResponse = (JObject) pageResponse;
            return jsonResponse;
        }
        
        protected Task<Event> CreatePostAsync(Event events)
        {
            var response = TestClient.PostAsJsonAsync("api/Event", events).Result;
            return response.Content.ReadAsAsync<Event>();
        }
        
        protected static Event GetNewEvent(int id = 1, string createdBy = "createdBy", string name = "name", 
             string place = "place", string description = "description",
             DateTime date = default, TypeOfVolunteer type = TypeOfVolunteer.Eco, Client client = default)
         {
             return new()
             {
                 Id = id,
                 CreatedBy = createdBy,
                 Name = name,
                 Place = place,
                 Description = description,
                 Date = date,
                 Type = type,
                 CreatedByNavigation = client
             };
         }
    }
}