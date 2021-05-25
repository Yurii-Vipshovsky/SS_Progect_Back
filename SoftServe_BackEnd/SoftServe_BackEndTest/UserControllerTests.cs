using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using SoftServe_BackEnd;
using SoftServe_BackEnd.Controllers;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Models;

namespace SoftServe_BackEndTest
{
    public class UserControllerTests
    {
        [Fact]
        public void Test_Registration()
        {
            // ARRANGE
            RegisterUser loginUser = new RegisterUser
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
            var userController = new UserController(inMemoryDb, mockSection.Object);
            var a = userController.Register(loginUser).GetAwaiter().GetResult();
            var successful = inMemoryDb.Clients.Any(x => x.Login == "string");
            var res =(OkObjectResult) a;
             
            // ACT
            int correctStatusCode = 200;
             
            // ASSERT
            Assert.True(successful);
            Assert.Equal( correctStatusCode,res.StatusCode);
             
        }
        
        [Fact]
         public void Test_Logout()
         {
             // ARRANGE
             var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName:nameof(Test_Logout))
                 .Options;
             
             var inMemoryDb = new DatabaseContext(options);
             Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        
             // ACT
             var userController = new UserController(inMemoryDb, mockSection.Object);
             var a = userController.Logout().GetAwaiter().GetResult();
             var okResult = a is ObjectResult;
             
             // ASSERT
             Assert.True(okResult);
             
         }
         
         // [Fact]
         // public void Test_Login()
         // {
         //     var loginUser = new LoginUser
         //     { 
         //         Password = "123456",
         //         Email = "norbert@gmail.com"
         //         
         //     };
         //     
         //     var options = new DbContextOptionsBuilder<DatabaseContext>()
         //         .UseInMemoryDatabase(nameof(Test_Login))
         //         .Options;
         //     
         //     var inMemoryDb = new DatabaseContext(options);
         //     
         //     inMemoryDb.Clients.Add(new Client
         //     {
         //         Birthday = Convert.ToDateTime("2021-05-07"),
         //         City = "NewYork",
         //         Email = "norbert@gmail.com",
         //         Events = null,
         //         Login = "string",
         //         Name = "string",
         //         Password = "123456",
         //         Site = "string",
         //         IsOrganization = true,
         //         PhoneNumber = "+380669045711"
         //     });
         //     
         //     inMemoryDb.SaveChanges();
         //     
         //     Mock<Microsoft.Extensions.Configuration.IConfiguration> mockSection = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
         //     //mockSection.Setup(x => x.GetSection("Authentication:JWT:SecurityKey"))
         //     //    .Returns(new ConfigurationSection("ThisIsMyCustomSecretKeyForAuthnetication"));
         //     var userController = new UserController(inMemoryDb, mockSection.Object);
         //     var actionResult = userController.Login(loginUser).GetAwaiter().GetResult();
         //     var res = actionResult is OkResult;
         //     Assert.True(res);
         //     
         // }
         
         
        
    }
}