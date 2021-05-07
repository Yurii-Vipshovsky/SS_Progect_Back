using Microsoft.EntityFrameworkCore;
using SoftServe_BackEnd.Models;

namespace VolunteerTest.Helpers
{
    public class FakeDbSet: DbSet<Event>
    {
        
    }
}