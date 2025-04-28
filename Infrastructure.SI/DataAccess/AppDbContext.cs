
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.SI.Extension.Identity;
namespace Infrastructure.SI.DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):
        IdentityDbContext<ApplicationUser>(options)
    {
    }
}
