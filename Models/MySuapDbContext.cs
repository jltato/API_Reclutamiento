using Microsoft.EntityFrameworkCore;

namespace API_Reclutamiento.Models
{
    public class MySuapDbContext : DbContext
    {
        public MySuapDbContext(DbContextOptions<MySuapDbContext> options) : base(options) { }

    }
}