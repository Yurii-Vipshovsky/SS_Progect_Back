using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Helpers;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;

namespace SoftServe_BackEnd.Controllers
{
    [Route("/[controller]")]
    public class EventController : Controller
    {
        private DatabaseContext _context;
        public EventController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] PaginationFilter filter, [FromQuery] string sortBy, [FromQuery] string search,
            [FromQuery] string order = "asc")
        {
            sortBy = string.IsNullOrEmpty(sortBy) ? "Id" : sortBy;
            var sortByProperty = typeof(Event).GetProperty(sortBy);

            var resultList = new List<Event>();

            if (search != null)     
                await foreach (var currentEvent in _context.Events)
                {
                    if (currentEvent.ToString().Contains(search))
                    {
                        resultList.Add(currentEvent);
                    }
                }
            else
            {
                resultList = _context.Events.ToList();
            }

            var pageFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var pagedData = resultList
                .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
                .Take(pageFilter.PageSize).ToList().OrderBy(employee =>
                    sortByProperty?.GetValue(employee)).ToList();
            if (order == "desc")
            {
                pagedData.Reverse();
            }

            var totalRecords = await _context.Events.CountAsync();
            var pagedResponse = Pagination.CreatePagedResponse(pagedData, pageFilter, totalRecords);

            if (pagedResponse.TotalRecords == 0)
            {
                pagedResponse.Message = "Page is empty";
            }

            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            if (!EventExist(id))
            {
                return NotFound();
            }

            var currentEvent = await _context.Events.FindAsync(id);
            return Ok(new Response<Event>(currentEvent));
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent([FromBody]CreateEvent eventInfo)
        {
            var newEvent = new Event
            {
                CreatedBy = eventInfo.CreatedBy,
                CreatedByNavigation = _context.Clients.FirstOrDefault(
                    clientModel => clientModel.Email == eventInfo.CreatedBy
                ),
                Date = DateTime.Now,
                Description = eventInfo.Description,
                Name = eventInfo.Name,
                Place = eventInfo.Place,
                Type = eventInfo.Type

            };
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return Ok(new Response<CreateEvent>(eventInfo));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, [FromBody]Event eventInfo)
        {
            _context.Entry(eventInfo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExist(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(new Response<Event>(eventInfo));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var currentEvent = await _context.Events.FindAsync(id);

            if (currentEvent == null)
            {
                return NotFound();
            }

            _context.Events.Remove(currentEvent);
            await _context.SaveChangesAsync();

            return Ok(new Response<Event>
            {
                Message = "Successfully deleted"
            });
        }
        
        private bool EventExist(long id) =>
            _context.Events.Any(e => e.Id == id);
    }
}