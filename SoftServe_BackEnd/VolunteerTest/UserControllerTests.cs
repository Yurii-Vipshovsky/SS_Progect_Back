using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using SoftServe_BackEnd;
using SoftServe_BackEnd.Controllers;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Helpers;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;

namespace VolunteerTest
{
    public class UserControllerTests
    {   [Fact]
        public async Task GetEvents_SortByIsEmpty_AssignsIdToSortBy()
        {
            
            var data = new List<Event>
            {
                new Event
                {
                    Date = DateTime.Now,
                    Description = "description",
                    Name = "Norbert",
                    Place = "Lviv",
                    CreatedBy = "user@example.com",
                    Type = TypeOfVolunteer.Culture,
                    CreatedByNavigation = new Client(),
                    Id = 1
                },
                new Event
                {
                    Date = DateTime.Now,
                    Description = "description",
                    Name = "Norbert",
                    Place = "Lviv",
                    CreatedBy = "user@example.com",
                    Type = TypeOfVolunteer.Culture,
                    CreatedByNavigation = new Client(),
                    Id = 2
                },
                new Event
                {
                    Date = DateTime.Now,
                    Description = "description",
                    Name = "Norbert",
                    Place = "Lviv",
                    CreatedBy = "user@example.com",
                    Type = TypeOfVolunteer.Culture,
                    CreatedByNavigation = new Client(),
                    Id = 3
                }
            }.AsQueryable();
            
        }


        //[Fact]
        // public void Test_FindClientByEmail()
        // {
        //     var options = new DbContextOptionsBuilder<DatabaseContext>()
        //         .UseInMemoryDatabase(databaseName: nameof(Test_FindClientByEmail))
        //         .Options;
        //
        //     var inMemoryDb = new DatabaseContext(options);
        //     
        //     
        //     inMemoryDb.Clients.Add(new Client()
        //     {
        //         
        //         Birthday = Convert.ToDateTime("2021-05-07"),
        //         City = "NewYork",
        //         Email = "norbert@gmail.com",
        //         Events = null,
        //         Login = "string",
        //         Name = "string",
        //         Password = "name",
        //         Site = "string",
        //         IsOrganization = true,
        //         PhoneNumber = "+380669045711"
        //         
        //     });
        //     
        //     inMemoryDb.SaveChanges();
        //     
        //     if (!inMemoryDb.Clients.Any())
        //     {
        //         throw new Exception("Number of client is null");
        //     }
        //
        //     
        //     Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        //
        //     UserController userController = new UserController(inMemoryDb, mockSection.Object);
        //     var a = userController.FindClientByEmail("norbert@gmail.com");
        //     
        //     Assert.Equal("NewYork",a.City);
        //
        // }
        
         [Fact]
         public void Test_Logout()
         {
             var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName:nameof(Test_Logout))
                 .Options;
             
             var inMemoryDb = new DatabaseContext(options);
             
             Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        
             UserController userController = new UserController(inMemoryDb, mockSection.Object);
             var a = userController.Logout().GetAwaiter().GetResult();
             
             var okResult = a is ObjectResult;
             
             Assert.True(okResult);
             
         }
         
         [Fact]
         public void Test_Login()
         {
             LoginUser loginUser = new LoginUser()
             { 
                 Password = "$2a$11$GZDGChc7FApXPcixGy8wAe0hFM1uDvr8Car/iCH6cN0T81DtIpn9e",
                 LoginString = "string"
                 
             };
             
             var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: nameof(Test_Login))
                 .Options;
             
             var inMemoryDb = new DatabaseContext(options);
             
             inMemoryDb.Clients.Add(new Client()
             {
                
                 Birthday = Convert.ToDateTime("2021-05-07"),
                 City = "NewYork",
                 Email = "norbert@gmail.com",
                 Events = null,
                 Login = "string",
                 Name = "string",
                 Password = "$2a$11$GZDGChc7FApXPcixGy8wAe0hFM1uDvr8Car/iCH6cN0T81DtIpn9e",
                 Site = "string",
                 IsOrganization = 1 == 1,
                 PhoneNumber = "+380669045711"
                
             });
             
             inMemoryDb.SaveChanges();
             
             Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
             //mockSection.Setup(x => x.GetSection("Authentication:JWT:SecurityKey"))
             //    .Returns(new ConfigurationSection("ThisIsMyCustomSecretKeyForAuthnetication"));
             UserController userController = new UserController(inMemoryDb, mockSection.Object);
             var a = userController.Login(loginUser).GetAwaiter().GetResult();
             var res = a is OkResult;
             Assert.True(res);
             
         }
         
         [Fact]
         public void Test_Registration()
         {
             RegisterUser loginUser = new RegisterUser()
             { 
                 Birthday = Convert.ToDateTime("2021-05-07"),
                 City = "NewYork",
                 Email = "norbert@gmail.com",
                 Password = "$2a$11$GZDGChc7FApXPcixGy8wAe0hFM1uDvr8Car/iCH6cN0T81DtIpn9e",
                 FullName = "string",
                 IsOrganization = true,
                 PhoneNumber = "+380669045711",
                 NickName = "string",
                 SiteUrl = "string"
                 
             };
             
             var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName:nameof(Test_Registration))
                 .Options;
             
             var inMemoryDb = new DatabaseContext(options);
             
             inMemoryDb.SaveChanges();
             
             Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            
            
             UserController userController = new UserController(inMemoryDb, mockSection.Object);
             
             var a = userController.Register(loginUser).GetAwaiter().GetResult();
             bool successful = inMemoryDb.Clients.Any(x => x.Login == "string");

             
             var res =(OkObjectResult) a;
             
             Assert.True( successful);
             Assert.Equal( 200,res.StatusCode);
             
         }
         
         
    }
}