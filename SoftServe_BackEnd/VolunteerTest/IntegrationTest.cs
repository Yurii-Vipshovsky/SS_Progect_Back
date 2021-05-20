using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftServe_BackEnd;
using SoftServe_BackEnd.Controllers;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Models;


namespace VolunteerTest
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
        
        protected async Task<Event> CreatePostAsync(Event events)
        {
            var response = await TestClient.PostAsJsonAsync("api/Event", events);
            return await response.Content.ReadFromJsonAsync<Event>();
            //return await response.Content.ReadAsAsync<Event>();
        }
        
        protected Event GetNewEvent(int id = 1, string createdBy = "createdBy", string name = "name", 
            string place = "plase", string description = "description",
            DateTime date = default, TypeOfVolunteer type = TypeOfVolunteer.Eco, Client client = default)
        {
            var events = new Event
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
            return events;
        }
    }
}