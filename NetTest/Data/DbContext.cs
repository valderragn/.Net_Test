using Microsoft.EntityFrameworkCore;
using NetTest.Models;

namespace NetTest.Data
{
    

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataRecord> DataRecords { get; set; }
    }

}
