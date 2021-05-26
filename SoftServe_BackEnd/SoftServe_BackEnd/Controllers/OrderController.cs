using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Helpers;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;

namespace SoftServe_BackEnd.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class OrderController : Controller
    {
        private readonly DatabaseContext _context;
        public OrderController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationFilter filter, [FromQuery] string sortBy,
            [FromQuery] string order = "asc")
        {
            var emailOfCurrentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.Clients.FirstOrDefault(
                clientModel => clientModel.Email == emailOfCurrentUser
            );
            
            sortBy = string.IsNullOrEmpty(sortBy) ? "Id" : sortBy;
            var sortByProperty = typeof(Event).GetProperty(sortBy);

            var orderList = new List<Order>();
            await foreach (var currentOrder in _context.Orders)
            {
                if (currentOrder.UserLoginNavigation == currentUser)
                {
                    orderList.Add(currentOrder);
                }
            }

            var resultList = orderList.Select(currentOrder =>
                _context.Events.FirstOrDefault(eventInfo => currentOrder.EventId == eventInfo.Id)).ToList();

            var pageFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var pagedData = resultList
                .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
                .Take(pageFilter.PageSize).ToList().OrderBy(pneEvent =>
                    sortByProperty?.GetValue(pneEvent)).ToList();
            if (order == "desc")
            {
                pagedData.Reverse();
            }

            var totalRecords = resultList.Count;
            var pagedResponse = Pagination.CreatePagedResponse(pagedData, pageFilter, totalRecords);

            if (pagedResponse.TotalRecords == 0)
            {
                pagedResponse.Message = "Page is empty";
            }

            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var emailOfCurrentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.Clients.FirstOrDefault(
                clientModel => clientModel.Email == emailOfCurrentUser
            );
            if (!OrderExist(id))
            {
                return NotFound();
            }
            var currentOrder = await _context.Orders.FindAsync(id);
            if (currentOrder.UserLoginNavigation != currentUser)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response<Client>
                {
                    Message = "Error",
                    Errors = new[] {"Access denied"},
                    Succeeded = false,
                    Data = null
                });
            }
            
            return Ok(new Response<Order>(currentOrder));
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostOrder([FromBody]int eventId)
        {
            var emailOfCurrentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newOrder = new Order
            {
                EventId = eventId,
                Event = _context.Events.FirstOrDefault(currentEvent => currentEvent.Id == eventId),
                UserLogin = emailOfCurrentUser,
                UserLoginNavigation = _context.Clients.FirstOrDefault(
                    clientModel => clientModel.Email == emailOfCurrentUser
                )
            };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return Ok(new Response<Order>
            {
                Message = "Order Created Successful"
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var currentOrder = await _context.Orders.FindAsync(id);

            if (currentOrder == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(currentOrder);
            await _context.SaveChangesAsync();

            return Ok(new Response<Order>
            {
                Message = "Successfully deleted"
            });
        }
        
        private bool OrderExist(long id) =>
            _context.Orders.Any(e => e.OrderId == id);
    }
}