using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftServe_BackEnd;
using SoftServe_BackEnd.Controllers;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Helpers;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;
using SoftServe_BackEndTest;
using Xunit;

namespace SoftServe_BackEndTest
{
    public class EventControllerTests
    {
        private readonly DatabaseContext _dbContext;

        private readonly EventController _target;

        public EventControllerTests()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = builder.Options;
            _dbContext = new DatabaseContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _target = new EventController(_dbContext);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _target.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetEvent_EventExist_ReturnsOkObjectResultWithEvent(int eventId)
        {
            // ARRANGE
            List<Event> events = new()
            {
                new Event
                {
                    Id = 1,
                    Name = "event1"
                },
                new Event
                {
                    Id = 2,
                    Name = "event2"
                }
            };

            await _dbContext.Events.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync();

            // ACT
            IActionResult result = await _target.GetEvent(eventId);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            Response<Event> returnedResponse = (Response<Event>)okObjectResult.Value;
            Event returnedEvent = returnedResponse.Data;
            Assert.Equal(eventId, returnedEvent.Id);
        }

        [Fact]
        public async Task PostEvent_ReturnsOkObjectResultWithPostedEvent()
        {
            // ARRANGE

            CreateEvent eventToPost = new()
            {
                Name = "event1",
                Description = "description1",
                Place = "place1",
                Type = TypeOfVolunteer.Culture
            };

            // ACT
            ActionResult<Event> result = await _target.PostEvent(eventToPost);

            // ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
            OkObjectResult okObjectResult = (OkObjectResult)result.Result;
            Response<CreateEvent> returnedResponse = (Response<CreateEvent>)okObjectResult.Value;
            CreateEvent returnedEvent = returnedResponse.Data;
            Assert.Equal(eventToPost.Name, returnedEvent.Name);
            Assert.Equal(eventToPost.Description, returnedEvent.Description);
            Assert.Equal(eventToPost.Place, returnedEvent.Place);
            Assert.Equal(eventToPost.Type, returnedEvent.Type);
        }

        [Fact]
        public async Task PostEvent_AddsEventToDatabase()
        {
            // ARRANGE

            CreateEvent eventToPost = new()
            {
                Name = "event1",
                Description = "description1",
                Place = "place1",
                Type = TypeOfVolunteer.Culture
            };

            // ACT
            ActionResult<Event> result = await _target.PostEvent(eventToPost);

            // ASSERT
            Assert.Equal(1, _dbContext.Events.Count());
        }
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        public async Task DeleteEvent_AfterAddRangeAsync_CheckCountEvents(int idEvent)
        {
            // ARRANGE
            List<Event> events = new()
            {
                new Event
                {
                    Id = 10,
                    Name = "event1"
                },
                new Event
                {
                    Id = 20,
                    Name = "event2"
                },
                new Event
                {
                    Id = 30,
                    Name = "event3"
                }
            };

            // ACT
            await _dbContext.Events.AddRangeAsync(events); 
            await _target.DeleteEvent(idEvent);

            // ASSERT
            Assert.Equal(events.Count() - 1, _dbContext.Events.Count());
        }
        [Fact]
        public async Task DeleteEvent_AfterAddAsync_CheckResultNull()
        {
            // ARRANGE
            Event eventToDelete = new Event
            {
                Id = 1,
                Place = "Lviv",
                Date = DateTime.Now,
                Description = "desc",
                Name = "name",
                Type = TypeOfVolunteer.Eco,
                CreatedBy = "user",
            };

            await _dbContext.Events.AddAsync(eventToDelete);
            await _dbContext.SaveChangesAsync();

            // ACT
            _dbContext.Remove(eventToDelete);
            await _dbContext.SaveChangesAsync();
            var result = _dbContext.Events.FindAsync(eventToDelete.Id).Result;

            // ASSERT
            Assert.Null(result);
        }
        [Fact]
        public async Task GetEvent_EventDoesNotExist_ReturnsNotFound()
        {
            // ARRANGE
            List<Event> events = new()
            {
                new Event
                {
                    Id = 1,
                    Place = "Lviv"
                },
                new Event
                {
                    Id = 2,
                    Type = TypeOfVolunteer.Families
                }
            };

            await _dbContext.Events.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync();

            // ACT
            IActionResult result = await _target.GetEvent(3);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task UpdateEvent_AfterAddAsync()
        {
            // ARRANGE
            Event eventToAdd = new Event
            {
                Id = 1,
                Place = "place",
                Date = DateTime.Now,
                Description = "description1",
                Name = "Name1",
                Type = TypeOfVolunteer.Eco,
                CreatedBy = "User1"
            };
            Event eventToPut = new Event
            {
                Id = 2,
                Place = "Lviv",
                Date = DateTime.Now,
                Description = "description2",
                Name = "Name2",
                Type = TypeOfVolunteer.Eco,
                CreatedBy = "User2"
            };
            
            // ACT
            await _dbContext.Events.AddAsync(eventToAdd);
            await _dbContext.SaveChangesAsync();
            await _target.PutEvent(2, eventToPut);
            var res = _dbContext.Events.FindAsync(2).Result;
            
            // ASSERT
            Assert.Equal(eventToPut.Place, res.Place);
            Assert.Equal(1, _dbContext.Events.Count());
        }
    }
}